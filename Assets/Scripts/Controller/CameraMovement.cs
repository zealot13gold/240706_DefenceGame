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
        ControlCameraWithKeyboard();
        ControlCameraWithMouse();
    }

    void ControlCameraWithKeyboard()
    {
        
        xValue = Input.GetAxis("Horizontal");
        zValue = Input.GetAxis("Vertical");

        if (xValue != 0 || zValue != 0)
        {
            //Debug.LogFormat("카메라 이동");
            Camera.main.transform.position += new Vector3(xValue, 0, zValue);
        }
    }

    void ControlCameraWithMouse()
    {
        Vector2 mouseToView = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        //Debug.LogFormat("마우스 위치: {0}", mouseToView);


        if (mouseToView.x >= 0.99f)
        {
            xValue = 1f;
            //Debug.LogFormat("X축으로 {0}, Z축으로 {1} 이동", xValue, zValue);
        }
        else if (mouseToView.x <= 0f)
        {
            xValue = -1f;
            //Debug.LogFormat("X축으로 {0}, Z축으로 {1} 이동", xValue, zValue);
        }
        else
        {
            xValue = 0f;
        }

        if (mouseToView.y >= 0.99f)
        {
            zValue = 1f;
            //Debug.LogFormat("X축으로 {0}, Z축으로 {1} 이동", xValue, zValue);
        }
        else if (mouseToView.y <= 0f)
        {
            zValue = -1f;
            //Debug.LogFormat("X축으로 {0}, Z축으로 {1} 이동", xValue, zValue);
        }
        else
        {
            zValue = 0f;
        }

        if (xValue != 0 || zValue != 0)
        {
            //Debug.LogFormat("카메라 이동");
            Camera.main.transform.position += new Vector3(xValue, 0, zValue);
        }
    }
}
