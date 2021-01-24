using Photon.Pun;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public int belongingPlayerIdentifier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            BallController ball = collision.gameObject.GetComponent<BallController>();
            if(ball != null)
                ball.Restart();
            PlayerController.Restart();            
            LevelManager.instance.IncreaseScore(belongingPlayerIdentifier);            
        }
    }
}
