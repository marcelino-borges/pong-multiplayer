using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerController : MonoBehaviour, IPunObservable
{
    private readonly float DELTA_Y = 4.83f;
    private static float initialMoveSpeed;

    //Lag compensation
    private Vector3 latestPos;
    private Quaternion latestRot;
    private float currentTime = 0;
    private double currentPacketTime = 0;
    private double lastPacketTime = 0;
    private Vector3 positionAtLastPacket = Vector3.zero;
    private Quaternion rotationAtLastPacket = Quaternion.identity;

    public PhotonView photonView;
    public Vector3 initialPosition;
    public static float moveSpeed = 5f;
    public int playerIdentifier = 0;

    private void Start()
    {
        initialMoveSpeed = moveSpeed;
        initialPosition = transform.position;
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            transform.Translate(ReadInputs() * moveSpeed * Time.deltaTime);
            transform.position = new Vector3(
                transform.position.x,
                Mathf.Clamp(transform.position.y, -DELTA_Y, DELTA_Y),
                transform.position.z
            );
        }
        else
        {
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
            transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
        }
    }

    [PunRPC]
    private Vector2 ReadInputs()
    {
        if (Input.GetKey(KeyCode.W))
            return Vector2.up;
        else if (Input.GetKey(KeyCode.S))
            return Vector2.down;

        return Vector2.zero;
    }

    public void IncrementMoveSpeed()
    {
        moveSpeed *= 1.1f;
    }

    public static void Restart()
    {
        moveSpeed = initialMoveSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot  = (Quaternion)stream.ReceiveNext();

            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            positionAtLastPacket = transform.position;
            rotationAtLastPacket = transform.rotation;
        }
    }
}
