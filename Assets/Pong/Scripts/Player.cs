using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private float _boundaryValue = 5f;

    private bool up = false;
    private bool down = false;

    [SerializeField]
    private int playerNumber = 0;

    private void Start()
    {
        if (NetworkCore.instance.playerNumber != playerNumber)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        up = Input.GetKey(KeyCode.UpArrow);
        down = Input.GetKey(KeyCode.DownArrow);
    }

    public void UpdatePhysics(float elapsedTime)
    {
        if (up)
        {
            if (transform.position.y < _boundaryValue)
            {
                transform.position += new Vector3(0f, 1, 0f) * speed * elapsedTime;
            }
        }
        else if(down)
        {
            if (transform.position.y > -_boundaryValue)
            {
                transform.position += new Vector3(0f, -1, 0f) * speed * elapsedTime;
            }
        }
    }

    public void SetPosition(float posX, float posY, int inputIndex)
    {
        transform.position = new Vector3(posX, posY, transform.position.z);
    }
}
