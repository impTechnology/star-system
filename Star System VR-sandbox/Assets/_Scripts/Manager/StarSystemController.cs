using System.IO;
using UnityEngine;
using System.Collections.Generic;


public class StarSystemController : MonoBehaviour
{
    public static StarSystemController Instance { get; private set; }
    [SerializeField] private List<StringDictionary> games;
    void Awake()
    {
        Instance = this;
    }
    public void LaunchSubgame(string subgameName)
    {
        string subgamePath = null;
        StringDictionary gameDictionary = games.Find(g => g.GameName.Equals(subgameName));
        Debug.Log(gameDictionary);
        if (gameDictionary != null)
            subgamePath = Path.Combine(Application.streamingAssetsPath, gameDictionary.FolderName, gameDictionary.GameName + ".exe");
        else
        {
            Debug.Log("[STAR-SYSTEM-CONTROLLER] [ERROR]: Subgame not found.");
            return;
        }
#if UNITY_EDITOR 
        Debug.Log("[STAR-SYSTEM-CONTROLLER] [INFO]: Selected Subgame -> {" + gameDictionary.GameName + "},{" + gameDictionary.FolderName + "}");
#else
        File.WriteAllText("current_subgame.txt", subgamePath);
        Application.Quit();
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ExitGame();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        Debug.Log("[STAR-SYSTEM-CONTROLLER] [INFO]: Selected Subgame -> {QUITING STAR SYSTEM}");
#else
        File.WriteAllText("shutdown_requested.txt", "exit");
        Application.Quit();
#endif
    }
}

[System.Serializable]
public class StringDictionary
{
    [field: SerializeField] public string FolderName { get; set; }
    [field: SerializeField] public string GameName { get; set; }
}