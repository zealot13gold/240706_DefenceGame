using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToGame : MonoBehaviour
{
    public void ClickStartButton()
    {
        SceneManager.LoadScene("Map_Bridge");
    }
}
