using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private float _boundaryValue = 5f;

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.y < _boundaryValue)
            {
                transform.position += new Vector3(0f, 1, 0f) * speed * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.y > -_boundaryValue)
            {
                transform.position += new Vector3(0f, -1, 0f) * speed * Time.deltaTime;
            }
        }
    }
}
