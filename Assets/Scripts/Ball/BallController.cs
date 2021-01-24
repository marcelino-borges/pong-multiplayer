using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private float moveSpeed = 8f;
    private float initialMoveSpeed;
    private Vector3 initialPosition;
    private Vector2 direction;
    private bool canMove = false;
    [SerializeField]
    private TrailRenderer trail;
    private Vector2 networkPosition;

    public AudioClip ballContact;
    public AudioClip explode;
    public AudioSource audioSource;
    public Rigidbody2D rb2d;
    public PhotonView photonView;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        initialPosition = transform.position;
        initialMoveSpeed = moveSpeed;
        float randomX = GetRandomDirectionAxis();
        float randomY = GetRandomDirectionAxis();
        direction = new Vector2(randomX, randomY);
        Restart();
    }

    private void FixedUpdate()
    {
        if (canMove) 
        {
            if (photonView.IsMine)
            {
                rb2d.velocity = direction * moveSpeed * Time.deltaTime;
            }
            else
            {
                rb2d.position = Vector3.Lerp(rb2d.position, networkPosition, Time.fixedDeltaTime);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb2d.position);
            stream.SendNext(rb2d.velocity);
        }
        else
        {
            networkPosition = (Vector2)stream.ReceiveNext();
            rb2d.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            Vector2 finalVelocity = rb2d.velocity * lag;
            networkPosition += finalVelocity;
        }    
    }

    public void Restart()
    {
        PlaySfx(explode);
        StartCoroutine(RestartCo());
    }

    public IEnumerator RestartCo()
    {
        yield return new WaitForSeconds(1f);
        canMove = false;
        ShowTrail(false);
        transform.position = initialPosition;
        yield return new WaitForSeconds(1f);
        canMove = true;
        ShowTrail(true);
        moveSpeed = initialMoveSpeed;
    }

    private void ShowTrail(bool show)
    {
        trail.gameObject.SetActive(show);
    }

    private float GetRandomDirectionAxis()
    {
        return Random.Range(-1, 1) == -1 ? -1 : 1;
    }

    public void IncrementMoveSpeed()
    {
        moveSpeed *= 1.1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Paddle"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.IncrementMoveSpeed();
            }
            direction.x *= -1;
            IncrementMoveSpeed();
            PlaySfx(ballContact);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            direction.y *= -1;
            PlaySfx(ballContact);
        }
    }

    private void PlaySfx(AudioClip audio)
    {
        if (audioSource != null && ballContact != audio)
            audioSource.PlayOneShot(audio);
    }
}
