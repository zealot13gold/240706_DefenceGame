using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveToAttackState : IState
{
    PlayerUnitSM sm;
    PlayerHealth health;
    NavMeshAgent navMesh;
    GameObject unit;
    public PlayerMoveToAttackState(GameObject unit)
    {
        sm = unit.GetComponent<PlayerUnitSM>();
        health = unit.GetComponent<PlayerHealth>();
        navMesh = unit.GetComponent<NavMeshAgent>();
        this.unit = unit;
    }

    public void Enter()
    {

        sm.navMesh.speed = sm.moveSpeed;
        //navMesh.destination = sm.targetEnemy.transform.position;

        sm.playerUnitVoice.clip = sm.playerDiscoverEnemyVoice;
        sm.playerUnitVoice.Play();
    }

    public void Update()
    {
        if (health.currentHP <= 0)
        {
            sm.UnitStateChange(sm.deadState);
        }
        else {
            sm.FindEnemy();                             // 가까운 적을 찾고, 더 가까운 적이 검색될 경우 타겟을 변경
            sm.ForceMove();                         // 강제이동으로 목적지에 도달하였는지 확인

            //Debug.LogFormat("{0} AttackMove: {1}, ForceMove: {2}", unit.name, sm.isAttackMove, sm.isForceMove);

            if (!sm.targetEnemy.activeSelf)                 // 적이 없으면 idle 상태가 됨
            {
                sm.UnitStateChange(sm.idleState);
            }

            else if (sm.isAttackMove && !sm.isForceMove)                        // 가까운 곳에 적이 존재하면 isAttackMove는 true가 됨
            {
                navMesh.destination = sm.targetEnemy.transform.position;
                sm.anim.SetBool("Walk", true);
            }
            else
            {
                sm.UnitStateChange(sm.idleState);           // 적은 감지되나, 위의 조건에 해당하지 않으면 일단 idleState로 이동 
            }
        }
    }

    public void Exit()
    {
        sm.anim.SetBool("Walk", false);
        //sm.targetEnemy = null;



        //Debug.LogFormat("{0} Walk 해제", unit.name);
    }
}