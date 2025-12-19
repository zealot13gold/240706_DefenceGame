using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTimer : MonoBehaviour
{
    public static SceneTransitionTimer Instance;

    private float startTime;
    bool isLoading = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
           Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void LoadGameScene()
    {
        if (isLoading) return;
        isLoading = true;

        startTime = Time.realtimeSinceStartup; // 실제 시간
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Map_Bridge")
        {
            float elapsed = Time.realtimeSinceStartup - startTime;
            Debug.LogFormat("{0} Scene 전환 시간: {0}초", scene.name, elapsed);
        }
    }
}
