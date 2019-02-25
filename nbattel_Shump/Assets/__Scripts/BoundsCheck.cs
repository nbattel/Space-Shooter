using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{

    [Header("Set in the Unity Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;  //Allows us to choose whether BoundsCheck forces a GameObject to stay on the screen(true) or allows it to exit the screen and notify us(false)

    [Header("These fields are set dynamically")]
    public bool isOnScreen = true;   //Turns false if the GameObject exits the screen
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;   //Used to determine which direction the enemies go off the screen

    private void Awake()
    {
        //Setting the camWidth and camHeight to the width and height of the camera, respectively
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;                     //Set to true until proven false
        offRight = offLeft = offUp = offDown = false;

        //Setting the player boundaries in the horizontalAxis
        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;                //If the GameObject is outside any of the boundaries it is supposed to be in, then this is set false
            offRight = true;
        }
        else if(pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;                //If the GameObject is outside any of the boundaries it is supposed to be in, then this is set false
            offLeft = true;
        }

        //Setting the player boundaries in the verticalAxis
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;                //If the GameObject is outside any of the boundaries it is supposed to be in, then this is set false
            offUp = true;
        }
        else if(pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;                //If the GameObject is outside any of the boundaries it is supposed to be in, then this is set false
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if(keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        //Draw the bounds in the scene pane using OnDrawGizmos()
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
