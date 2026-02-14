using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageResultState : IState
{
    float remainTime;

    public StageResultState(GameObject gameObject)
    {

    }
    public void Enter()            // 행동 시작 시
    {
        StageManager.instance.GameStageResult();

        Debug.LogFormat("Stage 종료, 결과 출력");
        // 자금 계산
        //CalculateMoney();

        // 사망한 플레이어 유닛 목록 초기화
        //StageManager.instance.playerManager.ClearDeadPlayerUnit();
    }

    //public override void OnStateUpdate()           // 상태 유지 중
    //{
    //    // 게임 종료 메시지 출력
    //    DisplayStageEndMessage();

    //    if (StageManager.instance.stageTextMessage.gameObject.activeSelf==false)                       // 결과 메시지 출력 후
    //    {
    //        // 이번 스테이지에서 얻은 점수, 자금 표시 -> 클릭 시 메시지 제거
    //        DisplayResultBoard();

    //        if (StageManager.instance.resultBoard.gameObject.activeSelf == false)
    //        {
    //            if (StageManager.instance.playerManager.playerUnitList.Count <= 0)
    //            {
    //                // 플레이어 유닛 수가 0이라면 OnStageExit() 실행 
    //                GameManager.instance.GameStateChange(GameManager.gameStateList.gameLobby);
    //            }
    //            else
    //            {
    //                // 플레이어 유닛 수가 0이 아니라면 스테이지 번호에 1 추가 후 스테이지 준비 상태로 돌입
    //                StageManager.instance.enemyManager.EmptyEnemyUnitList();
    //                Debug.LogFormat("적 유닛 리스트 비우기, 적 유닛 수: {0}", StageManager.instance.enemyManager.enemyUnitList.Count);
    //                Debug.LogFormat("사망한 적 유닛 리스트 비우기, 적 유닛 수: {0}", StageManager.instance.enemyManager.deadEnemyUnitList.Count);
    //                StageManager.instance.ChangeState(StageManager.instance.stagePrepare);
    //            }
    //        }
    //    }
    //}
    public void Update()
    {

    }
    public void Exit()             // 게임 종료
    {


    }

    // 스테이지 종료 후 사살한 적의 수만큼 자금 제공
    //void CalculateMoney()
    //{
    //    StageManager.instance.cash += StageManager.instance.enemyManager.deadEnemyUnitList.Count * 100;
    //    StageManager.instance.cashInStage += StageManager.instance.enemyManager.deadEnemyUnitList.Count * 100;
    //}

    // 결과창
    
}
