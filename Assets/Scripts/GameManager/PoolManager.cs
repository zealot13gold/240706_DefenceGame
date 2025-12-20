using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance=null;

    // 오브젝트 풀 목록
    public EnemyUnitPooling enemyPool;
    public PlayerUnitPooling playerPool;
    public BarrierPooling barrierPool;

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

    public void OnEnable()
    {
        playerPool = new PlayerUnitPooling();
        enemyPool = new EnemyUnitPooling();
        barrierPool = new BarrierPooling();
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
