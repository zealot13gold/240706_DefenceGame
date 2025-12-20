using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 모든 매니저 총괄
// 게임 시작, 종료 등 상태 변경 -> 다른 매니저가 이를 보고 함수 실행

public class GameManager : MonoBehaviour
{
    // 싱글톤
    public static GameManager instance = null;

    // 각 매니저 등록
    public event Action<gameStateList> gameStateChanged;

    public enum gameStateList 
    {
        gameLobby,          // 게임 로비로 이동
        gameLoading,        // 게임 로딩 중
        gameStart,          // 게임 화면으로 이동
        gameEnd             // 게임 종료
    };
    public gameStateList gameState;

    void Awake()
    {
        if(instance == null || instance == this)
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
        UIManager.instance.stateChanged += GameStateChange;
    }

    private void Start()
    {
        GameStateChange(gameStateList.gameLobby);
    }

    void GameStateChange(gameStateList state)
    {
        if (state != gameState)
        {
            gameState = state;
            gameStateChanged?.Invoke(state);
        }
    }

    //public void GoToLobby()
    //{
    //    gameState = gameStateList.gameLobby;
    //    gameStateChanged?.Invoke(gameState);
    //}

    //public void StartGame()
    //{
    //    // 로비 버튼에서 실행
    //    gameState = gameStateList.gameStart;
    //    gameStateChanged?.Invoke(gameState);
    //}

    //public void EndGame()
    //{
    //    // 로비 버튼에서 실행
    //    gameState = gameStateList.gameEnd;
    //    gameStateChanged?.Invoke(gameState);
    //}
}
