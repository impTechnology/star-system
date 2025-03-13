using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;


public class StarSystemController : MonoBehaviour
{
    [SerializeField] private List<StringDictionary> games;

    public void LaunchSubgame(string subgameName)
    {
        StringDictionary gameDictionary = games.Find(g => g.GameName.Equals(subgameName));
        string subgamePath = Path.Combine(Application.streamingAssetsPath, gameDictionary.FolderName, gameDictionary.GameName + ".exe");
        File.WriteAllText("current_subgame.txt", subgamePath);
        Application.Quit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ExitGame();
    }

    public void ExitGame()
    {
        File.WriteAllText("shutdown_requested.txt", "exit");
        Application.Quit();
    }
}

[System.Serializable]
public class StringDictionary
{
    [field: SerializeField] public string FolderName { get; set; }
    [field: SerializeField] public string GameName { get; set; }
}