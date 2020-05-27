using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name == "Character"||
            collision.collider.name == "Leg1")
        {
            Destroy(gameObject);
        }
    }
}
