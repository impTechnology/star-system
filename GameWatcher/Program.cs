using System;
using System.Diagnostics;
using System.Threading;
using System.IO;

class Program
{
    // Flag to track if a complete shutdown is requested
    static bool shutdownRequested = false;

    // Flag to track if we're currently in a subgame session
    static bool subgameActive = false;

    static void Main(string[] args)
    {
        // Force the console window to be visible
        Console.Title = "Game Launcher";
        Console.WriteLine("==============================");
        Console.WriteLine("     GAME LAUNCHER STARTED    ");
        Console.WriteLine("==============================");

        // Determine paths
        string executablePath = AppDomain.CurrentDomain.BaseDirectory;
        string managerPath = Path.Combine(executablePath, "StarSystem", "Star System VR-sandbox.exe");
        string subgamesFolder = Path.Combine(executablePath, "StarSystem\\Star System VR-sandbox_Data", "StreamingAssets");

        // Check if paths exist and inform user
        if (!File.Exists(managerPath))
        {
            Console.WriteLine($"ERROR: Manager not found at expected path: {managerPath}");
            Console.WriteLine("Make sure the launcher is in the same directory as the game folders.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine($"Manager path: {managerPath}");
        Console.WriteLine($"Subgames folder: {subgamesFolder}");
        Console.WriteLine("\nLauncher is running and monitoring game state...");
        Console.WriteLine("You can minimize this window but don't close it!");
        Console.WriteLine("\nPress ESC in this window at any time to exit completely.");

        // Start the manager initially
        Process? managerProcess = null;
        Process? subgameProcess = null;

        // Create thread to check for ESC key
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

        try
        {
            // Delete any existing state files when starting
            if (File.Exists("current_subgame.txt"))
                File.Delete("current_subgame.txt");
            if (File.Exists("shutdown_requested.txt"))
                File.Delete("shutdown_requested.txt");

            LaunchManager();

            // Main loop - keep the launcher running and listen for commands
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
                Thread.Sleep(100);
            }

            Console.WriteLine("Launcher is shutting down. Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Critical error: {e.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        // Helper method to launch the manager
        void LaunchManager()
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
        void LaunchSubgame(string subgamePath)
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
    }
}