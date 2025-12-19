using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager poolInstance;

    // 오브젝트 풀 목록
    public EnemyUnitPooling enemyPool;
    public PoolUnitPooling playerPool;
    public BaarrierPooling barrierPool;

    void Awake()
    {
        if(poolInstance == null)
        {
            //poolInstance = new PoolManager();
            poolInstance = this;
            DontDestoryOnLoading(gameObject);
        }
        else
        {
            Destory(gameObject);
        }
    }
}
