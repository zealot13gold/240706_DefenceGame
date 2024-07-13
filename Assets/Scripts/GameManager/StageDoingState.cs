using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDoingState : StageState
{

    int remainEnemyUnit;

    float remainTime;

    public StageDoingState(GameObject gameObject):base(gameObject)
    {

    }

    public override void OnStateEnter()            // 행동 시작 시
    {
        Debug.LogFormat("Stage {0} 시작", GameManager.Instance.stageNumber);


        // 적 유닛 생성 -> 이동
        // 스테이지 시작 시 스테이지 번호에 따라 적 생성
        GameManager.Instance.enemyManager.numberOfEnemyUnit = GameManager.Instance.enemyManager.NumberOfEnemiesInStage(GameManager.Instance.stageNumber);
        //Debug.LogFormat("Stage {0}에서 등장하는 적의 수: {1}", gm.stageNumber, gm.enemyManager.numberOfEnemyUnit);
        GameManager.Instance.enemyManager.CreateEnemies();
        remainEnemyUnit = GameManager.Instance.enemyManager.numberOfEnemyUnit;        // 시작할 때는 모든 적이 남아있음

        // 현재 스테이지에서 나타난 적 유닛 수
        GameManager.Instance.invadedEnemyUnitInStage = GameManager.Instance.enemyManager.numberOfEnemyUnit;         

        // 스테이지 시작 메시지 출력 시간
        remainTime = 0f;

        // 스테이지 UI 출력
        DisplayStageUI();
    }

    public override void OnStateUpdate()           // 상태 유지 중
    {
        // 스테이지 시작 메시지 출력 -> 몇 초 후 사라짐
        DisplayStageBeginMessage();

        // 플레이어/적 유닛 수 체크 -> 둘 중 하나가 0이 되면 스테이지 결과 상태로 이동
        remainPlayerUnit = CheckRemainPlayer();
        remainEnemyUnit = CheckRemainEnemy();

        // 점수 계산
        //CalculateScore();

        // 디스플레이의 점수, 남은 적의 수 갱신
        DisplayStageUI();

        if(remainPlayerUnit <=0 || remainEnemyUnit <=0)
        {
            GameManager.Instance.ChangeState(GameManager.Instance.stageResult);
        }
    }

    public override void OnStateExit()             // 상태 종료
    {
        // 스테이지 UI 제거
        //gm.scoreUI.gameObject.SetActive(false);
        GameManager.Instance.numOfEnemiesMessage.gameObject.SetActive(false);
    }

    // 스테이지 시작 메시지
    public void DisplayStageBeginMessage()
    {
        //Debug.LogFormat("스테이지 {0} 시작 메시지 출력", gm.stageNumber);
        GameManager.Instance.stageTextMessage.text = "Stage " + GameManager.Instance.stageNumber + " Start!!!";

        GameManager.Instance.stageTextMessage.gameObject.SetActive(true);


        if (remainTime < 2f)
        {
            //Debug.LogFormat("남은 시간: {0}", remainTime);
            //Debug.LogFormat("스테이지 {0} 메시지 출력: {1}", gm.stageNumber, gm.stageTextMessage.gameObject.activeSelf);
            remainTime += Time.deltaTime;
        }
        //Debug.LogFormat("스테이지{0} 시작 메시지 삭제", gm.stageNumber);
        else
        {
            GameManager.Instance.stageTextMessage.gameObject.SetActive(false);
        }
    }

    // 스테이지 시작 시 UI
    public void DisplayStageUI()
    {
        GameManager.Instance.numOfEnemiesMessage.text = remainEnemyUnit.ToString() + " / " + GameManager.Instance.enemyManager.numberOfEnemyUnit.ToString() ;
        GameManager.Instance.scoreUI.text = GameManager.Instance.score.ToString();


        GameManager.Instance.remainPlayerMessage.text = remainPlayerUnit.ToString();

        // 준비 UI 출력
        GameManager.Instance.numOfEnemiesMessage.gameObject.SetActive(true);
        //gm.moneyUI.gameObject.SetActive(true);
    }

   

    // 남아있는 적의 수 체크
    int CheckRemainEnemy()
    {
        GameManager.Instance.enemyManager.CheckDeadUnit();

        int remainEnemy = GameManager.Instance.enemyManager.enemyUnitList.Count;
        //Debug.LogFormat("적 유닛 수: {0}", gm.enemyManager.enemyUnitList.Count);

        return remainEnemy;
    }

    //void CalculateScore()
    //{
    //    if (!EnemyUnitPooling.Instance.needToCheckDeadUnit)                                       // 새롭게 사망한 적이 발생하였고, 이에 대한 점수 체크를 하지 않음 -> 점수 갱신이 필요
    //    {
    //        //Debug.LogFormat("체크- 점수: {0}, 죽은 유닛 수: {1}", gm.score, gm.enemyManager.deadEnemyUnitList.Count);
    //        GameManager.Instance.score += GameManager.Instance.enemyManager.deadEnemyUnitList.Count * 100;
    //        GameManager.Instance.scoreInStage += GameManager.Instance.enemyManager.deadEnemyUnitList.Count * 100;

    //        EnemyUnitPooling.Instance.needToCheckDeadUnit = true;                                 // 새롭게 사망한 적에 대한 점수 갱신을 완료하였음
    //    }
    //}
}
