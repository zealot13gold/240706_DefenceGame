using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyMoveState : IState
{
    EnemyUnitSM sm;
    EnemyHealth health;
    NavMeshAgent navMesh;
    GameObject unit;

    public EnemyMoveState(GameObject unit)
    {
        sm = unit.GetComponent<EnemyUnitSM>();
        health = unit.GetComponent<EnemyHealth>();
        navMesh = unit.GetComponent<NavMeshAgent>();
        this.unit = unit;
    }

    float currentTime = 0f;
    public void Enter()
    {
        sm.unitState = EnemyUnitSM.unitStateList.move;

        navMesh.speed = sm.moveSpeed;
        navMesh.destination = StageManager.instance.enemyDest.position;
        Debug.LogFormat("EnemyMoveState: {0}은 현재 move 상태, 이동목적지: {1}", unit.name, navMesh.destination);
    }

    public void Update()
    {
        if (health.currentHP <= 0)                      // 적의 체력이 0인지 확인
        {
            sm.UnitStateChange(sm.deadState);
        }

        sm.targetPlayer = FindEnemy();                           // isAttackMove 값을 확인함으로써 적이 존재하는지 확인

        if (sm.targetPlayer != null)                 // 강제 이동을 하지 않는 상태에서 적이 시야 내에 존재할 경우
        {
            //if (sm.unitState)
            //{
                sm.UnitStateChange(sm.moveToAttackState);
            //}
            //else
            //{
            //    sm.UnitStateChange(sm.attackState);
            //}
        }


        if(unit.transform.position == navMesh.destination)
        {
            sm.UnitStateChange(sm.idleState);
        }
    }
    public void Exit()
    {
        //sm.anim.SetBool("Attack", false);
    }

    GameObject FindEnemy()
    {
        Debug.LogFormat("EnemyUnitSM: 플레이어 유닛 검색");
        Collider[] players = Physics.OverlapSphere(unit.transform.position, sm.viewRange, sm.playerLayerMask);             // 유닛의 현재 위치에서 시야(viewPoint) 내에 적이 존재하는지 확인

        GameObject targetPlayer = null;
        Vector3 bufferPlayerPos;                                                                          // 적의 위치를 임시 저장
        float bufferEnemyDist = 10000f;                                                                  // 플레이어 유닛과 적 사이의 거리를 임시 저장

        if (players.Length <= 0) return null;

        for (int i = 0; i < players.Length; i++)
        {
            Debug.LogFormat("EnemyUnitSM: {0}이 {1}을 발견", unit.name, players[i].gameObject.name);
            bufferPlayerPos = players[i].transform.position;                                              // 공격 대상의 위치를 임시저장

            if (bufferEnemyDist > Mathf.Abs((bufferPlayerPos - unit.transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
            {
                targetPlayer = players[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장

                Debug.LogFormat("EnemyUnitSM: {0}의 타겟은 {1}", unit.name, targetPlayer.name);
                float targetPlayerHP = targetPlayer.GetComponent<PlayerHealth>().currentHP;                                      // 타겟 정보 설정
                //targetPlayerIsDead = targetPlayer.GetComponent<PlayerHealth>().isDead;

                if (targetPlayerHP<=0) continue;

                bufferEnemyDist = Mathf.Abs((targetPlayer.transform.position - unit.transform.position).magnitude);    // 가까운 적과의 거리를 저장
            }
        }

        return targetPlayer;
    }
    
    
}
