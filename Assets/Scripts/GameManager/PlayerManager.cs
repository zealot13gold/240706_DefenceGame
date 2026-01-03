using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 역할
 - 플레이어가 소지한 자원, 점수, 유닛 등을 관리

 * 입력값
 - 자원 상황 : Player 클래스에서 실시간으로 전달
 - 선택된 유닛 목록(배열) : 다중선택으로 선택된 유닛을 배열 형태로 저장
 - 메뉴 인터페이스(화면 아래 메뉴)

 * 함수
 - 유닛 생산 : 건물이 선택된 상태(선택된 건물 != null)에서 메뉴 인터페이스에 표시된 유닛을 클릭하여 생산, 자원 유무 체크 필요
 - 건물 건설 : 일꾼 유닛 1기가 선택된 상태에서 메뉴 인터페이스에 표시된 건물을 클릭하여 생산, 자원 유무 체크 필요
 */
public class PlayerManager
{
    // 플레이어 초기 상태
    public int numberOfPlayerUnit;                  // 유닛 수

    // 유닛 선택
    [HideInInspector] public List<GameObject> playerUnitList;                 // 플레이어가 보유한 모든 유닛의 목록
    [HideInInspector] public List<GameObject> deadPlayerUnitList;             // 사망한 플레이어 유닛 목록

    public PlayerManager()
    {
        playerUnitList = new List<GameObject>();
        deadPlayerUnitList = new List<GameObject>();
        numberOfPlayerUnit = 0;
    }

    // 유닛 생산하는 함수: 자원이 충분할 경우 생산 버튼을 클릭하면 새로운 유닛이 생성
    public void CreatePlayerUnit()
    {
        if(PlayerUnitPooling.Instance.playerQueue.Count <=0)
        {
            PlayerUnitPooling.Instance.CreatePlayerUnit();
        }
        playerUnitList.Add(PlayerUnitPooling.Instance.SpawnPlayerUnit());

        Debug.LogFormat("유닛 생산, 총 유닛: {0}", playerUnitList.Count);
        playerUnitList[numberOfPlayerUnit].name = PlayerUnitPooling.Instance.playerUnitNames[0];
    }

    // 사망한 플레이어 유닛은 큐로 되돌아감/사망한 유닛 목록에 포함
    public void CheckDeadUnit()
    {
        if (StageManager.instance.currentState == StageManager.instance.stagePlay)                   // 스테이지 진행 중일때만 체크하도록 함
        {
            foreach (GameObject unit in playerUnitList)
            {
                if (unit.GetComponent<Health>().currentHP <= 0)                        // 비활성화(사망)된 플레이어 유닛 존재
                {
                    //PlayerUnitPooling.Instance.PickUpPlayerUnit(unit);
                    deadPlayerUnitList.Add(unit);           // 해당 유닛을 사망한 유닛 목록에 추가
                    Debug.LogFormat("사망한 플레이어 수: {0}", deadPlayerUnitList.Count);

                    StageManager.instance.killedPlayerUnitInStage++;         // 해당 스테이지에서 사망한 플레이어 유닛 수 1 증가
                }
            }
            // 사망한 유닛은 현재 유닛 목록에서 제거
            foreach (GameObject unit in deadPlayerUnitList)     // 사망한 유닛 목록
            {
                playerUnitList.Remove(unit);                    // 사망한 유닛 목록을 실시간으로 체크하여 플레이어 유닛 목록에 사망한 유닛이 있다면 이를 해당 목록에서 제거
                Debug.LogFormat("유닛 제거, 총 유닛: {0}", playerUnitList.Count);
            }
        }
        
    }

    public void ClearDeadPlayerUnit()
    {
        // 스테이지 결과 상태일 경우 deadPlayerUnitList는 비움
        if (StageManager.instance.currentState == StageManager.instance.stageResult)
        {
            deadPlayerUnitList.Clear();
            Debug.LogFormat("사망한 플레이어 수: {0}", deadPlayerUnitList.Count);
        }
    }

    //void Update()
    //{


    //    // 큐가 비었을 경우 새로운 유닛 인스턴스를 만든 후 소환


    //    // 자원 계산하는 함수: 유닛 생산하는 함수 발동 시 현재 가지고 있는 자원량에서 유닛 생산에 필요한 자원량을 뺌
    //}
}
