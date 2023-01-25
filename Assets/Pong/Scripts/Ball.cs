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

    [SerializeField] private Player player1;
    [SerializeField] private Player player2;

    void Start()
    {
        //direction = new Vector2(Random.Range(-1f,1f), Random.Range(-1f, 1f));
        direction = new Vector2(-1f, 0.2f);
    }

    void Update()
    {
        if(HitBoundaries(transform.position.y))
        {
            direction.y = -direction.y;
        }

        transform.position += direction * speed * Time.deltaTime;

        HitPlayer(player1, true);
        HitPlayer(player2, false);
    }

    private bool HitBoundaries(float posY)
    {
        return transform.position.y >= boundaryValue || transform.position.y <= -boundaryValue;
    }

    private void HitPlayer(Player player, bool isPlayer1)
    {
        if (!(transform.position.x - player.transform.position.x < 0.25f) && isPlayer1)
            return;
        if (!(transform.position.x - player.transform.position.x > -0.25f) && !isPlayer1)
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
    }
}
