using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30.0f;

    // Update is called once per frame
    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");    //Getting the horizontal keys (left and right arrow keys and the 'A' and 'D' keys)
        float verticalAxis = Input.GetAxis("Vertical");       //Getting the Vertical Keys  (up and down arrow keys and the 'W' and 'S' keys)

        transform.Translate(Vector3.right * _speed * horizontalAxis * Time.deltaTime);    //Allowing the player to move horizontal across the screen
        transform.Translate(Vector3.up * _speed * verticalAxis * Time.deltaTime);         //Allowing the player to move vertical across the screen
    }
}
