using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    public TextMeshProUGUI nicknameUI;
    public TextMeshProUGUI logUI;
    public Button readyButton; 

    public void ReadyToPlay()
    {
        if (!string.IsNullOrEmpty(nicknameUI.text))
        {
            if (nicknameUI.text.Length > 3)
            {
                SetPlayerName();
                GameConnection.instance.Init();
                readyButton.gameObject.GetComponent<Image>().color = new Color32(46, 152, 74, 255);
                readyButton.interactable = false;
                StartCoroutine(WaitOtherPlayerToStart());
            } else
            {
                StartCoroutine(ShowMessageCo("YOUR NAME MUST HAVE MORE THAN 3 CHARACTERS"));
            }
            
        } else
        {
            StartCoroutine(ShowMessageCo("YOU NEED A NAME TO START A MATCH"));
        }
    }

    public IEnumerator WaitOtherPlayerToStart()
    {
        StartCoroutine(ShowMessageCo("WAITING ANOTHER PLAYER GET READY"));

        while (!GameConnection.instance.IsRoomFull())
        {
            yield return new WaitForSeconds(.5f);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetPlayerName()
    {
        GameConnection.instance.PlayerNickName = nicknameUI.text;
    }

    private IEnumerator ShowMessageCo(string msg)
    {
        logUI.text = msg;
        yield return new WaitForSeconds(3f);
        logUI.text = "";
    }
}
