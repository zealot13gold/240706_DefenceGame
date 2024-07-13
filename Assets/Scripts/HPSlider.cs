using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPSlider : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform, Camera.main.transform.rotation*Vector3.up);
    }
}
