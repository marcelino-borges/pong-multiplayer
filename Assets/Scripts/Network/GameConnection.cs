using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameConnection : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI debugUIText;
    public readonly string DEFAULT_ROOM_NAME = "PongRoom";    
    public static GameConnection instance;
    public bool isInRoom = false;
    public TextMeshProUGUI player1Name_UI;
    public TextMeshProUGUI player2Name_UI;

    public string PlayerNickName { 
        get => PhotonNetwork.LocalPlayer.NickName; 
        set { PhotonNetwork.LocalPlayer.NickName = value; } 
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if(!player.IsMasterClient)
                player2Name_UI.text = player.NickName;
            else
                player1Name_UI.text = player.NickName;
        }
    }

    public void Init()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        if(PhotonNetwork.InLobby == false)
        {
            PhotonNetwork.JoinLobby();
        }        
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        PhotonNetwork.JoinRoom(DEFAULT_ROOM_NAME);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        //AddDebugText("\nFalha ao criar sala");

        if (returnCode == ErrorCode.GameDoesNotExist)
        {
            RoomOptions room = new RoomOptions { MaxPlayers = 2 };
            PhotonNetwork.CreateRoom(DEFAULT_ROOM_NAME, room, null);
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        isInRoom = true;
    }

    public bool IsRoomFull()
    {
        return PhotonNetwork.CurrentRoom != null && PhotonNetwork.CurrentRoom.PlayerCount == 2;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //SetDebugText("\nVocê saiu da sala " + DEFAULT_ROOM_NAME);
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        //SetDebugText("\nO jogador " + otherPlayer.NickName + " saiu da " + DEFAULT_ROOM_NAME);
    }

    public void SetDebugText(string text)
    {
        if(debugUIText != null)
            debugUIText.text += text;
    }
}
