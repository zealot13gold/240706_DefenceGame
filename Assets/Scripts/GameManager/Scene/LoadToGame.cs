using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadToGame : MonoBehaviour
{
    bool isClick = false;

    public void ClickStartButton()
    {
        if (isClick) return;
        isClick = true;

        //SceneManager.LoadScene("Map_Bridge");
        GameManager.instance.GameStateChange(GameManager.gameStateList.gameStart);
        //SceneTransitionTimer.instance.LoadGameScene();
    }
}
