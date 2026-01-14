using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*
 * 플레이어 유닛 생성(유닛 생성 시 자금 계산)
 * 준비 UI 화면에 표시
 */
public class StagePrepareState: IState
{
    float spareTime;


    public StagePrepareState(GameObject gameObject)
    {
        spareTime = 0;
    }

    public void Enter()            // 행동 시작 시
    {
        // 스테이지 준비
        StageManager.instance.GameStagePrepare();

        // 이전 스테이지의 정보 초기화
        ClearPreviousStageInfo();

        // 준비 UI 출력
        spareTime = StageManager.instance.prepareTime;
        DisplayPrepareUI();

        // 스테이지 번호 1 추가
        StageManager.instance.stageNumber++;
        StageManager.instance.stageNumTitle.text = "Stage " + StageManager.instance.stageNumber.ToString();
        Debug.LogFormat("Stage {0} 준비", StageManager.instance.stageNumber);

        // 준비 시간 표시
        StageManager.instance.remainTimeMessage.color = Color.white;
        
    }

    //public override void OnStateUpdate()           // 상태 유지 중
    //{
    //    //    // 실시간으로 플레이어 유닛 수 체크
    //    //remainPlayerUnit = CheckRemainPlayer();

    //    //    // 시간 카운트 함수 -> 준비 시간이 경과되면 스테이지 시작
    //    //    DisplayPrepareUI();
    //    //    if (spareTime<=0)
    //    //    {
    //    //        StageManager.instance.ChangeState(StageManager.instance.stagePlay);
    //    //    }
    //}

    public void Exit()             // 상태 종료
    {
        Debug.LogFormat("준비시간 종료");
        // 준비 UI 삭제
        StageManager.instance.buttons.gameObject.SetActive(false);
    }

    void ClearPreviousStageInfo()
    // 이전 스테이지의 정보 초기화
    {
        StageManager.instance.scoreInStage = 0;          // 이전 스테이지에서 획득한 점수 초기화
        StageManager.instance.cashInStage = 0;           // 이전 스테이지에서 획득한 자금 초기화

        StageManager.instance.producedPlayerUnitInStage = 0; // 이전 스테이지에서 생산된 플레이어 유닛 수 초기화
        StageManager.instance.killedPlayerUnitInStage = 0;   // 이전 스테이지에서 사망한 플레이어 유닛 수 초기화
        StageManager.instance.invadedEnemyUnitInStage = 0;   // 이전 스테이지에서 출현한 적 유닛 수 초기화
        StageManager.instance.killedEnemyUnitInStage = 0;    // 이전 스테이지에서 사망한 적 유닛 수 초기화
    }

    // 준비시간 메시지, 유닛 생성 버튼 활성화
    public void DisplayPrepareUI()
    {
        StageManager.instance.cashUI.text = "Cash: " + StageManager.instance.cash.ToString();
        //StageManager.instance.remainPlayerMessage.text = remainPlayerUnit.ToString();

        // 준비 UI 출력
        StageManager.instance.displayRemainTime.gameObject.SetActive(true);
        StageManager.instance.buttons.gameObject.SetActive(true);
    }

    //public void DisplayPrepareTime(int spareTime)
    //{
    //    StageManager.instance.remainTimeMessage.text = "Prepare Time: " + (spareTime).ToString();
    //}




}
