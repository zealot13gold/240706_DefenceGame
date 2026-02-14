using UnityEngine;
using UnityEngine.AI;

/*
 * OnStateEnter()
 - 

 * OnStateUpdate()
 - 주변에 적이 있는지 확인
 - 
 */

public class PlayerIdleState:IState
{
    PlayerUnitSM sm;
    PlayerHealth health;
    GameObject unit;
    public PlayerIdleState(GameObject unit) 
    {
        sm = unit.GetComponent<PlayerUnitSM>();
        health = unit.GetComponent<PlayerHealth>();
        this.unit = unit;
    }

    public void Enter()
    {
        //navMesh.isStopped = true;               // dest의 초기값은 {0, 0, 0}이므로, 해당 좌표로 이동하지 못하도록 함

        sm.dest = unit.transform.position;
        

    }

    public void Update()
    {
        if (health.currentHP <= 0)
        {
            sm.UnitStateChange(sm.deadState);
        }
        else
        {
            sm.FindEnemy();                           // isAttackMove 값을 확인함으로써 적이 존재하는지 확인
            //sm.ForceMove();

            if (sm.isForceMove)                      // 오른쪽 클릭으로 인해 isForceMove 값이 true로 변경되었을 경우
            {
                sm.UnitStateChange(sm.moveState);
            }
            else if (/*sm.targetEnemy.activeSelf &&*/ sm.targetEnemy != null)                 // 강제 이동을 하지 않는 상태에서 적이 시야 내에 존재할 경우
            {
                if (sm.isAttackMove)
                {
                    sm.UnitStateChange(sm.moveToAttackState);
                }
                else
                {
                    sm.UnitStateChange(sm.attackState);
                }
            }
        }
    }
    public void Exit()
    {

    }
}