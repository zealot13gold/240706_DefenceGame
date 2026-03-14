using System.Collections;
using UnityEngine;

public class EnemyAttackState : IState
{
    EnemyUnitSM sm;
    EnemyHealth health;
    GameObject unit;

    public EnemyAttackState(GameObject unit)
    {
        sm = unit.GetComponent<EnemyUnitSM>();
        health = unit.GetComponent<EnemyHealth>();
        this.unit = unit;
    }
    PlayerHealth playerHealth;
    public void Enter()
    {
        sm.unitState = EnemyUnitSM.unitStateList.attack;
        //navMesh.isStopped = true;
        //Debug.LogFormat("{0}은 현재 attack 상태, 시야범위: {1}, 공격범위: {2}", unit.name, sm.viewRange, sm.attackRange);

        //sm.enemyAudioSource.clip = sm.enemyAttackVoice;
        Debug.LogFormat("EnemyAttackState: {0}의 타겟은 {1}", unit.name, sm.targetPlayer.gameObject.name);
        playerHealth = sm.targetPlayer.GetComponent<PlayerHealth>();

    }

    public void Update()
    {
        if (health.currentHP <= 0)                      // 사망하였을 경우
        {
            sm.UnitStateChange(sm.deadState);
        }
        else
        {
            if (/*sm.unitState == EnemyUnitSM.unitStateList.attack &&*/ /*!sm.targetPlayer.activeSelf*/playerHealth.currentHP <= 0 /*sm.targetPlayerIsDead*/)                            // 적과의 거리가 너무 멀어지거나 현재 공격중인 적의 체력이 0 이하일 때
            {
                Debug.LogFormat("{0} 제거 완료", sm.targetPlayer.name);

                sm.UnitStateChange(sm.idleState);                           // idle 상태로 변경
            }
            else
            {
                Attack();
            }
        }
    }
    public void Exit()
    {
        //sm.anim.SetBool("Attack", false);
        //sm.targetPlayer = null;
    }

    void Attack()
    {
        // 시야는 공격 대상을 정면으로 바라봄
        unit.transform.LookAt(sm.targetPlayer.transform.position);

        // 사정거리 안에 있는 적들 중 하나에게 유닛의 공격력 수치를 전달 -> 적의 health 부분에서 받는 모든 데미지 계산

        Debug.LogFormat("{0}이 {1}을 공격", unit.name, sm.targetPlayer.name);
        //    sm.anim.SetBool("Attack", true);
        //    //sm.enemyAudioSource.Play();
        sm.targetPlayer.GetComponent<Health>().CalculateHP(sm.attackDemage);
        //    currentTime = 0;
        //}
    }
}