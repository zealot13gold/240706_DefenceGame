using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTimer : MonoBehaviour
{
    public static SceneTransitionTimer instance;

    private float startTime;
    bool isLoading = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //GameManager.instance.gameStateChanged+=OnSceneLoaded;
        }
        else
        {
           Destroy(gameObject);
        }
    }
    //private void OnEnable()
    //{
    //    // 반드시 GameManager 생성 후 실행
    //    //StartCoroutine("ClassInit");
    //}

    //private void OnDisable()
    //{
    //    GameManager.instance.gameStateChanged -= SceneLoad;
    //}

    //void OnDestroy()
    //{
    //    if (Instance == this)
    //    {
    //        SceneManager.sceneLoaded -= OnSceneLoaded;
    //    }
    //}

    public void LoadGameScene()
    {
        if (isLoading) return;
        isLoading = true;

        startTime = Time.realtimeSinceStartup; // 실제 시간
    }

    //void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == "Map_Bridge")
    //    {
    //        float elapsed = Time.realtimeSinceStartup - startTime;
    //        Debug.LogFormat("{0} Scene 전환 시간: {0}초", scene.name, elapsed);
    //    }
    //}

    void OnSceneLoaded(GameManager.gameStateList state)
    {
        float elapsed = Time.realtimeSinceStartup - startTime;
        //Debug.LogFormat("{0} Scene 전환 시간: {0}초", scene.name, elapsed);

    }
}
