using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    static public Enemy E;
    public float _speed = 20.0f;
    public bool boolEnemy1 = true;
    public bool boolEnemy2 = true;
    public float _fireRate = 0.3f;
    private float health = 1;    

    public float powerUpDropChance = 4.0f;
    public bool notifiedOfDestruction = false;

    protected BoundsCheck _bndCheck;   //Private variable allows this Enemy script to store a reference to the BoundsCheck script component attached to the same GameObject
    [SerializeField]
    private GameObject _enemy0ExplosionPrefab;
    [SerializeField]
    protected GameObject _enemy1ExplosionPrefab;
    [SerializeField]
    protected AudioClip _audioClip;


    private void Awake()
    {
        if (E == null)
        {
            E = this;
        }
        else
        {
            Debug.Log("Enemy.Awake() - Attempted to assign second Enemy.E");
        }
        _bndCheck = GetComponent<BoundsCheck>();    //Searches the BoundCheck script component attached to this same GameObject
        boolEnemy1 = (Random.value > 0.5f);
        boolEnemy2 = (Random.value > 0.5f);        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        Move();

        if (_bndCheck != null && _bndCheck.offDown)         //Checks to see whether the GameObject has gone off the screen because it has a pos.y that is too negative
        {
            Destroy(gameObject);
        }
        else if (_bndCheck != null && _bndCheck.offRight)         //Checks to see whether the GameObject has gone off the screen because it has a pos.x that is too positive
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D coll)
    {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag)
        {
            case "Projectile_Hero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //If this enemy is off the screen dont damage it
                if (!_bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                //Hurt the enemy 
                //Get the damage amount from the WEAP_DICT
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0)
                {
                    //Destroy the enemy
                    if(!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Main.S._score += 100;
                    Destroy(this.gameObject);
                    Instantiate(_enemy0ExplosionPrefab, transform.position, Quaternion.identity);
                    AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
                    Main.S.enemiesDestroyed++;                    
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero :" + otherGO.name);
                break;

        }
    }


    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    public virtual void Move()  //Moving the Enemy down the screen
    {
        Vector3 tempPos = pos;
        tempPos.y -= _speed * Time.deltaTime;
        pos = tempPos;
    }
}
