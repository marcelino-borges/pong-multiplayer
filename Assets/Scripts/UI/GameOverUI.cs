using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI message_UI;
    public static GameOverUI instance;
    public GameObject gameOverPanel;

    private void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void ShowPanel()
    {
        gameOverPanel.SetActive(true);
    }

    public void SetWinner(int playerNumber)
    {
        string playerName = "";
        
        if(playerNumber == 1)
            playerName = "LEFT PLAYER";
        else
            playerName = "RIGHT PLAYER";

        message_UI.text = playerName + " HAS WON!";
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
}
