using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageResultState : StageState
{
    float remainTime;

    public StageResultState(GameObject gameObject) : base(gameObject)
    {

    }
    public override void OnStateEnter()            // 행동 시작 시
    {
        Debug.LogFormat("Stage 종료, 결과 출력");
        // 자금 계산
        CalculateMoney();

        // 사망한 플레이어 유닛 목록 초기화
        GameManager.instance.playerManager.ClearDeadPlayerUnit();

        // 스테이지 종료 메시지 출력 시간
        remainTime = 0f;
    }

    public override void OnStateUpdate()           // 상태 유지 중
    {
        // 게임 종료 메시지 출력
        DisplayStageEndMessage();

        if (GameManager.instance.stageTextMessage.gameObject.activeSelf==false)                       // 결과 메시지 출력 후
        {
            // 이번 스테이지에서 얻은 점수, 자금 표시 -> 클릭 시 메시지 제거
            DisplayResultBoard();

            if (GameManager.instance.resultBoard.gameObject.activeSelf == false)
            {
                if (GameManager.instance.playerManager.playerUnitList.Count <= 0)
                {
                    // 플레이어 유닛 수가 0이라면 OnStageExit() 실행 
                    StageManager.LoadScene("MainLobby");
                }
                else
                {
                    // 플레이어 유닛 수가 0이 아니라면 스테이지 번호에 1 추가 후 스테이지 준비 상태로 돌입
                    GameManager.instance.enemyManager.EmptyEnemyUnitList();
                    Debug.LogFormat("적 유닛 리스트 비우기, 적 유닛 수: {0}", GameManager.instance.enemyManager.enemyUnitList.Count);
                    Debug.LogFormat("사망한 적 유닛 리스트 비우기, 적 유닛 수: {0}", GameManager.instance.enemyManager.deadEnemyUnitList.Count);
                    GameManager.instance.ChangeState(GameManager.instance.stagePrepare);
                }
            }
        }
    }

    public override void OnStateExit()             // 게임 종료
    {


    }

    // 스테이지 종료 후 사살한 적의 수만큼 자금 제공
    void CalculateMoney()
    {
        GameManager.instance.cash += GameManager.instance.enemyManager.deadEnemyUnitList.Count * 100;
        GameManager.instance.cashInStage += GameManager.instance.enemyManager.deadEnemyUnitList.Count * 100;
    }

    

    public void DisplayStageEndMessage()
    {
        //Debug.LogFormat("스테이지 {0} 종료 메시지 출력", gm.stageNumber);

        if (GameManager.instance.enemyManager.enemyUnitList.Count <= 0)
        {
            GameManager.instance.stageTextMessage.text = "Stage " + GameManager.instance.stageNumber + " Clear!!";
        }
        else
        {
            GameManager.instance.stageTextMessage.text = "Stage " + GameManager.instance.stageNumber + " Defeated";
        }

        GameManager.instance.stageTextMessage.gameObject.SetActive(true);


        if (remainTime < 2f)
        {
            //Debug.LogFormat("남은 시간: {0}", remainTime);
            //Debug.LogFormat("스테이지 {0} 메시지 출력: {1}", gm.stageNumber, gm.stageTextMessage.gameObject.activeSelf);
            remainTime += Time.deltaTime;
        }
        //Debug.LogFormat("스테이지{0} 시작 메시지 삭제", gm.stageNumber);
        else
        {
            GameManager.instance.stageTextMessage.gameObject.SetActive(false);
        }
    }

    // 결과창
    void DisplayResultBoard()
    {
        GameManager.instance.resultBoard.gameObject.SetActive(true);

        GameManager.instance.gameResultTextInBoard.text = GameManager.instance.stageTextMessage.text;
        GameManager.instance.obtainCashInStageInBoard.text = GameManager.instance.cashInStage.ToString();
        GameManager.instance.obtainScoreInStageInBoard.text = GameManager.instance.scoreInStage.ToString();

        GameManager.instance.producedPlayerUnitsInStageInBoard.text =  GameManager.instance.producedPlayerUnitInStage.ToString();
        GameManager.instance.killedPlayerToEnemyInStageInBoard.text = GameManager.instance.killedPlayerUnitInStage.ToString();
        GameManager.instance.invadedEnemyUnitsInStageInBoard.text = GameManager.instance.invadedEnemyUnitInStage.ToString();
        GameManager.instance.killedEnemyToPlayerInStageInBoard.text = GameManager.instance.killedEnemyUnitInStage.ToString();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.instance.resultBoard.gameObject.SetActive(false);
        }
    }
}
