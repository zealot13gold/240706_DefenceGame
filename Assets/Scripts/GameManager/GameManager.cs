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
    //public SceneManager sceneManager;
    //public SoundManager soundManager;
    //public PoolManager poolManager;
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

    private void Start()
    {
        // 게임 프로그램 실행 시 가장 먼저 로비로 이동
        GoToLobby();
    }

    void GoToLobby()
    {
        gameState = gameStateList.gameLobby;
        gameStateChanged?.Invoke(gameState);
    }

    void StartGame()
    {
        gameState = gameStateList.gameStart;
        gameStateChanged?.Invoke(gameState);
    }

    void EndGame()
    {
        gameState = gameStateList.gameEnd;
        gameStateChanged?.Invoke(gameState);
    }
}
