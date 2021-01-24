using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreHud : MonoBehaviour
{
    public TextMeshProUGUI scorePlayer1_UI;
    public TextMeshProUGUI scorePlayer2_UI;
    public int scorePlayer1;
    public int scorePlayer2;
    public PhotonView photonView;

    public static ScoreHud instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void IncreaseScore(int playerIdentifier)
    {
        if(playerIdentifier == 1)
        {
            scorePlayer1++;
            scorePlayer1_UI.text = scorePlayer1.ToString();
        } 
        else if (playerIdentifier == 2)
        {
            scorePlayer2++;
            scorePlayer2_UI.text = scorePlayer2.ToString();
        }
    }
}
