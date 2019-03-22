using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum WeaponType
{
    none,
    simple,
    blaster
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;      
    public string letter;                          //Letter to show on the power-up
    public Color color = Color.white;              //Color of collar & power-up
    public GameObject laserprefab;                 //Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;                  //Amount of damage cause
    public float continuousDamage = 0;             //Damage per second (laser)
    public float delayBetweenShots = 0;
    public float velocity = 20;                    //Speed of projectiles
}