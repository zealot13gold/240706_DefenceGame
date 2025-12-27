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
    public gameStateList gameState/*=gameStateList.gameLobby*/;

    void Awake()
    {
        Debug.LogFormat("instance 싱글톤 실행");
        if (instance != null && instance != this)
        {
            
            Destroy(gameObject);

        }
        else
        {
            Debug.LogFormat("instance 싱글톤 초기화");
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        Debug.LogFormat("GameManager: 로비 씬으로 이동");
        SceneLoader.instance.LobbySceneLoad();
        //GameStateChange(gameStateList.gameLobby);
    }

    public void GameStateChange(gameStateList state)
    {
        // 각 씬의 UI 버튼으로 실행
        if (state != gameState)
        {
            gameState = state;
            gameStateChanged?.Invoke(state);
        }
    }
}
