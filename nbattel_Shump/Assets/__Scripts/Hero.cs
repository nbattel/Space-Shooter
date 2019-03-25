using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero S;
    [SerializeField]
    private float _speed = 30.0f;
    private float _projectileSpeed = 60f;
    public int lives = 4;
    public float gameRestartDelay = 2f;
    public float fireRate = 0.10f;
    public float canFire = 0.0f;

    private GameObject lastTriggerGo = null;
    //Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;
    public Weapon[] weapons;


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


    public void Awake()
    {
        if(S == null)
        {
            S = this;
        }
        else
        {
            Debug.Log("Hero.Awake() - Attempted to assign second Hero.S");
        }
        // fireDelegate += Shoot;
    }

    // Update is called once per frame
    public void Update()
    {
        Movement();

        if(Input.GetAxis("Jump") == 1 && fireDelegate != null)
        {
            fireDelegate();
        }
    }

    private void Movement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");    //Getting the horizontal keys (left and right arrow keys and the 'A' and 'D' keys)
        float verticalAxis = Input.GetAxis("Vertical");       //Getting the Vertical Keys  (up and down arrow keys and the 'W' and 'S' keys)

        transform.Translate(Vector3.right * _speed * horizontalAxis * Time.deltaTime);    //Allowing the player to move horizontal across the screen
        transform.Translate(Vector3.up * _speed * verticalAxis * Time.deltaTime);         //Allowing the player to move vertical across the screen
    }

    //private void Shoot()
    //{
    //    if (Time.time > canFire)
    //    {
    //        GameObject projGO = Instantiate<GameObject>(laserPrefab);
    //        projGO.transform.position = transform.position + new Vector3(0, 5f, 0);
    //        Rigidbody2D rigidB = projGO.GetComponent<Rigidbody2D>();
    //        rigidB.velocity = Vector3.up * _projectileSpeed;
    //        canFire = Time.time + fireRate;

    //        Projectile proj = projGO.GetComponent<Projectile>();
    //        proj.type = WeaponType.simple;
    //        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
    //        rigidB.velocity = Vector3.up * tSpeed;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;

        if(go == lastTriggerGo)
        {
            return;
        }

        lastTriggerGo = go;

        if(go.tag == "Enemy")
        {
            Destroy(go);
            Damage();
        }
        else
        {
            print("Triggered by non-enemy: " + go.name);
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
