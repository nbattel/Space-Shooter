using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    none,                  //The default/no weapon
    simple,                //A simple weapon
    blaster,               //A triple shot blaster
    //heatSeeekingMissile    //Homing Missiles
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

    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();

        //Call SetTpe() for the dafault _type of WeaponType.none
        SetType(_type);

        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;            
        }

        //Find the fireDelegate of the root GameObject
        GameObject rootGO = transform.root.gameObject;
        if(rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(type == WeaponType.simple)
            {
                type = WeaponType.blaster;
            }
            else if(type == WeaponType.blaster)
            {
                type = WeaponType.simple;
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
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch(type)
        {
            case WeaponType.simple:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;

            case WeaponType.blaster:
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

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}





