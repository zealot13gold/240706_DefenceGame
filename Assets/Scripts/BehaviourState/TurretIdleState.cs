using UnityEngine;
using UnityEngine.AI;

public class TurretIdleState:TurretState
{

    public TurretIdleState(GameObject unit) : base(unit) {}

    public override void OnStateEnter()
    {
        Debug.LogFormat("{0}은 현재 idle 상태", unit.name);
    }

    public override void OnStateUpdate()
    {
        Debug.LogFormat("{0} 체력: {1}", unit, health.currentHP);
        if (health.currentHP <= 0)
        {
            sm.ChangeState(sm.deadState);
        }
        else
        {
            sm.FindEnemy();                           // isAttackMove 값을 확인함으로써 적이 존재하는지 확인

            if ( sm.targetEnemy != null)                 // 강제 이동을 하지 않는 상태에서 적이 시야 내에 존재할 경우
            {
               sm.ChangeState(sm.attackState);
            }
        }
    }
    public override void OnStateExit()
    {

    }
}