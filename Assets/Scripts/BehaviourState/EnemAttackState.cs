using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyUnitState
{
    public EnemyAttackState(GameObject unit) : base(unit) { }

    float currentTime = 0f;
    public override void OnStateEnter()
    {
        //navMesh.isStopped = true;
        //Debug.LogFormat("{0}은 현재 attack 상태, 시야범위: {1}, 공격범위: {2}", unit.name, sm.viewRange, sm.attackRange);

        sm.enemyAudioSource.clip = sm.enemyAttackVoice;

    }

    public override void OnStateUpdate()
    {
        if (health.currentHP <= 0)
        {
            sm.ChangeState(sm.deadState);
        }
        else
        {
            sm.FindEnemy();                             // 가까운 적을 찾고, 더 가까운 적이 검색될 경우 타겟을 변경
            Debug.LogFormat("타겟 체력: {0}", sm.targetPlayerHealth);

            if (sm.isAttackMove || /*!sm.targetPlayer.activeSelf*/sm.targetPlayerHealth <= 0 /*sm.targetPlayerIsDead*/)                            // 적과의 거리가 너무 멀어지거나 현재 공격중인 적의 체력이 0 이하일 때
            {
                if(sm.targetPlayerIsDead) Debug.LogFormat("{0} 제거 완료", sm.targetPlayer.name); 
                
                sm.ChangeState(sm.idleState);                           // idle 상태로 변경
            }
            else
            {
                Attack();

            }
        }
    }
    public override void OnStateExit()
    {
        sm.anim.SetBool("Attack", false);
        sm.targetPlayer = null;
    }

    void Attack()
    {
        // 시야는 공격 대상을 정면으로 바라봄
        unit.transform.LookAt(sm.targetPlayer.transform.position);

        // 사정거리 안에 있는 적들 중 하나에게 유닛의 공격력 수치를 전달 -> 적의 health 부분에서 받는 모든 데미지 계산

        if (sm.attackRange > 1.5f)                    // 사정거리가 1.5을 초과하면 원거리, 아니면 근거리 유닛
        {
            // 원거리 유닛

        }
        else
        {
            // 근거리 유닛

        }

        if (currentTime < sm.attackDelayTime)
        {
            currentTime += Time.deltaTime;
            //Debug.LogFormat("{0}은 {1}초 후에 공격", unit.name, sm.attackDelayTime - currentTime);
        }
        else
        {
            //Debug.LogFormat("{0}이 {1}을 공격", unit.name, sm.targetEnemy.name);
            sm.anim.SetBool("Attack", true);
            sm.enemyAudioSource.Play();
            sm.targetPlayer.GetComponent<Health>().CalculateHP(sm.attackDemage);
            currentTime = 0;
        }
    }
}