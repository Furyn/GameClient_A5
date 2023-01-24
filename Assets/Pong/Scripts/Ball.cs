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

    void Start()
    {
        direction = Vector2.up;
    }

    void Update()
    {
        if(HitBoundaries(transform.position.y))
        {
            direction.y = -direction.y;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private bool HitBoundaries(float posY)
    {
        return transform.position.y >= boundaryValue || transform.position.y <= -boundaryValue;
    }

    private bool HitPlayer(float posX)
    {
        return false;
    }
}
