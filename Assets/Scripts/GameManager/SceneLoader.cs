using UnityEngine;

// GameManager에서 게임 진행 상태(gameState)를 확인하고, 이에 따라 게임 씬 변경
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance=null;
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
}
