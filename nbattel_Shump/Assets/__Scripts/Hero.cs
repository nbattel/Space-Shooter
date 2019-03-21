using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30.0f;
    public int lives = 4;
    public bool shieldsActiveBlue = true;
    public bool shieldsActiveYellow = false;
    public bool shieldsActiveRed = false;
    [SerializeField]
    private GameObject _shieldBlue;
    [SerializeField]
    private GameObject _shieldYellow;
    [SerializeField]
    private GameObject _shieldRed;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    public void Damage()
    {
        if(shieldsActiveBlue == true)
        {
            shieldsActiveBlue = false;
            _shieldBlue.SetActive(false);
            shieldsActiveYellow = true;
            _shieldYellow.SetActive(true);
        }
        else if(shieldsActiveYellow == true)
        {
            shieldsActiveYellow = false;
            _shieldYellow.SetActive(false);
            shieldsActiveRed = true;
            _shieldRed.SetActive(true);
        }
        else if(shieldsActiveRed == true)
        {
            shieldsActiveRed = false;
            _shieldRed.SetActive(false);
        }
        lives--;

        if(lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
