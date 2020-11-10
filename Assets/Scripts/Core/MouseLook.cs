using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    Transform playerBody;
    public Transform firstPersonCamera;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        //Set variables
        playerBody = GameObject.FindWithTag("Player").transform;        

        //Position the camera from wherever it is
        firstPersonCamera.position = playerBody.position;
        firstPersonCamera.Translate(Vector3.up*1.5f);
        firstPersonCamera.Translate(Vector3.forward*0.5f);        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        //Restricts rotation
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Rotates camera up and down
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //Rotates entire player object left and right
        playerBody.Rotate(Vector3.up, mouseX);
    }
}
