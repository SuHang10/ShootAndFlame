using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    public Vector3 cameraOffsetFPS = new Vector3(0, 2.25f, 0.2f);
    public Vector3 cameraOffsetTPS = new Vector3(0, 2.0f, -3.0f);
    private bool isTPS = true;

    private float x, y;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            isTPS = !isTPS;

        Vector3 offset;
        if(!isTPS)
        {
            offset = new Vector3(0, 0, 0);
            offset.y = 1.5f;
            //offset = Quaternion.AngleAxis(Player.transform.rotation.eulerAngles.y, Vector3.up) * cameraOffsetFPS;
        }
        else
        {
            offset = -Player.transform.forward * 2.5f;
            offset.y = 2.5f;
            //offset = Quaternion.AngleAxis(Player.transform.rotation.eulerAngles.y, Vector3.up) * cameraOffsetTPS;
        }
        transform.position = Player.transform.position;
        transform.position += offset;

        transform.rotation = Player.transform.rotation;
        /*x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        transform.eulerAngles += new Vector3(-y, x, 0);*/
    }
}
