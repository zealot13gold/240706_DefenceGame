using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveToAttackState : EnemyUnitState
{

    public EnemyMoveToAttackState(GameObject unit) : base(unit) { }

    public override void OnStateEnter()
    {
        //Debug.LogFormat("{0}은 현재 move to attack 상태, {1}에게 이동", unit.name, sm.targetEnemy.name);

        //navMesh.isStopped = false;
        sm.navMesh.speed = sm.moveSpeed;
        navMesh.destination = sm.targetEnemy.transform.position;

        sm.enemyAudioSource.clip = sm.enemyScreamingVoice;
        sm.enemyAudioSource.Play();
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

            //Debug.LogFormat("{0} AttackMove: {1}", unit.name, sm.isAttackMove);

            if (!sm.targetEnemy.activeSelf)                 // 적이 없으면 idle 상태가 됨
            {
                sm.ChangeState(sm.idleState);
            }

            else if (sm.isAttackMove /*&& !sm.isForceMove*/)                        // 가까운 곳에 적이 존재하면 isAttackMove는 true가 됨
            {
                navMesh.destination = sm.targetEnemy.transform.position;
                sm.anim.SetBool("Walk", true);
            }
            else
            {
                sm.ChangeState(sm.idleState);           // 적은 감지되나, 위의 조건에 해당하지 않으면 일단 idleState로 이동 
            }
        }
    }

    public override void OnStateExit()
    {
        sm.anim.SetBool("Walk", false);
        sm.targetEnemy = null;
        base.OnStateExit();


        //Debug.LogFormat("{0} Walk 해제", unit.name);
    }
}