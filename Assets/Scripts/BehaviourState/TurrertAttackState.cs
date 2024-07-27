using System.Collections;
using UnityEngine;

public class TurrertAttackState : TurretState
{
    public TurrertAttackState(GameObject unit) : base(unit) { }

    float currentTime = 0f;
    public override void OnStateEnter()
    {
        Debug.LogFormat("{0}은 현재 attack 상태, 공격범위: {1}", unit.name, sm.attackRange);

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

            if (!sm.targetEnemy.activeSelf)                            // 적과의 거리가 너무 멀어지면
            {
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
        sm.targetEnemy = null;
        sm.isFire = false;
    }

    void Attack()
    {
        // 시야는 공격 대상을 정면으로 바라봄
        sm.turretHead.transform.LookAt(sm.targetEnemy.transform.position);
        
        // 사정거리 안에 있는 적들 중 하나에게 유닛의 공격력 수치를 전달 -> 적의 health 부분에서 받는 모든 데미지 계산
        if (currentTime < sm.attackDelayTime)
        {
            sm.isFire = false;
            currentTime += Time.deltaTime;
            //Debug.LogFormat("{0}은 {1}초 후에 공격", unit.name, sm.attackDelayTime - currentTime);
        }
        else
        {
            //Debug.LogFormat("{0}이 {1}을 공격", unit.name, sm.targetEnemy.name);
            sm.isFire = true;
            sm.targetEnemy.GetComponent<Health>().CalculateHP(sm.attackDemage);
            sm.gunFireSound.Play();
            currentTime = 0;
        }
    }
}