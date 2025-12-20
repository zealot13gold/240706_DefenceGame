using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitPooling : MonoBehaviour
{
    // 인스턴스
    private static PlayerUnitPooling PlayerUnitPoolingInstance;

    // 플레이어 유닛 생산
    public Transform playerUnitSpawnPoint;              // 생산 위치
    public GameObject soliderPrefab;                    // 생산 가능 유닛(전투병)
    [HideInInspector] public Queue<GameObject> playerQueue;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public string[] playerUnitNames;

    public static PlayerUnitPooling Instance
    {
        get
        {
            if (PlayerUnitPoolingInstance == null)
            {
                PlayerUnitPoolingInstance = new PlayerUnitPooling();
            }
            return PlayerUnitPoolingInstance;
        }
    }

    private void Awake()
    {
        playerQueue = new Queue<GameObject>();

        if (PlayerUnitPoolingInstance == null)
        {
            PlayerUnitPoolingInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreatePlayerUnit()
    {
        GameObject playerObject = Instantiate(soliderPrefab);
        playerObject.SetActive(false);
        playerObject.transform.SetParent(transform);
        playerQueue.Enqueue(playerObject);
        Debug.LogFormat("플레이어 유닛 풀링에서 {0} 생성", playerObject.name);

        return playerObject;
    }

    public GameObject SpawnPlayerUnit()
    {
        if (playerQueue.Count > 0)
        {
            GameObject playerObject;
            playerObject = playerQueue.Dequeue();
            playerObject.transform.SetParent(null);
            playerObject.transform.position = PlayerSpawnSite();
            playerObject.GetComponent<PlayerHealth>().currentHP = playerObject.GetComponent<PlayerHealth>().maxHP;
            playerObject.name = playerUnitNames[0];
            playerObject.gameObject.SetActive(true);

            Debug.LogFormat("플레이어 유닛 풀링에서 {0} 소환, 큐 안의 남은 유닛 수: {1}", playerObject.name, playerQueue.Count);

            return playerObject;
        }
        else
            return null;
    }

    public void PickUpPlayerUnit(GameObject player)
    {
        playerQueue.Enqueue(player);
        player.SetActive(false);
        player.GetComponent<BoxCollider>().enabled = true;
        player.transform.SetParent(transform);

        PlayerUnitSM sm = player.GetComponent<PlayerUnitSM>();
        sm.isForceMove = false;
        sm.isAttackMove = false;
        sm.currentState = sm.idleState;
    }

    Vector3 PlayerSpawnSite()
    {
        Vector3 spawnSite = playerUnitSpawnPoint.position;

        float angle = 0f;
        float radius = 0.05f;
        float pi = Mathf.PI;

        RaycastHit hit;
        if(Physics.Raycast(spawnSite, Vector3.down, Mathf.Infinity, groundLayer))
        {

            if(Physics.Raycast(spawnSite, Vector3.down, out hit, Mathf.Infinity, playerLayer))
            {
                spawnSite = hit.point;

                Debug.LogFormat("해당 위치에 플레이어 유닛 존재");
                angle += pi / 3f;
                radius += 0.1f * angle;

                spawnSite += new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
            }
            else
            {
                Debug.LogFormat("해당 위치에 플레이어 유닛 없음");
            }
        }
        Debug.LogFormat("플레이어 소환 위치: {0}", spawnSite);
        return spawnSite;
    }
}
