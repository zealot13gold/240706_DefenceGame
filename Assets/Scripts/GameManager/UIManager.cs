//using System;
//using UnityEngine;

//public class UIManager : MonoBehaviour
//{
//    public static UIManager instance = null;
//    public event Action<GameManager.gameStateList> stateChanged;

//    private void Awake()
//    {
//        if(instance == null || instance==this)
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

//    public void ChangeBGMVolume()
//    {

//    }

//    public void ChangeSFXVolume()
//    {

//    }

//    //public void RequestChange(GameManager.gameStateList state)
//    //{
//    //    // 로비 및 게임의 버튼으로 실행 -> GameManager에 전달
//    //    stateChanged?.Invoke(state);
//    //}

//    void UIChange(GameManager.gameStateList gameStateList)
//    {

//    }
//}
