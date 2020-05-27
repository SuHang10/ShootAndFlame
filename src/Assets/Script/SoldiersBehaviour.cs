using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldiersBehaviour : MonoBehaviour
{
    public Animator animatior;
    public float speed;
    private bool onLand = true;
    private Rigidbody rigid;
    void Start()
    {
        rigid = transform.GetComponent<Rigidbody>();
        animatior = GetComponent<Animator>();
        speed = 1.0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animatior.SetInteger("state", 2);
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                animatior.SetInteger("state", 1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                animatior.SetInteger("state", 1);
            }
            if (Input.GetKey(KeyCode.W))
            {
                animatior.SetInteger("state", 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                animatior.SetInteger("state", 1);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Land")
            onLand = true;
        Debug.Log(collision.collider.name);
    }
}
