using UnityEngine;
using UnityEngine.AI;

/*
 * OnStateEnter()
 - 

 * OnStateUpdate()
 - 주변에 적이 있는지 확인
 - 
 */

public class PlayerIdleState:PlayerUnitState
{

    public PlayerIdleState(GameObject unit) : base(unit) {}

    public override void OnStateEnter()
    {
        //navMesh.isStopped = true;               // dest의 초기값은 {0, 0, 0}이므로, 해당 좌표로 이동하지 못하도록 함
        Debug.LogFormat("{0}은 현재 idle 상태", unit.name);
    }

    public override void OnStateUpdate()
    {
        if (health.currentHP <= 0)
        {
            sm.ChangeState(sm.deadState);
        }
        else
        {
            sm.FindEnemy();                           // isAttackMove 값을 확인함으로써 적이 존재하는지 확인

            if (sm.isForceMove)                      // 오른쪽 클릭으로 인해 isForceMove 값이 true로 변경되었을 경우
            {
                sm.ChangeState(sm.moveState);
            }
            else if (/*sm.targetEnemy.activeSelf &&*/ sm.targetEnemy != null)                 // 강제 이동을 하지 않는 상태에서 적이 시야 내에 존재할 경우
            {
                if (sm.isAttackMove)
                {
                    sm.ChangeState(sm.moveToAttackState);
                }
                else
                {
                    sm.ChangeState(sm.attackState);
                }
            }
        }
    }
    public override void OnStateExit()
    {

    }
}