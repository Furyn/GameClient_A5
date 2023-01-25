using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float boundaryValue = 5f;
    private Vector3 direction;

    private bool hasHitPlayer = false;
    private float timeSinceHasHitPlayer = 0f;

    void Start()
    {
        //direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f));
        direction = new Vector2(-1f, 0f);
    }

    public void UpdatePhysics(Player player1, Player player2, float elapsedTime)
    {
        HitPlayer(player1);
        HitPlayer(player2);
        HitBoundaries();

        transform.position += direction * speed * elapsedTime;

        if (hasHitPlayer)
        {
            timeSinceHasHitPlayer += elapsedTime;
            if (timeSinceHasHitPlayer >= 0.1f)
            {
                hasHitPlayer = false;
                timeSinceHasHitPlayer = 0f;
            }
        }

    }

    private void HitBoundaries()
    {
        if (transform.position.y >= boundaryValue || transform.position.y <= -boundaryValue)
        {
            direction.y = -direction.y;
        }
    }

    private void HitPlayer(Player player)
    {
        if (hasHitPlayer)
            return;

        float distance = transform.position.x - player.transform.position.x;
        if ( distance > 0.25f || distance < -0.25f)
            return;

        // Partie centre Player 
        if (transform.position.y > player.transform.position.y - 0.5f && transform.position.y < player.transform.position.y + 0.5f)
        {
            Debug.Log("Hit center");
            direction.x = -direction.x;
        }
        // Partie basse Player 
        else if (transform.position.y < player.transform.position.y - 0.5f && transform.position.y > player.transform.position.y - 1f)
        {
            Debug.Log("Hit low");
            direction.x = -direction.x;
        }
        // Partie haute Player 
        else if (transform.position.y > player.transform.position.y + 0.5f && transform.position.y < player.transform.position.y + 1f)
        {
            Debug.Log("Hit high");
            direction.x = -direction.x;
        }

        hasHitPlayer = true;
    }
}
