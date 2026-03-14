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
        sm.unitState = EnemyUnitSM.unitStateList.attackMove;
        //Debug.LogFormat("{0}은 현재 move to attack 상태, {1}에게 이동", unit.name, sm.targetEnemy.name);

        //navMesh.isStopped = false;
        sm.navMesh.speed = sm.moveSpeed;
        navMesh.destination = sm.targetPlayer.transform.position;
        dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리



        //sm.enemyAudioSource.clip = sm.enemyScreamingVoice;
        //sm.enemyAudioSource.Play();
    }

    public void Update()
    {
        if (health.currentHP <= 0)
        {
            sm.UnitStateChange(sm.deadState);
        }

        sm.targetPlayer = UpdateCloserPlayer();                             // 이미 발견한 무리 내에서 가장 가까운 적을 검색
        playerHealth = sm.targetPlayer.GetComponent<PlayerHealth>();
        //Debug.LogFormat("{0} AttackMove: {1}", unit.name, sm.isAttackMove);

        if (sm.targetPlayer == null || playerHealth.currentHP <=0)                 // 적이 없으면 idle 상태가 됨
        {
            sm.UnitStateChange(sm.idleState);
        }

        Debug.LogFormat("EnemyMoveToAttackState: {0}의 타겟은 {1}", unit.name, sm.targetPlayer.gameObject.name);
        navMesh.destination = sm.targetPlayer.transform.position;
        dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리 갱신

        if (dist <= 1f)                        // 가까운 곳에 적이 존재하면 isAttackMove는 true가 됨
        {
            sm.UnitStateChange(sm.attackState);
        }

        Debug.LogFormat("EnemyMoveToAttackState: {0}의 타겟은 {1}", unit.name, sm.targetPlayer.gameObject.name);
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
        //sm.targetPlayer = null;
        //base.OnStateExit();


        //Debug.LogFormat("{0} Walk 해제", unit.name);
    }

    GameObject UpdateCloserPlayer()                                                                     // 이미 발견한 무리 내에서 가장 가까운 플레이어를 검색
    {
        GameObject targetPlayer = sm.targetPlayer;                                                                 // 결과값, 타겟 플레이어 유닛을 임시 저장
        Vector3 bufferPlayerPos;
        float bufferPlayerDist = dist;                                                                   // 플레이어 유닛과 적 사이의 거리를 임시 저장, 초기값은 현재 타겟과의 거리로 설정

        for (int i = 0; i < sm.players.Length; i++)
        {
                                                                 
            if (Mathf.Abs((sm.players[i].transform.position - unit.transform.position).magnitude) <= sm.viewRange)          // 현재 시야 안에 있는지 확인
            {   
                //if (targetPlayer == sm.targetPlayer) continue;                                                                 // 이미 발견한 유닛은 건너뜀

                Debug.LogFormat("EnemyMoveToAttackState: {0}이 {1}을 발견", unit.name, sm.players[i].gameObject.gameObject.name);
                bufferPlayerPos = sm.players[i].gameObject.transform.position;                                              // 거리 계산을 위해 플레이어 유닛의 위치를 임시 저장

                if (bufferPlayerDist > Mathf.Abs((bufferPlayerPos - unit.transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
                {
                    if (sm.players[i].gameObject.GetComponent<PlayerHealth>().currentHP <= 0) continue;
                    Debug.LogFormat("EnemyUnitSM: {0}의 새로운 타겟 {1}", unit.name, sm.players[i].gameObject.name);

                    bufferPlayerDist = Mathf.Abs((bufferPlayerPos - unit.transform.position).magnitude);    // 가까운 플레이어와의 거리를 저장
                    targetPlayer = sm.players[i].gameObject;
                }
            }
        }
        return targetPlayer;
    }
}