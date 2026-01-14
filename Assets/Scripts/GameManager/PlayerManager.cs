using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 역할
 - 플레이어 유닛 관리
 */
public class PlayerManager:MonoBehaviour
{
    public static PlayerManager instance=null;

    //public event Action<bool> playerDefeated;

    // 플레이어 초기 상태
    public int numberOfPlayerUnit;                  // 유닛 수

    // 유닛 목록
    //[HideInInspector] public List<GameObject> playerUnitList;                 // 플레이어가 보유한 모든 유닛의 목록
    //[HideInInspector] public List<GameObject> deadPlayerUnitList;             // 사망한 플레이어 유닛 목록
    
    public PlayerManager()
    {
        numberOfPlayerUnit = 0;
    }

    void OnEnable()
    {
        if(instance != null && instance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        //playerUnitList = new List<GameObject>();
        //deadPlayerUnitList = new List<GameObject>();
        numberOfPlayerUnit = 0;
    }

    // 유닛 생산하는 함수: 자원이 충분할 경우 생산 버튼을 클릭하면 새로운 유닛이 생성
    //public void CreatePlayerUnit()
    //{
    //    if(PlayerUnitPooling.Instance.playerQueue.Count <=0)
    //    {
    //        PlayerUnitPooling.Instance.CreatePlayerUnit();
    //    }
    //    playerUnitList.Add(PlayerUnitPooling.Instance.SpawnPlayerUnit());

    //    Debug.LogFormat("유닛 생산, 총 유닛: {0}", playerUnitList.Count);
    //    playerUnitList[numberOfPlayerUnit].name = PlayerUnitPooling.Instance.playerUnitNames[0];
    //}

    public void CheckUnit(bool isDead)
    {
        if (isDead)
        {
            // 유닛 사망
            numberOfPlayerUnit--;
            StageManager.instance.killedPlayerUnitInStage++;
        }
        else
        {
            // 유닛 생산
            numberOfPlayerUnit++;
            StageManager.instance.producedPlayerUnitInStage++;
        }
        StageManager.instance.remainPlayerMessage.text = numberOfPlayerUnit.ToString();

        if (numberOfPlayerUnit == 0) StageManager.instance.PlayerDefeated();
    }

    // 사망한 플레이어 유닛은 큐로 되돌아감/사망한 유닛 목록에 포함
    //public void CheckDeadUnit()
    //{
    //    //if (StageManager.instance.currentState == StageManager.instance.stagePlay)                   // 스테이지 진행 중일때만 체크하도록 함
    //    //{
    //        foreach (GameObject unit in playerUnitList)
    //        {
    //            if (unit.GetComponent<Health>().currentHP <= 0)                        // 비활성화(사망)된 플레이어 유닛 존재
    //            {
    //                //PlayerUnitPooling.Instance.PickUpPlayerUnit(unit);
    //                deadPlayerUnitList.Add(unit);           // 해당 유닛을 사망한 유닛 목록에 추가
    //                Debug.LogFormat("사망한 플레이어 수: {0}", deadPlayerUnitList.Count);

    //                StageManager.instance.killedPlayerUnitInStage++;         // 해당 스테이지에서 사망한 플레이어 유닛 수 1 증가
    //            }
    //        }
    //        // 사망한 유닛은 현재 유닛 목록에서 제거
    //        foreach (GameObject unit in deadPlayerUnitList)     // 사망한 유닛 목록
    //        {
    //            playerUnitList.Remove(unit);                    // 사망한 유닛 목록을 실시간으로 체크하여 플레이어 유닛 목록에 사망한 유닛이 있다면 이를 해당 목록에서 제거
    //            Debug.LogFormat("유닛 제거, 총 유닛: {0}", playerUnitList.Count);
    //        }
    //    //}
        
    //}

    //public void ClearDeadPlayerUnit()
    //{
    //    // 스테이지 결과 상태일 경우 deadPlayerUnitList는 비움
    //    if (StageManager.instance.currentState == StageManager.instance.stageResult)
    //    {
    //        deadPlayerUnitList.Clear();
    //        Debug.LogFormat("사망한 플레이어 수: {0}", deadPlayerUnitList.Count);
    //    }
    //}

    //void Update()
    //{


    //    // 큐가 비었을 경우 새로운 유닛 인스턴스를 만든 후 소환


    //    // 자원 계산하는 함수: 유닛 생산하는 함수 발동 시 현재 가지고 있는 자원량에서 유닛 생산에 필요한 자원량을 뺌
    //}
}
