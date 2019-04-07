using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public float speed = 15.0f;
    private float _health = 2;
    public float rotationsPerMinute = 10.0f;

    private BoundsCheck _bndCheck;   //Private variable allows this Enemy script to store a reference to the BoundsCheck script component attached to the same GameObject

    // Start is called before the first frame update
    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();    //Searches the BoundCheck script component attached to this same GameObject
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        transform.Rotate(0, 0, 6.0f * rotationsPerMinute * Time.deltaTime);

        if (_bndCheck != null && _bndCheck.offDown)         //Checks to see whether the GameObject has gone off the screen because it has a pos.y that is too negative
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "Projectile_Hero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //If the Asteroid is off the screen dont damage it
                if (!_bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                //Hurt the Asteroid 
                //Get the damage amount from the WEAP_DICT
                _health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (_health <= 0)
                {
                    //Destroy the Asteroid
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Asteroid hit by non-ProjectileHero :" + otherGO.name);
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

    public void Move()  //Moving the Asteroid down the screen
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
}
