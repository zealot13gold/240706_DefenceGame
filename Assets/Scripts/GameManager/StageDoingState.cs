using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDoingState : StageState
{
    int remainEnemyUnit;
    float remainTime;

    // 스테이지 시작 시 적 생성 간격
        float spawnInterval=0.5f;                   // 적은 spawnInterval 시간마다 생성
        float spawnTime;                         // 적 유닛 1기가 생성되는 사이클 시간
        bool isSpawnEnemy;                   // 적 생성 여부
 
    public StageDoingState(GameObject gameObject):base(gameObject)
    {

    }

    public override void OnStateEnter()            // 행동 시작 시
    {
        Debug.LogFormat("Stage {0} 시작", StageManager.instance.stageNumber);
        remainEnemyUnit=0;

        // 적 유닛 생성 -> 이동
        // 스테이지 시작 시 스테이지 번호에 따라 적 생성
        StageManager.instance.enemyManager.numberOfEnemyUnit = StageManager.instance.enemyManager.NumberOfEnemiesInStage(StageManager.instance.stageNumber);


        //GameManager.Instance.enemyManager.CreateEnemies();
        //remainEnemyUnit = GameManager.Instance.enemyManager.numberOfEnemyUnit;        // 시작할 때는 모든 적이 남아있음

        // 현재 스테이지에서 나타난 적 유닛 수
        StageManager.instance.invadedEnemyUnitInStage = StageManager.instance.enemyManager.numberOfEnemyUnit;         

        // 스테이지 시작 메시지 출력 시간
        remainTime = 0f;

        // 스테이지 업데이트 전에 적 유닛 생성 사이클 시간 초기화를 수행
        spawnTime=0f;
        isSpawnEnemy=false;

        // 스테이지 UI 출력
        DisplayStageUI();
    }

    public override void OnStateUpdate()           // 상태 유지 중
    {
        // 스테이지 시작 메시지 출력 -> 몇 초 후 사라짐
        DisplayStageBeginMessage();

        // 적 유닛 생산
        if(!isSpawnEnemy)   
        {
            SpawnEnemies();
            isSpawnEnemy=true;
        }

        // 플레이어/적 유닛 수 체크 -> 둘 중 하나가 0이 되면 스테이지 결과 상태로 이동
        remainPlayerUnit = CheckRemainPlayer();
        remainEnemyUnit = CheckRemainEnemy();

        // 디스플레이의 점수, 남은 적의 수 갱신
        DisplayStageUI();

        if(remainPlayerUnit <=0 || remainEnemyUnit <=0)
        {
            StageManager.instance.ChangeState(StageManager.instance.stageResult);
        }
    }

    public override void OnStateExit()             // 상태 종료
    {
        // 스테이지 UI 제거
        //gm.scoreUI.gameObject.SetActive(false);
        StageManager.instance.numOfEnemiesMessage.gameObject.SetActive(false);
    }

    // 스테이지 시작 메시지
    public void DisplayStageBeginMessage()
    {
        Debug.LogFormat("스테이지 {0} 시작 메시지 출력", StageManager.instance.stageNumber);
        StageManager.instance.stageTextMessage.text = "Stage " + StageManager.instance.stageNumber + " Start!!!";

        StageManager.instance.stageTextMessage.gameObject.SetActive(true);

        // 준비시간 표시 메시지를 아래와 같이 변경
        StageManager.instance.remainTimeMessage.text = "Enemies are Coming!!!";
        StageManager.instance.remainTimeMessage.color = Color.red;

        if (remainTime < 2f)
        {
            //Debug.LogFormat("남은 시간: {0}", remainTime);
            //Debug.LogFormat("스테이지 {0} 메시지 출력: {1}", gm.stageNumber, gm.stageTextMessage.gameObject.activeSelf);
            remainTime += Time.deltaTime;
        }
        //Debug.LogFormat("스테이지{0} 시작 메시지 삭제", gm.stageNumber);
        else if(remainTime>=2f && remainTime < 5f)
        {
            StageManager.instance.stageTextMessage.gameObject.SetActive(false);
            remainTime += Time.deltaTime;
        }
        else
        {
            //GameManager.Instance.stageTextMessage.gameObject.SetActive(false);
            StageManager.instance.displayRemainTime.gameObject.SetActive(false);         // 스테이지 메시지 숨기기
        }
   }

    // 스테이지 시작 시 UI
    public void DisplayStageUI()
    {
        StageManager.instance.numOfEnemiesMessage.text = remainEnemyUnit.ToString() + " / " + StageManager.instance.enemyManager.numberOfEnemyUnit.ToString() ;
        StageManager.instance.scoreUI.text = "Score: " + StageManager.instance.score.ToString();


        StageManager.instance.remainPlayerMessage.text = remainPlayerUnit.ToString();

        // 준비 UI 출력
        StageManager.instance.numOfEnemiesMessage.gameObject.SetActive(true);
        //gm.moneyUI.gameObject.SetActive(true);
    }

   // 남아있는 적의 수 체크
    int CheckRemainEnemy()
    {
        StageManager.instance.enemyManager.CheckDeadUnit();

        int remainEnemy = StageManager.instance.enemyManager.enemyUnitList.Count;
        //Debug.LogFormat("적 유닛 수: {0}", gm.enemyManager.enemyUnitList.Count);

        return remainEnemy;
    }

    void SpawnEnemies()
    {
        Debug.LogFormat("StageDoing: 적 {0}기 생성 시작", StageManager.instance.enemyManager.numberOfEnemyUnit);
        if(StageManager.instance.enemyManager.numberOfEnemyUnit > remainEnemyUnit)
        {
            if(spawnInterval >= spawnTime)            // 적 생성 시간이 되면 적 생성
            {
                StageManager.instance.enemyManager.CreateEnemies();
                remainEnemyUnit++;                              // 적의 수 1 증가
                spawnTime = 0f;                     // 적 생성 사이클을 0으로 초기화
                Debug.LogFormat("StageDoing: 적 1기 생성");
            }
            else
            {
                Debug.LogFormat("StageDoing: 적 생성 {0}% 완료", spawnTime/spawnInterval*100);
                spawnTime += Time.deltaTime;        // 사이클 경과시간 증가
            }
        }
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
