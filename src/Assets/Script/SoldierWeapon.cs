using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierWeapon : Weapon
{
    void Start()
    {
        bulletOffset = new Vector3(0.125f, 1.0f, 0.66f);
        shootCD = 5.0f;
        inShootCDTime = 0.0f;
    }

    void Update()
    {
        inShootCDTime -= Time.deltaTime;
        /*if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }*/
    }
}
