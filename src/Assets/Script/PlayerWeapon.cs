using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerWeapon : Weapon
{
    public Bullet biuBullet;
    public float biuCD = 0;
    public const int fullClip = 20;
    private int clip = fullClip;
    public int ammunition = 200;
    public const float clipCDTime = 3.0f;
    private float inClipCDTime = 0.0f;
    private bool clipInCD = false;

    private bool shootOption_One = true;

    void Start()
    {
        bulletOffset = new Vector3(0.232f, 1.16f, 1.056f);
        shootCD = 0.6f;
        inShootCDTime = 0.0f;
        damage = 20;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            shootOption_One = !shootOption_One;

        if(Input.GetMouseButton(1))
            Biu();

        biuCD -= Time.deltaTime;

        if (clipInCD)
        {
            inClipCDTime -= Time.deltaTime;
            if (inClipCDTime <= 0)
            {
                clipInCD = false;
                clip = fullClip;
            }
        }
        else
        {
            inShootCDTime -= Time.deltaTime;
        }
    }
    public void ChangeClip()
    {
        if (!clipInCD && ammunition>0)
        {
            inClipCDTime = clipCDTime;
            clipInCD = true;
            if(fullClip > clip && clip >= 0)
            {
                if (ammunition > fullClip - clip)
                    ammunition -= (fullClip - clip);
                else
                {
                    ammunition = 0;
                    clip += ammunition;
                }
            }
        }
    }
    override public void Shoot()
    {
        if (inShootCDTime<0 && !clipInCD)
        {
            if(shootOption_One)
                ShootOnce(transform.rotation, transform.position);
            else
            {
                ShootOnce(transform.rotation, transform.position);
                ShootOnce(transform.rotation, transform.position + transform.forward * 0.2f);
                ShootOnce(transform.rotation, transform.position + transform.forward * 0.4f);
            }
        }
        else
        {
            Debug.Log("武器CD中");
        }
    }
    override protected void ShootOnce(Quaternion shootRot,Vector3 shootPos)
    {
        if(clip>0)
        {
            var bullet = Instantiate(goBullet);
            bullet.shootRotation = shootRot;
            bullet.shootPosition = shootPos;
            bullet.bulletOffset = bulletOffset;
            bullet.damage = damage;
            clip--;
            if (clip > 0)
                inShootCDTime = shootCD;

            audioSource.clip = shootAudio;
            audioSource.Play();
        }
    }

    public int GetClipNum()
    {
        return clip;
    }
    public void SetClipNum(int num)
    {
        clip = num;
    }
    public bool ClipInCD()
    {
        return clipInCD;
    }

    public void Biu()
    {
        if (biuCD <= 0)
        {
            var bullet = Instantiate(biuBullet);
            bullet.shootRotation = transform.rotation;
            bullet.shootPosition = transform.position;
            bullet.bulletOffset = bulletOffset;
            bullet.damage = 0;
            bullet.speed = 40.0f;

            audioSource.clip = biuAudio;
            audioSource.Play();
            biuCD = 5.0f;
        }
        else
            Debug.Log("Biu In CD");
    }
}
