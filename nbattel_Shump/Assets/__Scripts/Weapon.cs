using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponType
{
    none,                   //The default/no weapon
    simple,                 //A simple weapon
    simpleEnemy,            //Enemy Wepon
    blaster,                //A triple shot blaster
    homingMissile,          //Homing Missiles
    nuke                    //Destroys all the enemies on the screen

}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public Color color = Color.white;              //Color of collar & power-up
    public GameObject projectilePrefab;            //Prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0f;                  //Amount of damage cause
    public float delayBetweenShots = 0f;
    public float velocity = 20;                    //Speed of projectiles
}

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")] [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer _collarRend;
    public float movementSpeed = 35.0f;
    private AudioSource _audioSource;
    private GameObject[] _enemiesOnScreen;

    static public float nukes = 0;

    // Start is called before the first frame update
    void Start()
    {

        _audioSource = GetComponent<AudioSource>();
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();

        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;            
        }

        //Call SetTpe() for the dafault _type of WeaponType.none
        SetType(_type);

        //Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
        else if(rootGO.GetComponent<Enemy_2>() != null)
        {
            rootGO.GetComponent<Enemy_2>().fireDelegate += Fire;
        }

        if (transform.parent.gameObject.tag == "Enemy")
        {
            type = WeaponType.simpleEnemy;
        }
    }

    void Update()
    {
        if (transform.parent.gameObject.tag == "Hero")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (type == WeaponType.simple)
                {
                    type = WeaponType.blaster;
                    Main.S.activeWeapon.text = "Active Weapon: Blaster";
                }
                else if (type == WeaponType.blaster)
                {
                    type = WeaponType.homingMissile;
                    Main.S.activeWeapon.text = "Active Weapon: Heat Seeking Missiles";
                }
                else if (type == WeaponType.homingMissile)
                {
                    type = WeaponType.nuke;
                    Main.S.activeWeapon.text = "Active Weapon: Nuke";
                }
                else if (type == WeaponType.nuke)
                {
                    type = WeaponType.simple;
                    Main.S.activeWeapon.text = "Active Weapon: Simple";
                }
            }
        }        
    }

    public WeaponType type
    {
        get
        {
            return(_type);
        }
        set
        {
            SetType(value);
        }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }

        def = Main.GetWeaponDefinition(_type);
        _collarRend.material.color = def.projectileColor;
        lastShotTime = 0;
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Time.time - lastShotTime < def.delayBetweenShots) return;

        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        Vector3 velEnemy = Vector3.down * def.velocity;

        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        if (transform.parent.gameObject.tag == "Hero")
        {
            switch (type)
            {
                case WeaponType.simple:
                    _audioSource.Play();
                    p = MakeProjectile();
                    p.rigid.velocity = vel;
                    break;

                case WeaponType.simpleEnemy:
                    _audioSource.Play();
                    p = MakeProjectile();
                    p.rigid.velocity = vel;
                    break;

                case WeaponType.blaster:
                    _audioSource.Play();
                    p = MakeProjectile();                 //Make middle projectile
                    p.rigid.velocity = vel;
                    //Make right projectile
                    p = MakeProjectile();
                    p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                    p.rigid.velocity = p.transform.rotation * vel;
                    //Make left projectile
                    p = MakeProjectile();
                    p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                    p.rigid.velocity = p.transform.rotation * vel;
                    break;

                case WeaponType.homingMissile:
                    _audioSource.Play();
                    p = MakeProjectile();
                    p.rigid.velocity = vel;
                    break;

                case WeaponType.nuke:
                    if (nukes == 0)
                    {
                        break;
                    }
                    else
                    {
                        _audioSource.Play();
                        int i = 0;
                        p = MakeProjectile();                 //Make middle projectile
                        p.rigid.velocity = vel;
                        while (i < 360)
                        {
                            p = MakeProjectile();
                            p.transform.rotation = Quaternion.AngleAxis(1 + i, Vector3.back);
                            p.rigid.velocity = p.transform.rotation * vel;
                            i++;
                        }
                        nukes--;
                        Main.S.nukesLeft--;
                        Main.S.updateNukeText();
                        DestroyEnemiesOnScreen();
                        break;
                    }
            }
        
                
        }
        else if(transform.parent.gameObject.tag == "Enemy")
        {
            switch (type)
            {
                case WeaponType.simpleEnemy:                    
                    p = MakeProjectile();
                    p.rigid.velocity = velEnemy;
                    break;
            }
        }
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "Projectile_Hero";
            go.layer = LayerMask.NameToLayer("Projectile_Hero");           
        }
        else if(transform.parent.gameObject.tag == "Enemy")
        {
            go.tag = "Projectile_Enemy";
            go.layer = LayerMask.NameToLayer("Projectile_Enemy");           
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);

    }

    public void DestroyEnemiesOnScreen()
    {
        _enemiesOnScreen = GameObject.FindGameObjectsWithTag("Enemy");
        print(_enemiesOnScreen.Length.ToString());
        for (int i = 0; i < _enemiesOnScreen.Length; i++)
        {
            Destroy(_enemiesOnScreen[i]);
            Main.S.enemiesDestroyed++;
        }
        Main.S._score += 1000;
    }
}





