using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20.0f;
    public Quaternion shootRotation;
    public Vector3 shootPosition;
    public Vector3 bulletOffset = new Vector3(0.29f, 1.45f, 1.32f);
    private Vector3 forceVec;
    public int damage = 20;
    void Start()
    {
        float eulerAngles_y = shootRotation.eulerAngles.y;
        forceVec = Quaternion.AngleAxis(eulerAngles_y, Vector3.up) * Vector3.up;
        Vector3 offset = Quaternion.AngleAxis(eulerAngles_y, Vector3.up) * bulletOffset;

        transform.position = shootPosition + offset;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, eulerAngles_y, 0);
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    private void Clear()
    {
        if(transform.position.z>40 || transform.position.z<-40||
            transform.position.x > 40 || transform.position.x < -40)
            Destroy(gameObject);
    }
}
