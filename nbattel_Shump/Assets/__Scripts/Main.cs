using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;   //A singleton for main
    [Header("Set in Inspector")]
    [SerializeField]
    private GameObject[] _enemyPrefabs;       //Array of enemy prefabs

    [SerializeField]
    private GameObject _heroPrefab;
    public float enemySpawnPerSecond = 1f;  //Spawn rate of Enemies/Second
    public float enemyDefaultPadding = 1.5f;  //Padding for position

    public WeaponDefinition[] weaponDefinitions;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    private BoundsCheck _bndCheck;

    private void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();           //Set _bndCheck to reference the BoundsCheck component on the GameObject
        S = this;
        Instantiate(_heroPrefab, Vector3.zero, Quaternion.identity);   //creating the Hero and setting it to a starting posiiton of p:[0,0,0]
        Invoke("SpawnEnemy", 3f / enemySpawnPerSecond);    //Invoke SpawnEnemy() once (one enemy appears every 3 seconds)

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach(WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;
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

        //Invoke SpawnEnemy() again
        Invoke("SpawnEnemy", 3f / enemySpawnPerSecond);
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }

    public void Restart()
    {
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
