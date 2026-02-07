using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PlayerMoveState : IState
{
    PlayerUnitSM sm;
    PlayerHealth health;
    NavMeshAgent navMesh;
    GameObject unit;
    public PlayerMoveState(GameObject unit)
    {
        sm = unit.GetComponent<PlayerUnitSM>();
        health = unit.GetComponent<PlayerHealth>();
        navMesh = unit.GetComponent<NavMeshAgent>();
        this.unit= unit;
    }

    public void Enter()
    {
        Debug.LogFormat("{0}은 현재 movestate 상태, {1}로 이동", unit.name, sm.dest);
        //navMesh.isStopped = false;

        sm.navMesh.speed = sm.moveSpeed;

        navMesh.destination = sm.dest;
        //navMesh.SetDestination(sm.dest);
    }

    public void Update()        // UnitSM에서 FixedUpdate()로 프레임마다 실행
    {
        if (health.currentHP <= 0)
        {
            sm.UnitStateChange(sm.deadState);
        }
        else
        {
            sm.anim.SetBool("Walk", true);
            //Debug.LogFormat("{0} 강제이동?: {1}", unit.name, sm.anim.GetCurrentAnimatorStateInfo(0).IsName("Walk"));

            sm.ForceMove();                         // 강제이동으로 목적지에 도달하였는지 확인

            if (sm.isForceMove)                     // 계속 이동
            {
                Debug.LogFormat("{0} 목적지 변경: {1}", unit, sm.dest);
                //navMesh.SetDestination(sm.dest);
                navMesh.destination = sm.dest;          // 현재 프레임에 저장된 목적지로 이동 - 갱신된 목적지 반영
            }

            // 목적지이면 완전히 멈춤
            else
            {
                sm.UnitStateChange(sm.idleState);
            }
        }
    }
    public void Exit()
    {
        sm.anim.SetBool("Walk", false) ;
        sm.dest = unit.transform.position;
        Debug.LogFormat("{0} Walk 해제", unit.name);
    }
}