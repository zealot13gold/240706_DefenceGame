using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 플레이어 유닛 생성(유닛 생성 시 자금 계산)
 * 준비 UI 화면에 표시
 */
public class StagePrepareState : StageState
{
    float spareTime;

    public StagePrepareState(GameObject gameObject) : base(gameObject)
    {
        
    }

    public override void OnStateEnter()            // 행동 시작 시
    {
        // 이전 스테이지의 정보 초기화
        ClearPreviousStageInfo();

        // 준비 UI 출력
        spareTime = GameManager.Instance.prepareTime;
        DisplayPrepareUI();

        // 스테이지 번호 1 추가
        GameManager.Instance.stageNumber++;
        Debug.LogFormat("Stage {0} 준비", GameManager.Instance.stageNumber);

    }

    public override void OnStateUpdate()           // 상태 유지 중
    {
        // 실시간으로 플레이어 유닛 수 체크
        remainPlayerUnit = CheckRemainPlayer();

        // 현재 스테이지에서 생산된 플레이어 유닛 수 체크


        // 시간 카운트 함수 -> 준비 시간이 경과되면 스테이지 시작
        DisplayPrepareUI();
        if (spareTime<=0)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.stageDoing);
        }
    }
    
    public override void OnStateExit()             // 상태 종료
    {
        Debug.LogFormat("준비시간 종료");
        // 준비 UI 삭제
        GameManager.Instance.remainTimeMessage.gameObject.SetActive(false);
        GameManager.Instance.buttons.gameObject.SetActive(false);
        //gm.moneyUI.gameObject.SetActive(false);
    }

    // 준비시간 메시지, 유닛 생성 버튼 활성화
    public void DisplayPrepareUI()
    {
        CountingPrepareTime();
        GameManager.Instance.cashUI.text = GameManager.Instance.cash.ToString();
        GameManager.Instance.remainPlayerMessage.text = remainPlayerUnit.ToString();

        // 준비 UI 출력
        GameManager.Instance.remainTimeMessage.gameObject.SetActive(true);
        GameManager.Instance.buttons.gameObject.SetActive(true);
        //gm.moneyUI.gameObject.SetActive(true);
    }
    void CountingPrepareTime()
    {
        GameManager.Instance.remainTimeMessage.text = "Prepare Time: " + spareTime.ToString();

        spareTime -= Time.deltaTime;
    }

    // 이전 스테이지의 정보 초기화
    void ClearPreviousStageInfo()
    {
        GameManager.Instance.scoreInStage = 0;          // 이전 스테이지에서 획득한 점수 초기화
        GameManager.Instance.cashInStage = 0;           // 이전 스테이지에서 획득한 자금 초기화

        GameManager.Instance.producedPlayerUnitInStage = 0; // 이전 스테이지에서 생산된 플레이어 유닛 수 초기화
        GameManager.Instance.killedPlayerUnitInStage = 0;   // 이전 스테이지에서 사망한 플레이어 유닛 수 초기화
        GameManager.Instance.invadedEnemyUnitInStage = 0;   // 이전 스테이지에서 출현한 적 유닛 수 초기화
        GameManager.Instance.killedEnemyUnitInStage = 0;    // 이전 스테이지에서 사망한 적 유닛 수 초기화
    }

}
