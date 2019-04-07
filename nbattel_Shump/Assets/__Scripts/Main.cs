using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    static public Main S;   //A singleton for main
    [Header("Set in Inspector")]
    public GameObject[] _enemyPrefabs;       //Array of enemy prefabs
    public Transform[] _enemies;
    private UIManager _uiManager;

    public int _score = 0;
    static private int _highScore = 0;
    public Text score;
    public Text highscore;
    public Text activeWeapon;
    public Text nukes;
    public Text enemiesDestroyedText;
    public float nukesLeft = 0;

    public int enemiesDestroyed = 0;
    private bool levelOneActivated = false;
    private bool levelTwoActivated = false;
    public bool gameOver = true;

    public GameObject heroPrefab;
    public float enemySpawnPerSecond = 1f;  //Spawn rate of Enemies/Second
    public float enemyDefaultPadding = 1.5f;  //Padding for position

    public WeaponDefinition[] weaponDefinitions;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    private BoundsCheck _bndCheck;

    //Powerups
    public GameObject[] prefabPowerUp;

    public void ShipDestroyed(Enemy e)
    {
        float rand = Random.Range(0f, 10.0f);
        if(rand <= e.powerUpDropChance)
        {
            int ndx = Random.Range(0, prefabPowerUp.Length);
            Instantiate(prefabPowerUp[ndx], e.transform.position, Quaternion.identity);
        }
    }

    public void updateNukeText()
    {
        nukes.text = "Nukes remaining " + nukesLeft;
    }

    private void Awake()
    {
        Time.timeScale = 0.0f;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _bndCheck = GetComponent<BoundsCheck>();           //Set _bndCheck to reference the BoundsCheck component on the GameObject
        S = this;
        Invoke("SpawnEnemy", 3f / enemySpawnPerSecond);    //Invoke SpawnEnemy() once (one enemy appears every 3 seconds)

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
        }

        score.text = "Score: " + _score;
        highscore.text = "Highscore: " + _highScore;
        enemiesDestroyedText.text = "Enemies Destroyed: " + enemiesDestroyed;
    }

    void Update()
    {
        score.text = "Score: " + _score;
        enemiesDestroyedText.text = "Enemies Destroyed: " + enemiesDestroyed;

        if (gameOver == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Instantiate(heroPrefab, Vector3.zero, Quaternion.identity);   //creating the Hero and setting it to a starting posiiton of p:[0,0,0]
                gameOver = false;
                _uiManager.HideTitleScreen();
                Time.timeScale = 1.0f;
            }
        }

        if(enemiesDestroyed >= 15)
        {
            if(levelOneActivated == false)
            {
                _uiManager.ShowLevelOne();
                Time.timeScale = 0.0f;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Enemy.E._speed = 30.0f;
                    _uiManager.HideLevelOne();
                    Time.timeScale = 1.0f;
                    levelOneActivated = true;
                }                
            }            
        }

        if (enemiesDestroyed >= 30)
        {
            if(levelTwoActivated == false)
            {
                _uiManager.ShowLevelTwo();
                Time.timeScale = 0.0f;
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    Enemy.E._speed = 40.0f;
                    _uiManager.HideLevelTwo();
                    Time.timeScale = 1.0f;
                    levelTwoActivated = true;
                }
            }                        
        }
    }

    public void SpawnEnemy()
    {
        //Picking a random enemy prefab to instantiate
        int rand = Random.Range(0, _enemyPrefabs.Length);
        GameObject go = Instantiate<GameObject>(_enemyPrefabs[rand]);

        //Positioning the enemy above the screen with a random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }

        if (rand == 0 || rand == 1)
        {
            //Set the initial position for the spawn enemy
            Vector3 pos = Vector3.zero;
            float xMin = -_bndCheck.camWidth + enemyPadding;
            float xMax = _bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = _bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;
        }
        else
        {
            //Set the initial position for the spawn enemy
            Vector3 pos = Vector3.zero;
            float yMax = 37.0f;
            float yMin = 0.0f;
            pos.y = Random.Range(yMin, yMax);
            pos.x = -_bndCheck.camWidth - enemyPadding;
            go.transform.position = pos;
        }

        if (enemiesDestroyed < 15)
        {
            //Invoke SpawnEnemy() again
            Invoke("SpawnEnemy", 3f / enemySpawnPerSecond);
        }
        else if(enemiesDestroyed >= 15 && enemiesDestroyed < 30)
        {
            //Invoke SpawnEnemy() again
            Invoke("SpawnEnemy", 2f / enemySpawnPerSecond);
        }
        else if (enemiesDestroyed >= 30)
        {
            //Invoke SpawnEnemy() again
            Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        }        
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
        if (_score >= _highScore)
        {
            _highScore = _score;
            highscore.text = "Highscore: " + _highScore;
        }

        SceneManager.LoadScene("Game");
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }

        return (new WeaponDefinition());
    }

}
