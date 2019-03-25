using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
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
        health = 10;
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
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero :" + otherGO.name);
                break;

        }
    }
}
