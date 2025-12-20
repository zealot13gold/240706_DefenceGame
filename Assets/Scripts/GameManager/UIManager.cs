using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public event Action<GameManager.gameStateList> stateChanged;

    private void Awake()
    {
        if(instance == null || instance==this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.instance.gameStateChanged += UIRegister;
    }

    private void OnDisable()
    {
        GameManager.instance.gameStateChanged -= UIRegister;
    }

    public void ChangeBGMVolume()
    {

    }

    public void ChangeSFXVolume()
    {

    }

    public void RequestChange(GameManager.gameStateList state)
    {
        // 로비 및 게임의 버튼으로 실행 -> GameManager에 전달
        stateChanged?.Invoke(state);
    }

    void UIRegister(GameManager.gameStateList state)
    {
        // GameManager 씬 전환 시 실행 -> 해당 씬에서 씬 전환과 관련된 UI 등록

    }
}
