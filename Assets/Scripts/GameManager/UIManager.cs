//using System;
//using UnityEngine;
//using UnityEngine.UI;

//public class UIManager : MonoBehaviour
//{
//    public static UIManager instance = null;

//    private void Awake()
//    {
//        if (instance == null || instance == this)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    private void OnEnable()
//    {
//        GameManager.instance.gameStateChanged += UIChange;
//    }

//    private void OnDisable()
//    {
//        GameManager.instance.gameStateChanged -= UIChange;
//    }



//    //public void RequestChange(GameManager.gameStateList state)
//    //{
//    //    // 로비 및 게임의 버튼으로 실행 -> GameManager에 전달
//    //    stateChanged?.Invoke(state);
//    //}

//    void UIChange(GameManager.gameStateList state)
//    {
//        switch(state)
//        {
//            case GameManager.gameStateList.gameLobby:

//                break;
//            case GameManager.gameStateList.gameLoading:

//                break;
//            case GameManager.gameStateList.gameStart:

//                break;
//            case GameManager.gameStateList.gameEnd:

//                break;
//        }
//    }
//}
