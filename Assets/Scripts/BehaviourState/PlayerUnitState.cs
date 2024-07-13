using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitState : BaseState
{
    protected GameObject unit;                        // 유닛 오브젝트
    //[HideInInspector]public Vector3 target;                          // 목표(적 위치)
    //[HideInInspector]public Vector3 dest;                            // 유닛 목적지
    public Rigidbody unitRigid = null;

    protected PlayerUnitSM sm;
    protected NavMeshAgent navMesh;
    protected PlayerUnit unitScript;
    protected PlayerHealth health;
    //protected Vector3 beforeDest;


    protected PlayerUnitState(GameObject unit)
    {
        this.unit = unit;

        unitScript = unit.GetComponent<PlayerUnit>();
        sm = unit.GetComponent<PlayerUnitSM>();
        navMesh = unit.GetComponent<NavMeshAgent>();
        health = unit.GetComponent<PlayerHealth>();
    }

    public override void OnStateEnter()
    {

    }

    // Start is called before the first frame update
    public override void OnStateUpdate()
    {
        
    }

    // Update is called once per frame
    public override void OnStateExit()             // 상태 종료
    {
        Debug.LogFormat("{0} : {1} 상태 종료", unit.name, sm.currentState);
        unitRigid = null;
    }
}
