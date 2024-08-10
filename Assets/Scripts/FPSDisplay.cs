using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    static float FPS;
    public Text FPSDisplayText;

    //void Update()
    //{

    //    float time = 0;

    //    while (time < 1)
    //    {
    //        time += Time.deltaTime;
    //    }

    //    FPS = 1 / time;

    //    FPSDisplayText.text = FPS.ToString() + " FPS";

    //}

    private void Start()
    {
        StartCoroutine(FPSCalculator());
    }

    IEnumerator FPSCalculator()
    {

        //float time = 0;

        while (true)
        {
            FPS = 0;
            for (float time=0; time<=1.0f; time+=Time.deltaTime)
            {
                FPS++;

                Debug.LogFormat("Time: {0}, FPS: {1}", time, FPS);

                yield return null;
            }
            //time += Time.deltaTime;
            FPSDisplayText.text = FPS.ToString() + " fps";
        }  
    }
}
