using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager에서 게임 진행 상태(gameState)를 확인하고, 이에 따라 게임 씬 변경
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance=null;

    // 씬 목록
    [Header("씬 목록")]
    [Tooltip("로비 씬 이름")] public string lobbySceneName;
    [Tooltip("게임 씬 이름")] public string gameSceneName;

    private void Awake()
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

    private void OnEnable()
    {
        // 반드시 GameManager 생성 후 실행
        StartCoroutine("ClassInit");
    }

    private void OnDisable()
    {
        GameManager.instance.gameStateChanged -= SceneLoad;
    }

    public void LobbySceneLoad()
    {
        // GameManager.start() 전용
        SceneManager.LoadSceneAsync(lobbySceneName);
    }

    void SceneLoad(GameManager.gameStateList state)
    {
        switch (state)
        {
            case GameManager.gameStateList.gameLobby:
                // 로비로 이동
                Debug.LogFormat("SceneLoader: 로비로 이동");
                SceneManager.LoadSceneAsync(lobbySceneName);
                break;
            case GameManager.gameStateList.gameLoading:
                // 로딩 창
                //Debug.LogFormat("SceneLoader: {0)로 이동", lobbySceneName);

                break;
            case GameManager.gameStateList.gameStart:
                // 게임 화면으로 이동
                Debug.LogFormat("SceneLoader: 게임 씬으로 이동");
                SceneManager.LoadSceneAsync(gameSceneName);
                break;
            case GameManager.gameStateList.gameEnd:
                // 로비에서 게임 종료
                Debug.LogFormat("SceneLoader: 게임 종료");
                Application.Quit();
                break;
        }
    }

    IEnumerator ClassInit()
    {
        while (GameManager.instance == null)
        {
            //Debug.LogFormat("SceneLoader: GameManager 초기화 될때까지 대기");
            yield return null;
        }
        GameManager.instance.gameStateChanged += SceneLoad;
        //Debug.LogFormat("SceneLoader: GameManager에 SceneLoader 연결");
    }
}
