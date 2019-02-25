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
}
