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

    EnemyHealth targetEnemyHealth;

    public void Enter()
    {
        //navMesh.isStopped = true;               // dest의 초기값은 {0, 0, 0}이므로, 해당 좌표로 이동하지 못하도록 함

        sm.dest = unit.transform.position;
        

    }

    public void Update()
    {
        if (health.currentHP <= 0)
        {
            Debug.LogFormat("PlayerIdleState: {0} 사망", unit.name);
            sm.UnitStateChange(sm.deadState);
        }
        else
        {
            //FindEnemy();                           // isAttackMove 값을 확인함으로써 적이 존재하는지 확인
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

    public GameObject FindEnemy()                                                                         // 적이 존재하는지 확인(idleState 상태에서 지속적으로 확인), isAttack 값을 변경
    {
        sm.enemies = Physics.OverlapSphere(unit.transform.position, sm.viewRange, sm.enemyLayerMask);             // 유닛의 현재 위치에서 시야(viewPoint) 내에 적이 존재하는지 확인

        Vector3 bufferEnemyPos;                                                                          // 적의 위치를 임시 저장
        float bufferEnemyDist = 10000f;                                                                  // 플레이어 유닛과 적 사이의 거리를 임시 저장
        GameObject targetEnemy = null;

        for (int i = 0; i < sm.enemies.Length; i++)
        {
            bufferEnemyPos = sm.enemies[i].transform.position;                                              // 공격 대상의 위치를 임시저장

            if (bufferEnemyDist > Mathf.Abs((bufferEnemyPos - unit.transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
            {
                targetEnemy = sm.enemies[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장
               float  enemyHealth = sm.targetEnemy.GetComponent<EnemyHealth>().currentHP;
                if (enemyHealth <= 0) continue;

                sm.targetEnemy = sm.enemies[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장

                //targetEnemyIsDead = targetEnemy.GetComponent<Health>().isDead;
                bufferEnemyDist = Mathf.Abs((sm.targetEnemy.transform.position - unit.transform.position).magnitude);    // 가까운 적과의 거리를 저장
            }
        }
        return targetEnemy;
    }

    
}