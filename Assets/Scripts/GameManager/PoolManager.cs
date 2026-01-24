using System.Collections;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance=null;

    [Header("오브젝트 풀 목록")]
    [Tooltip("보병 오브젝트 풀")] public ObjectPool assaultPool;
    [Tooltip("좀비 오브젝트 풀")] public ObjectPool zombiePool;
    [Tooltip("장애물 오브젝트 풀")] public ObjectPool barrierPool;

    [Header("프리팹 목록")]
    [Header("플레이어 유닛")]
    [Tooltip("소총병(Assault) 프리팹")] public GameObject assualtPref;
    [Tooltip("소총병(Assault) 제작 갯수")] public int assaultNum;

    [Header("적 유닛")]
    [Tooltip("좀비 프리팹")] public GameObject zombiePref;
    [Tooltip("좀비 제작 갯수")] public int zombieNum;

    [Header("설치 오브젝트")]
    [Tooltip("장애물 프리팹")] public GameObject barrierPref;
    [Tooltip("장애물 제작 갯수")] public int barrierNum;

    [Header("이펙트")]
    [Tooltip("실탄 이펙트 프리팹")] public GameObject bulletEffectPref;
    [Tooltip("실탄 이펙트 제작 갯수")] public int bulletEffectNum;
    [Tooltip("타격 이펙트 프리팹")] public GameObject hitEffectPref;
    [Tooltip("타격 이펙트 제작 갯수")] public int hitEffectNum;

    // 현재 게임 씬 저장
    GameManager.gameStateList currentScene;

    void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        StartCoroutine("ClassInit");

        // 플레이어 유닛 풀
        assaultPool = new ObjectPool(assualtPref);

        // 적 유닛 풀
        zombiePool = new ObjectPool(zombiePref);

        // 구조물 풀
        barrierPool = new ObjectPool(barrierPref);
    }

    void PoolState(GameManager.gameStateList scene)
    {
        // 씬이 변경되었을 경우
        if (currentScene != scene)
        {
            if (scene == GameManager.gameStateList.gameStart)
            {
                // 게임 씬으로 변경
                SetPool();
            }
            else
            {
                // 게임 씬에서 다른 씬으로 변경되었을 경우
                ClearPool();
            }
            // 씬 상태 저장
            currentScene = scene;
        }
    }

    // 요청 시 해당 위치에 오브젝트 배치
    public void SpawnObject(string name, Vector3 pos, Quaternion rot)
    {
        if (name == "AssaultMan") assaultPool.SetObject(pos, rot);
        else if (name == "Zombie") zombiePool.SetObject(pos, rot);
        else if (name == "Barrier") barrierPool.SetObject(pos, rot);

    }

    void SetPool()
    {
        // 모든 풀에서 오브젝트 생성
        for (int i = 0; i < assaultNum; i++) assaultPool.CreateObject();
        for (int i = 0; i < zombieNum; i++) zombiePool.CreateObject();
        for (int i = 0; i < barrierNum; i++) barrierPool.CreateObject();
    }

    void ClearPool()
    {
        // 모든 풀에서 오브젝트 회수
        assaultPool.ReturnAll();
        zombiePool.ReturnAll();
        barrierPool.ReturnAll();
    }

    IEnumerator ClassInit()
    {
        while (GameManager.instance == null)
        {
            //Debug.LogFormat("SceneLoader: GameManager 초기화 될때까지 대기");
            yield return null;
        }
        GameManager.instance.gameStateChanged += PoolState;
        //Debug.LogFormat("SceneLoader: GameManager에 SceneLoader 연결");
    }
}
