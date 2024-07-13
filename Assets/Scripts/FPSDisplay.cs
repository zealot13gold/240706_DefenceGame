using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    static float FPS;
    public Text FPSDisplayText;

    void Update()
    {
        FPS = 1 / Time.deltaTime;

        FPSDisplayText.text = "   FPS: " + FPS.ToString() + " ms";
    }
}
