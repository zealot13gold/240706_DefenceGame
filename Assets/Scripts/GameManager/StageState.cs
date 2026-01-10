using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageState : BaseState
{
    //protected GameManager gm;
    protected int remainPlayerUnit;
    protected StageState(GameObject gameManager)
    {

    }


    public override void OnStateEnter()            // 행동 시작 시
    {

    }

    public override void OnStateUpdate()           // 상태 유지 중
    {
        
    }

    public override void OnStateExit()             // 상태 종료
    {

    }

    // 남아있는 플레이어 수 체크
    //protected int CheckRemainPlayer()
    //{
        //StageManager.instance.playerManager.CheckDeadUnit();

        //int remainPlayer = StageManager.instance.playerManager.playerUnitList.Count;
        //Debug.LogFormat("플레이어 유닛 수: {0}", remainPlayer);

        //return remainPlayer;
    //}
}
