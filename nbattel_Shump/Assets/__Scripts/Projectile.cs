using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    private BoundsCheck _bndCheck;
    // Start is called before the first frame update
    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_bndCheck.offUp)
        {
            Destroy(this.gameObject);
        }
    }
}
