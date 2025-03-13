using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

class Program
{
    #region COM_VARS
    private static bool shutdownRequested = false;
    private static bool subgameActive = false;
    #endregion

    #region PATHS
    private static string? executablePath;
    private static string? managerPath;
    private static string? subgamesFolder;
    #endregion

    #region GAME PROCESSES
    private static Process? managerProcess = null;
    private static Process? subgameProcess = null;
    #endregion

    private static void InitText()
    {
        Console.Title = "Game Launcher";
        Console.WriteLine("==============================");
        Console.WriteLine("     GAME LAUNCHER STARTED    ");
        Console.WriteLine("==============================");
    }

    private static void GetPaths(string gameFolder, string gameName)
    {
        executablePath = AppDomain.CurrentDomain.BaseDirectory;
        managerPath = Path.Combine(executablePath, gameFolder, gameName + ".exe");
        subgamesFolder = Path.Combine(executablePath, gameFolder + "\\" + gameName + "_Data", "StreamingAssets");
    }

    private static void PrintState()
    {
        Console.WriteLine($"Manager path: {managerPath}");
        Console.WriteLine($"Subgames folder: {subgamesFolder}");
        Console.WriteLine("\nLauncher is running and monitoring game state...");
        Console.WriteLine("You can minimize this window but don't close it!");
        Console.WriteLine("\nPress ESC in this window at any time to exit completely.");
    }

    private static bool CheckPaths()
    {
        if (!File.Exists(managerPath))
        {
            Console.WriteLine($"ERROR: Manager not found at expected path: {managerPath}");
            Console.WriteLine("Make sure the launcher is in the same directory as the game folders.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return false;
        }
        return true;
    }

    // Create thread to check for ESC key
    private static void CreateCloseAction()
    {
        Thread keyListenerThread = new Thread(() =>
        {
            while (!shutdownRequested)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("\n[" + DateTime.Now.ToString("HH:mm:ss") + "] Exit requested. Shutting down...");
                        shutdownRequested = true;

                        // Create shutdown file to signal we want to exit
                        File.WriteAllText("shutdown_requested.txt", "exit");

                        // Kill any running processes if necessary
                        try
                        {
                            if (managerProcess != null && !managerProcess.HasExited)
                                managerProcess.Kill();
                            if (subgameProcess != null && !subgameProcess.HasExited)
                                subgameProcess.Kill();
                        }
                        catch { /* Ignore errors during shutdown */ }

                        break;
                    }
                }
                Thread.Sleep(100);
            }
        });
        keyListenerThread.IsBackground = true;
        keyListenerThread.Start();
    }

    private static void UpdateFiles()
    {
        if (File.Exists("current_subgame.txt"))
            File.Delete("current_subgame.txt");
        if (File.Exists("shutdown_requested.txt"))
            File.Delete("shutdown_requested.txt");
    }

    private static void CheckSubgamePath()
    {
        string subgamePath = File.ReadAllText("current_subgame.txt");
        File.Delete("current_subgame.txt"); // Clean up

        if (File.Exists(subgamePath))
        {
            subgameActive = true;
            LaunchSubgame(subgamePath);
        }
        else
        {
            Console.WriteLine($"Error: Subgame not found at {subgamePath}");
            if (!shutdownRequested)
                LaunchManager(); // Restart manager if subgame not found
        }
    }

    private static void MainLoop()
    {
        while (!shutdownRequested)
        {
            // Check for shutdown request file (can be created by manager or subgame)
            if (File.Exists("shutdown_requested.txt"))
            {
                shutdownRequested = true;
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Shutdown requested by game. Exiting...");
                break;
            }
            // Check if a subgame file has been created
            if (File.Exists("current_subgame.txt") && !shutdownRequested)
            {
                CheckSubgamePath();
            }
            // Check if subgame has exited
            if (subgameActive && subgameProcess != null && subgameProcess.HasExited && !shutdownRequested)
            {
                Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Subgame closed. Returning to manager...");
                Thread.Sleep(1000); // Short delay to prevent issues
                subgameActive = false;
                LaunchManager();
            }
            // Check if manager has exited
            if (managerProcess != null && managerProcess.HasExited)
            {
                // If we weren't launching a subgame and a subgame isn't active
                if (!subgameActive && !File.Exists("current_subgame.txt") && !shutdownRequested)
                {
                    // Brief delay to allow the subgame file to be created if that's what's happening
                    Thread.Sleep(200);

                    // Double-check the subgame file wasn't just created
                    if (!File.Exists("current_subgame.txt") && !shutdownRequested)
                    {
                        // Manager was closed directly, exit launcher
                        Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Manager closed directly. Shutting down launcher...");
                        shutdownRequested = true;
                    }
                }
            }
            // Sleep to prevent high CPU usage
            Thread.Sleep(500);
        }
    }

    // Helper method to launch the manager
    private static void LaunchManager()
    {
        if (shutdownRequested)
            return;

        try
        {
            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + "] Launching game manager...");
            managerProcess = Process.Start(managerPath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error launching manager: {e.Message}");
        }
    }

    // Helper method to launch a subgame
    private static void LaunchSubgame(string subgamePath)
    {
        if (shutdownRequested)
            return;

        try
        {
            Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss") + $"] Launching subgame: {Path.GetFileName(subgamePath)}");
            subgameProcess = Process.Start(subgamePath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error launching subgame: {e.Message}");
            subgameActive = false;
            if (!shutdownRequested)
                LaunchManager(); // Return to manager if subgame launch fails
        }
    }

    static void Main(string[] args)
    {
        InitText();
        GetPaths("StarSystem", "Star System VR-sandbox");

        if (!CheckPaths())
            return;

        PrintState();
        CreateCloseAction();

        try
        {
            UpdateFiles();
            LaunchManager();
            MainLoop();

            Console.WriteLine("Launcher is shutting down. Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Critical error: {e.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}