﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set in inspector")]
    public GameObject poi;
    public GameObject[] panels;
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;
    private float _panelHt;
    private float _depth;

    // Start is called before the first frame update
    void Start()
    {
        _panelHt = panels[0].transform.localScale.y;
        _depth = panels[0].transform.position.z;

        //Set the initial position of panels
        panels[0].transform.position = new Vector3(0, 0, _depth);
        panels[1].transform.position = new Vector3(0, _panelHt, _depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tY, tX = 0;
        tY = Time.time * scrollSpeed % _panelHt + (_panelHt * 0.5f);

        if(poi != null)
        {
            tX = -poi.transform.position.x * motionMult;
        }

        //Position panels[0]
        panels[0].transform.position = new Vector3(tX, tY, _depth);

        //The position panels[1] where needed to make a continuos starfield
        if(tY >= 0)
        {
            panels[1].transform.position = new Vector3(tX, tY - _panelHt, _depth);
        }
        else
        {
            panels[1].transform.position = new Vector3(tX, tY + _panelHt, _depth);
        }
    }
}
