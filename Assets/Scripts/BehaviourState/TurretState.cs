using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class TurretState : BaseState
{
    protected GameObject unit;                        // 유닛 오브젝트

    protected TurretSM sm;
    protected TurretHealth health;
    //protected GameObject turretHead;

    protected TurretState(GameObject unit)
    {
        this.unit = unit;

        //unitScript = unit.GetComponent<PlayerUnit>();
        sm = unit.GetComponent<TurretSM>();
        health = unit.GetComponent<TurretHealth>();
        //turretHead = unit.GetComponentInChildren<GameObject>();
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
    }
}
