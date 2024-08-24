using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitPooling : MonoBehaviour
{
    // 인스턴스
    private static EnemyUnitPooling enemyPoolingInstance;

    // 적 유닛 생산
    //private float spawnX1, spawnX2, spawnZ1, spawnZ2;   // 적 소환 영역
    public Transform enemyUnitSpawnPoint;              // 생산 위치
    public LayerMask enemyLayer;

    public GameObject enemyPrefab;                     // 적 유닛 프리팹
    [HideInInspector] public Queue<GameObject> enemyQueue;

    public bool needToCheckDeadUnit=true;                      // stageDoingState에서 새롭게 사망한 적에 대한 점수 갱신을 완료하였는지

    public static EnemyUnitPooling Instance
    {
        get
        {
            if(enemyPoolingInstance==null)
            {
                enemyPoolingInstance = new EnemyUnitPooling();
            }
            return enemyPoolingInstance;
        }
    }

    private void Awake()
    {
        enemyQueue = new Queue<GameObject>();

        if (enemyPoolingInstance == null)
        {
            enemyPoolingInstance = this;

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 적을 생성 후 큐에 보관하는 함수
    public GameObject CreateEnemy()
    {
        GameObject enemyObject = Instantiate(enemyPrefab);
        enemyObject.SetActive(false);
        enemyObject.transform.SetParent(transform);
        enemyQueue.Enqueue(enemyObject);

        return enemyObject;
    }

    // 큐에 저장된 적을 맵에 소환 -> 지정된 장소 안의 랜덤한 위치에 생성
    public GameObject SpawnEnemy()
    {
        if (enemyQueue.Count > 0)
        {
            GameObject enemyObject;
            enemyObject = enemyQueue.Dequeue();
            enemyObject.transform.SetParent(null);
            enemyObject.GetComponent<EnemyHealth>().currentHP = enemyObject.GetComponent<EnemyHealth>().maxHP;
            enemyObject.transform.position = EnemySpawnSite();

            enemyObject.gameObject.SetActive(true);
            //enemyObject.gameObject.GetComponent<EnemyUnitSM>().ChangeState(enemyObject.gameObject.GetComponent<EnemyUnitSM>().idleState);
            Debug.LogFormat("{0}을 큐로부터 소환, 현재 큐에 저장된 적의 수: {1}", enemyObject.name, enemyQueue.Count);
            Debug.LogFormat("{0} 체력: {1}", enemyObject.name, enemyObject.GetComponent<EnemyHealth>().currentHP);

            return enemyObject;
        }
        else
            return null;
    }

    public void PickUpEnemy(GameObject enemy)
    {
        enemyQueue.Enqueue(enemy);
        enemy.SetActive(false);
        enemy.GetComponent<BoxCollider>().enabled = true;
        enemy.transform.SetParent(transform);
        Debug.LogFormat("{0} 회수, 현재 큐에 저장된 적의 수: {1}", enemy.name, enemyQueue.Count);

        EnemyUnitSM sm = enemy.GetComponent<EnemyUnitSM>();
        sm.isAttackMove = false;
        sm.currentState = sm.idleState;

        needToCheckDeadUnit = false;                      // 새롭게 사망한 적에 대한 점수 갱신을 하지 않음 -> 점수 갱신 필요
    }

    // 적 소환 지점을 설정하는 함수
    Vector3 EnemySpawnSite()
    {
        Vector3 spawnSite = enemyUnitSpawnPoint.position;

        float angle = 0f;
        float radius = 1.0f;
        float pi = Mathf.PI;

        while (Physics.Raycast(spawnSite, Vector3.down, Mathf.Infinity, enemyLayer))
        {
            spawnSite += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));

            angle += pi / 3f;
            radius += 0.1f * pi / 3f;
        }

        RaycastHit hit;
        if (Physics.Raycast(spawnSite, Vector3.down, out hit, Mathf.Infinity))
        {
            spawnSite = hit.point;
        }

        return spawnSite;
    }
}


