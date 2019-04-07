using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    private float health = 3;

    public override void Move()
    {
        if (gameObject.transform.position.x > 24f || gameObject.transform.position.x < -24f)
        {   
            boolEnemy1 = !boolEnemy1;
        }

        Vector3 tempPos = pos;
        if(boolEnemy1)
        {
            tempPos.x -= _speed * Time.deltaTime;
        }
        else
        {
            tempPos.x += _speed * Time.deltaTime;
        }
        tempPos.y -= _speed * Time.deltaTime;
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
                    Main.S._score += 150;
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
