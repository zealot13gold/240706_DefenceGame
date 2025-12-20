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
        GameManager.instance.gameStateChanged += SceneLoad;
    }

    private void OnDisable()
    {
        GameManager.instance.gameStateChanged -= SceneLoad;
    }

    void SceneLoad(GameManager.gameStateList state)
    {
        switch (state)
        {
            case GameManager.gameStateList.gameLobby:
                // 로비로 이동
                SceneManager.LoadSceneAsync(lobbySceneName);
                break;
            case GameManager.gameStateList.gameLoading:
                // 로딩 창

                break;
            case GameManager.gameStateList.gameStart:
                // 게임 화면으로 이동
                SceneManager.LoadSceneAsync(gameSceneName);
                break;
            case GameManager.gameStateList.gameEnd:
                // 로비에서 게임 종료
                Application.Quit();
                break;
        }
    }
}
