using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public Player player1;

    [SerializeField]
    public Player player2;

    [SerializeField]
    public Ball ball;

    public float nextGameTick = 0f;
    public float gameTickInterval = 1.0f/30f;
    public float now = 0f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

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
