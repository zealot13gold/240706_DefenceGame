using UnityEngine;
using UnityEngine.AI;

public class EnemyIdleState : EnemyUnitState
{

    public EnemyIdleState(GameObject unit) : base(unit) { }

    public override void OnStateEnter()
    {
        //navMesh.isStopped = true;               // dest의 초기값은 {0, 0, 0}이므로, 해당 좌표로 이동하지 못하도록 함
        

        sm.enemyAudioSource.clip = sm.enemySearchingVoice;
        sm.enemyAudioSource.Play();
    }

    public override void OnStateUpdate()
    {
        if (health.currentHP <= 0)                      // 적의 체력이 0인지 확인
        {
            sm.ChangeState(sm.deadState);          
        }
        else
        {
            sm.FindEnemy();                           // isAttackMove 값을 확인함으로써 적이 존재하는지 확인

            if (sm.targetEnemy != null)                 // 강제 이동을 하지 않는 상태에서 적이 시야 내에 존재할 경우
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
        base.OnStateExit();
    }
}