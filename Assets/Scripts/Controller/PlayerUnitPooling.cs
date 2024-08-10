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

            DontDestroyOnLoad(gameObject);
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

        return playerObject;
    }

    // 큐에 저장된 적을 맵에 소환
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

            return playerObject;
        }
        else
            return null;
    }

    public void PickUpPlayerUnit(GameObject player)
    {
        playerQueue.Enqueue(player);
        player.SetActive(false);
        player.transform.SetParent(transform);
        //player.GetComponent<PlayerUnitSM>().isForceMove = false;
        //player.GetComponent<PlayerUnitSM>().isAttackMove = false;
        //player.GetComponent<PlayerUnitSM>().isFire = false;
    }

    // 적 소환 지점을 설정하는 함수
    Vector3 PlayerSpawnSite()
    {
        Vector3 spawnSite = playerUnitSpawnPoint.position;

        float angle = 0f;
        float radius = 0.05f;
        float pi = Mathf.PI;

       while (Physics.Raycast(spawnSite, Vector3.down, Mathf.Infinity, playerLayer))
        {
            spawnSite += new Vector3(radius*Mathf.Cos(angle), 0f, radius*Mathf.Sin(angle));

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
