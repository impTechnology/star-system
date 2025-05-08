using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [System.Serializable]
    public class GameData
    {
        public string title;
        [TextArea] public string description;
    }

    [Header("UI Referencie")]
    public TMP_Text gameTitleText;
    public TMP_Text gameDescriptionText;
    public Button startButton;

    [Header("Zoznam Buttonov vlavo")]
    public List<Button> gameButtons;

    [Header("Popisy hier (definovane v kóde)")]
    public List<GameData> games;

    private int currentGameIndex = -1;

    void Start()
    {
        for (int i = 0; i < gameButtons.Count; i++)
        {
            int index = i;
            gameButtons[i].onClick.AddListener(() => OnGameSelected(index));
        }

        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    void OnGameSelected(int index)
    {
        if (index < games.Count)
        {
            currentGameIndex = index;

            gameTitleText.text = games[index].title;
            gameDescriptionText.text = games[index].description;
        }
    }

    void OnStartButtonClicked()
    {
        if (currentGameIndex >= 0 && currentGameIndex < games.Count)
        {
            Debug.Log("Spustam hru: " + games[currentGameIndex].title);
        }
        else
        {
            Debug.Log("Ziadna hra nie je vybrata.");
        }
    }
}
