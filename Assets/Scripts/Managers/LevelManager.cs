using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int maxScore = 10;

    public static LevelManager instance;

    public Transform player1_Spawn;
    public Transform player2_Spawn;
    public Transform ball_Spawn;

    public PhotonView photonView;

    private void Start()
    {
        if(instance == null)
            instance = this;

        photonView = GetComponent<PhotonView>();


        if (PhotonNetwork.IsMasterClient)
        {
            GameObject player1 = PhotonNetwork.Instantiate("Paddle", player1_Spawn.position, Quaternion.identity);
            player1.GetComponent<PlayerController>().playerIdentifier = 1;
            PhotonNetwork.Instantiate("Ball", ball_Spawn.position, Quaternion.identity);
        } else
        {
            GameObject player2 = PhotonNetwork.Instantiate("Paddle", player2_Spawn.position, Quaternion.identity);
            player2.GetComponent<PlayerController>().playerIdentifier = 2;
        }
    }

    public void IncreaseScore(int playerIdentifier)
    {
        if(photonView.IsMine)
            ScoreHud.instance.photonView.RPC("IncreaseScore", RpcTarget.All, playerIdentifier);

        int playerScore = playerIdentifier == 1 ? ScoreHud.instance.scorePlayer1 : ScoreHud.instance.scorePlayer2;

        if (playerScore >= maxScore)
        {
            GameOverUI.instance.SetWinner(playerIdentifier);
            GameOverUI.instance.ShowPanel();
        }
    }
}
