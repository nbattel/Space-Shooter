using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
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
}
