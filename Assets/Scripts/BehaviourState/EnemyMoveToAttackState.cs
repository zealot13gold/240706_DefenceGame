using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveToAttackState : IState
{
    EnemyUnitSM sm;
    EnemyHealth health;
    GameObject unit;
    NavMeshAgent navMesh;
    public EnemyMoveToAttackState(GameObject unit) 
    {
        sm = unit.GetComponent<EnemyUnitSM>();
        health = unit.GetComponent<EnemyHealth>();
        navMesh = unit.GetComponent<NavMeshAgent>();
        this.unit = unit;
    }

    PlayerHealth playerHealth;
    float dist;

    public void Enter()
    {

        //Debug.LogFormat("{0}은 현재 move to attack 상태, {1}에게 이동", unit.name, sm.targetEnemy.name);

        //navMesh.isStopped = false;
        sm.navMesh.speed = sm.moveSpeed;
        navMesh.destination = sm.targetPlayer.transform.position;
        dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리

        playerHealth = sm.targetPlayer.GetComponent<PlayerHealth>();

        //sm.enemyAudioSource.clip = sm.enemyScreamingVoice;
        //sm.enemyAudioSource.Play();
    }

    public void Update()
    {
        if (health.currentHP <= 0)
        {
            sm.UnitStateChange(sm.deadState);
        }

        sm.targetPlayer = FindEnemy();                             // 이미 발견한 무리 내에서 가장 가까운 적을 검색
       //Debug.LogFormat("{0} AttackMove: {1}", unit.name, sm.isAttackMove);

        if (sm.targetPlayer != null && playerHealth.currentHP <=0)                 // 적이 없으면 idle 상태가 됨
        {
            sm.UnitStateChange(sm.idleState);
        }

        navMesh.destination = sm.targetPlayer.transform.position;
        dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리 갱신

        if (dist <=1f)                        // 가까운 곳에 적이 존재하면 isAttackMove는 true가 됨
        {
            sm.UnitStateChange(sm.attackState);
        }
        //else
        //{
        //    sm.UnitStateChange(sm.idleState);           // 적은 감지되나, 위의 조건에 해당하지 않으면 일단 idleState로 이동 
        //}

    }

    public void Exit()
    {
        sm.anim.SetBool("Walk", false);
        sm.targetPlayer = null;
        //base.OnStateExit();


        //Debug.LogFormat("{0} Walk 해제", unit.name);
    }

    GameObject FindEnemy()
    {
        GameObject targetPlayer = null;
        Vector3 bufferPlayerPos;                                                                          // 적의 위치를 임시 저장
        float bufferEnemyDist = dist;                                                                  // 플레이어 유닛과 적 사이의 거리를 임시 저장, 초기값은 현재 타겟과의 거리로 설정

        for (int i = 0; i < sm.players.Length; i++)
        {
            if (Mathf.Abs((sm.players[i].transform.position - unit.transform.position).magnitude) <= sm.viewRange)          // 현재 시야 안에 있는지 확인
            {
                GameObject bufferPlayer = sm.players[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장
                //if (targetPlayer == sm.targetPlayer) continue;                                                                 // 이미 발견한 적은 건너뜀

                Debug.LogFormat("EnemyUnitSM: {0}이 {1}을 발견", unit.name, bufferPlayer.gameObject.name);
                bufferPlayerPos = bufferPlayer.transform.position;                                              // 공격 대상의 위치를 임시저장

                if (bufferEnemyDist > Mathf.Abs((bufferPlayerPos - unit.transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
                {
                    if (sm.players[i].gameObject.GetComponent<PlayerHealth>().currentHP <= 0) continue;
                    Debug.LogFormat("EnemyUnitSM: {0}의 타겟은 {1}", unit.name, sm.players[i].gameObject.name);

                    bufferEnemyDist = Mathf.Abs((sm.players[i].gameObject.transform.position - unit.transform.position).magnitude);    // 가까운 적과의 거리를 저장
                    targetPlayer = sm.players[i].gameObject;
                }
            }
            else
            {
                sm.players[i] = null;                                                                                 // 시야 안에 없는 적은 null로 설정
            }

        }

        return targetPlayer;
    }
}