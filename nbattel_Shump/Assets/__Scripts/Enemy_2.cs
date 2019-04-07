using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    private float health = 10;
    //Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Create a WeaponFireDelegate field named fireDelegate
    public WeaponFireDelegate fireDelegate;
    public Weapon[] weapons;

    public override void Update()
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

        if (Main.S.enemiesDestroyed >= 30)
        {
           fireDelegate();
        }
    }

    public override void Move()
    {
        if (gameObject.transform.position.y <= 0.0f || gameObject.transform.position.y >= 37.0f)
        {
            boolEnemy2 = !boolEnemy2;
        }

        Vector3 tempPos = pos;
        if (boolEnemy2)
        {
            tempPos.y -= (_speed / 2) * Time.deltaTime;
        }
        else
        {
            tempPos.y += (_speed / 2) * Time.deltaTime;
        }
        tempPos.x += (_speed / 2) * Time.deltaTime;
        pos = tempPos;
    }

    public override void OnTriggerEnter2D(Collider2D coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
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
                if (health <= 0)
                {
                    //Destroy the enemy
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Main.S._score += 300;
                    Destroy(this.gameObject);
                    Instantiate(_enemy1ExplosionPrefab, transform.position, Quaternion.identity);
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
}
