using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //public Camera cam;
    private float xValue;
    private float zValue;

    //private void Awake()
    //{
    //    //cam = GetComponent<Camera>();
    //}

    void Update()
    {
        xValue = Input.GetAxis("Horizontal");
        zValue = Input.GetAxis("Vertical");

        if(xValue != 0 || zValue != 0 ) 
        {
            Debug.LogFormat("카메라 이동");
            Camera.main.transform.position += new Vector3(xValue, 0, zValue);
        }
    }
}
