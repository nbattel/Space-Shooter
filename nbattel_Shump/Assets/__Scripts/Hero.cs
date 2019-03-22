using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField]
    private float _speed = 30.0f;
    private float _projectileSpeed = 60f;
    public int lives = 4;
    public float gameRestartDelay = 10f;
    public float fireRate = 0.10f;
    public float canFire = 0.0f;

    public bool shieldsActiveBlue = true;
    public bool shieldsActiveYellow = false;
    public bool shieldsActiveRed = false;
    public GameObject laserPrefab;

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
        Shoot();
    }

    private void Movement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");    //Getting the horizontal keys (left and right arrow keys and the 'A' and 'D' keys)
        float verticalAxis = Input.GetAxis("Vertical");       //Getting the Vertical Keys  (up and down arrow keys and the 'W' and 'S' keys)

        transform.Translate(Vector3.right * _speed * horizontalAxis * Time.deltaTime);    //Allowing the player to move horizontal across the screen
        transform.Translate(Vector3.up * _speed * verticalAxis * Time.deltaTime);         //Allowing the player to move vertical across the screen
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            if(Time.time > canFire)
            {
                GameObject projGO = Instantiate<GameObject>(laserPrefab);
                projGO.transform.position = transform.position + new Vector3(0, 5f, 0);
                //Instantiate(laserPrefab, transform.position + new Vector3(0, 5.0f, 0), Quaternion.identity);
                Rigidbody2D rigidB = projGO.GetComponent<Rigidbody2D>();
                rigidB.velocity = Vector3.up * _projectileSpeed;
                canFire = Time.time + fireRate;
            }
            
        }
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
            lives = 4;
            Main.S.DelayedRestart(gameRestartDelay);
        }
    }
}
