using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    private bool _inputUp = false;
    private bool _inputDown = false;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            // TO DO : Si la position du player ne dépasse pas la limite du haut
            transform.position += new Vector3(0f, 1, 0f) * speed * Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // TO DO : Si la position du player ne dépasse pas la limite du bas
            transform.position += new Vector3(0f, -1, 0f) * speed * Time.deltaTime;
        }


    }
}
