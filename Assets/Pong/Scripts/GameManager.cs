using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Player player1;

    [SerializeField]
    private Player player2;

    [SerializeField]
    private Ball ball;

    private float nextGameTick = 0f;
    private float gameTickInterval = 1.0f/30f;
    private float now = 0f;

    private void Update()
    {
        now += Time.deltaTime;

        if (now >= nextGameTick)
        {
            GameTick(gameTickInterval);

            nextGameTick += gameTickInterval;
        }
    }

    private void GameTick(float elapsedTime)
    {
        player1.UpdatePhysics(elapsedTime);
        player2.UpdatePhysics(elapsedTime);
        ball.UpdatePhysics(player1, player2, elapsedTime);
    }
}
