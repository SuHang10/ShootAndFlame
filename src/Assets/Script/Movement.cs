using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public float moveSpeed = 1.0f;
    public float rotSpeed = 20.0f;
    private bool onLand = true;
    private Transform transform;
    public void Initialized(Transform _transform,float _moveSpeed)
    {
        transform = _transform;
        moveSpeed = _moveSpeed;
    }
    public Vector3 Forward()
    {
        return transform.forward * moveSpeed;
    }
    public Vector3 Left()
    {
        return -transform.right * moveSpeed;
    }
    public Vector3 Right()
    {
        return transform.right * moveSpeed;
    }
    public Vector3 Back()
    {
        return -transform.forward * moveSpeed;
    }
    public Vector3 LeftForward()
    {
        return (transform.forward - transform.right).normalized * moveSpeed;
    }
    public Vector3 RighrForward()
    {
        return (transform.forward + transform.right).normalized * moveSpeed;
    }
    public Vector3 LeftBack()
    {
        return (-transform.forward - transform.right).normalized * moveSpeed;
    }
    public Vector3 RightBack()
    {
        return (-transform.forward + transform.right).normalized * moveSpeed;
    }
}
