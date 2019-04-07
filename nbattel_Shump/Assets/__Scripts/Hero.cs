using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{

    static public Hero S;
    public float speed = 30.0f;
    private float _projectileSpeed = 60f;
    public int lives = 4;
    public float gameRestartDelay = 2f;
    public float fireRate = 0.10f;
    public float canFire = 0.0f;

    private GameObject _lastTriggerGo = null;
    //Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;
    public Weapon[] weapons;

    private UIManager _uiManager;

    public bool shieldsActiveBlue = true;
    public bool shieldsActiveYellow = false;
    public bool shieldsActiveRed = false;
    public bool isSpeedBoostActive = false;
    public GameObject laserPrefab;
    [SerializeField]
    private GameObject _enemy0ExplosionPrefab;
    [SerializeField]
    private GameObject _enemy1ExplosionPrefab;
    [SerializeField]
    private GameObject _HeroExplosionPrefab;
    [SerializeField]
    private AudioClip _audioClip;

    public GameObject shieldBlue;
    public GameObject shieldYellow;
    public GameObject shieldRed;
    public GameObject leftEngineFailure;
    public GameObject rightEngineFailure;


    public void Awake()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.Log("Hero.Awake() - Attempted to assign second Hero.S");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager != null)
        {
            _uiManager.UpdateLives(lives);
        }
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

        if(isSpeedBoostActive == true)
        {
            transform.Translate(Vector3.right * speed * 2.0f * horizontalAxis * Time.deltaTime);    //Allowing the player to move horizontal across the screen
            transform.Translate(Vector3.up * speed * 2.0f * verticalAxis * Time.deltaTime);         //Allowing the player to move vertical across the screen

        }
        else
        {
            transform.Translate(Vector3.right * speed * horizontalAxis * Time.deltaTime);    //Allowing the player to move horizontal across the screen
            transform.Translate(Vector3.up * speed * verticalAxis * Time.deltaTime);         //Allowing the player to move vertical across the screen
        }
    }

    //Finding the closest enemy by tag
    public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        GameObject otherGO = other.gameObject;

        if (go == _lastTriggerGo)
        {
            return;
        }

        _lastTriggerGo = go;

        if (go.tag == "Enemy")
        {
            Destroy(go);
            Instantiate(_enemy1ExplosionPrefab, go.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
            Damage();
        }
        else if (go.tag == "Asteroid")
        {
            Destroy(go);
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
            Damage();
        }
        else
        {
            print("Triggered by non-enemy: " + go.tag);
        }
    }

    public void Damage()
    {
        if(shieldsActiveBlue == true)
        {
            shieldsActiveBlue = false;
            shieldBlue.SetActive(false);
            shieldsActiveYellow = true;
            shieldYellow.SetActive(true);
        }
        else if(shieldsActiveYellow == true)
        {
            shieldsActiveYellow = false;
            shieldYellow.SetActive(false);
            shieldsActiveRed = true;
            shieldRed.SetActive(true);
        }
        else if(shieldsActiveRed == true)
        {
            shieldsActiveRed = false;
            shieldRed.SetActive(false);
            leftEngineFailure.SetActive(true);
            rightEngineFailure.SetActive(true);
        }
        lives--;
        _uiManager.UpdateLives(lives);

        if(lives < 1)
        {
            Destroy(this.gameObject);
            Instantiate(_HeroExplosionPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
            lives = 4;
            Main.S.DelayedRestart(gameRestartDelay);
        }
    }

    public void SpeedBoostPowerUpOn()
    {
        isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    public IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        isSpeedBoostActive = false;
    }
}
