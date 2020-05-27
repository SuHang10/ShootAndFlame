using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Bullet goBullet;
    public Vector3 bulletOffset = new Vector3(0, 0, 0);

    public AudioSource audioSource;
    public AudioClip shootAudio, biuAudio;

    public int damage = 10;
    public float shootCD = 0.6f;
    protected float inShootCDTime = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        inShootCDTime -= Time.deltaTime;
    }

    virtual public void Shoot()
    {
        ShootOnce(transform.rotation,transform.position);
    }

    virtual protected void ShootOnce(Quaternion shootRot, Vector3 shootPos)
    {
        var bullet = Instantiate(goBullet);
        bullet.shootRotation = shootRot;
        bullet.shootPosition = shootPos;
        bullet.bulletOffset = bulletOffset;
        bullet.damage = damage;
        inShootCDTime = shootCD;

        audioSource.clip = shootAudio;
        audioSource.Play();
    }

    public bool ShootInCD()
    {
        return inShootCDTime > 0 ? true : false;
    }
}
