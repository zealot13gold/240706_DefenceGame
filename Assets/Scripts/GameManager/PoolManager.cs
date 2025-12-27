using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance=null;

    [Header("오브젝트 풀 목록")]
    [Tooltip("적 유닛 오브젝트 풀")] public EnemyUnitPooling enemyPool;
    [Tooltip("플레이어 유닛 오브젝트 풀")] public PlayerUnitPooling playerPool;
    [Tooltip("장애물 오브젝트 풀")] public BarrierPooling barrierPool;

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
        playerPool = new PlayerUnitPooling();
        enemyPool = new EnemyUnitPooling();
        barrierPool = new BarrierPooling();
    }

    void OnDisable()
    {
        
    }

    public void SetPool()
    {
        // 스테이지 시작 시 실행
        
    }

    public void ClearPool()
    {
        // 로비 이동 시 실행

    }
}
