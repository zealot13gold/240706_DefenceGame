using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public int numberOfEnemyUnit;                  // 생산 가능 유닛 수

    // 적 유닛 목록
    public List<GameObject> enemyUnitList;    // 생산된 적 리스트
    public List<GameObject> deadEnemyUnitList;  // 현재 스테이지에서 사망한 적 리스트
    public int deadUnitNumber=0;                  // 모든 스테이지에서 사망한 적 유닛 수 총합

    public EnemyManager()
    {
        enemyUnitList = new List<GameObject>();
        deadEnemyUnitList = new List<GameObject>();
    }

    public void CreateEnemies()
    {
        Debug.LogFormat("적 유닛 {0}기 생성", numberOfEnemyUnit);
        // 적 생성
        for (int i = 0; i < numberOfEnemyUnit; i++)
        {
            Debug.LogFormat("큐 안에 저장된 적의 수: {0}", EnemyUnitPooling.Instance.enemyQueue.Count);
            if (EnemyUnitPooling.Instance.enemyQueue.Count <= 0)
            {
                
                EnemyUnitPooling.Instance.CreateEnemy();            // 적을 생성 후 큐에 저장
            }
            enemyUnitList.Add(EnemyUnitPooling.Instance.SpawnEnemy());        // 큐에 저장된 적을 맵에 소환
            enemyUnitList[i].name = "enemy " + (i + 1);
            Debug.LogFormat("적 유닛-{0} 생성", enemyUnitList[i].name);
        }
    }

    // 사망한 적 유닛은 유닛 리스트에서 제거
    public void CheckDeadUnit()
    {
        foreach(GameObject unit in enemyUnitList) 
        {
            if (!unit.activeSelf)                                   // 비활성화(사망)된 적 유닛 수 존재 시
            {
                //EnemyUnitPooling.Instance.PickUpEnemy(unit);
                deadEnemyUnitList.Add(unit);                        // 사망한 적 유닛 목록에 추가

                GameManager.Instance.killedEnemyUnitInStage++;      // 해당 스테이지에서 사망한 적 유닛 수 1 증가

                // 적 유닛이 사망할 때마다 점수 100씩 증가
                GameManager.Instance.score += 100;
                GameManager.Instance.scoreInStage += 100;
            }
        }

        foreach(GameObject unit in deadEnemyUnitList)
        {
            enemyUnitList.Remove(unit);                             // 사망한 적 유닛 목록을 실시간으로 체크하여 적 유닛 목록에 사망한 유닛이 있다면 이를 해당 목록에서 제거 
        }
        //Debug.LogFormat("정리 후 적 유닛 수: {0}", enemyUnitList.Count);
    }

    // 스테이지 종료 시 적 유닛 리스트 비우기
    public void EmptyEnemyUnitList()
    {
        enemyUnitList.Clear();
        deadEnemyUnitList.Clear();

        //Debug.LogFormat
    }

    // 스테이지에 따라 등장하는 적의 수 변경
    public int NumberOfEnemiesInStage(int stageNumber)
    {
        //Debug.LogFormat("현재 스테이지 번호: {0}", stageNumber);
        int enemies;

        // 스테이지에 따라 적의 수 변경
        enemies = stageNumber * 10;

        return enemies;
    }
}
