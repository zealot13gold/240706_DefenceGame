using NUnit.Framework;
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

    EnemyHealth enemyHealth;
    float dist;

    public void Enter()
    {

        //sm.navMesh.speed = sm.moveSpeed;
        //navMesh.destination = sm.targetEnemy.transform.position;

        //sm.playerUnitVoice.clip = sm.playerDiscoverEnemyVoice;
        //sm.playerUnitVoice.Play();
        dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리 갱신
    }

    public void Update()
    {
        if (health.currentHP <= 0)
        {
            sm.UnitStateChange(sm.deadState);
        }
        else {
            sm.targetEnemy = UpdateCloserEnemy();                             // 가까운 적을 찾고, 더 가까운 적이 검색될 경우 타겟을 변경
            enemyHealth = sm.targetEnemy.GetComponent<EnemyHealth>();
            //sm.ForceMove();                         // 강제이동으로 목적지에 도달하였는지 확인

            //Debug.LogFormat("{0} AttackMove: {1}, ForceMove: {2}", unit.name, sm.isAttackMove, sm.isForceMove);

            if (sm.targetEnemy == null || enemyHealth.currentHP <=0)                 // 적이 없으면 idle 상태가 됨
            {
                sm.UnitStateChange(sm.idleState);
            }

            Debug.LogFormat("EnemyMoveToAttackState: {0}의 타겟은 {1}", unit.name, sm.targetEnemy.gameObject.name);
            navMesh.destination = sm.targetEnemy.transform.position;
            dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리 갱신

            if (dist <= 1f)                        // 가까운 곳에 적이 존재하면 isAttackMove는 true가 됨
            {
                sm.UnitStateChange(sm.attackState);
            }

            Debug.LogFormat("EnemyMoveToAttackState: {0}의 타겟은 {1}", unit.name, sm.targetEnemy.gameObject.name);
            navMesh.destination = sm.targetEnemy.transform.position;
            dist = (navMesh.destination - unit.transform.position).sqrMagnitude;      // 적 유닛과 target 사이의 거리 갱신

            if (dist <= 1f)                        // 가까운 곳에 적이 존재하면 isAttackMove는 true가 됨
            {
                sm.UnitStateChange(sm.attackState);
            }
        }

    public void Exit()
    {
        sm.anim.SetBool("Walk", false);
        //sm.targetEnemy = null;



        //Debug.LogFormat("{0} Walk 해제", unit.name);
    }

    GameObject UpdateCloserEnemy()                                                                     // 이미 발견한 무리 내에서 가장 가까운 적을 검색
    {
        GameObject targetPlayer = sm.targetEnemy;                                                                 // 결과값, 타겟 적 유닛을 임시 저장
        Vector3 bufferPlayerPos;
        float bufferPlayerDist = dist;                                                                   // 플레이어 유닛과 적 사이의 거리를 임시 저장, 초기값은 현재 타겟과의 거리로 설정

        for (int i = 0; i < sm.enemies.Length; i++)
        {

            if (Mathf.Abs((sm.enemies[i].transform.position - unit.transform.position).magnitude) <= sm.viewRange)          // 현재 시야 안에 있는지 확인
            {
                //if (targetPlayer == sm.targetPlayer) continue;                                                                 // 이미 발견한 유닛은 건너뜀

                Debug.LogFormat("EnemyMoveToAttackState: {0}이 {1}을 발견", unit.name, sm.enemies[i].gameObject.gameObject.name);
                bufferPlayerPos = sm.enemies[i].gameObject.transform.position;                                              // 거리 계산을 위해 적 유닛의 위치를 임시 저장

                if (bufferPlayerDist > Mathf.Abs((bufferPlayerPos - unit.transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
                {
                    if (sm.enemies[i].gameObject.GetComponent<PlayerHealth>().currentHP <= 0) continue;
                    Debug.LogFormat("EnemyUnitSM: {0}의 새로운 타겟 {1}", unit.name, sm.enemies[i].gameObject.name);

                    bufferPlayerDist = Mathf.Abs((bufferPlayerPos - unit.transform.position).magnitude);    // 가까운 적과의 거리를 저장
                    targetPlayer = sm.enemies[i].gameObject;
                }
            }
        }
        return targetPlayer;
    }
}