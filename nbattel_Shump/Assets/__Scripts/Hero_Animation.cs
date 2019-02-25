using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Animation : MonoBehaviour
{
    private Animator _animation;

    // Start is called before the first frame update
    void Start()
    {
        _animation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))  //If 'A' key is pressed down or left arrow key is pressed down
        {
            _animation.SetBool("Turn_Left", true);
            _animation.SetBool("Turn_Right", false);
        }
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))  //If 'A' key is up or left arrow key is up
        {
            _animation.SetBool("Turn_Left", false);
            _animation.SetBool("Turn_Right", false);
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))  //If 'D' key is pressed down or right arrow key is pressed down
        {
            _animation.SetBool("Turn_Left", false);
            _animation.SetBool("Turn_Right", true);
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))  //If 'D' key is up or right arrow key is up
        {
            _animation.SetBool("Turn_Left", false);
            _animation.SetBool("Turn_Right", false);
        }
    }
}
