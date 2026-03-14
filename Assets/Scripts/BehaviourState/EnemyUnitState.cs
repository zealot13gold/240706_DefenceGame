//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.AI;

//public class EnemyUnitState : BaseState
//{
//    protected GameObject unit;                        // 유닛 오브젝트
//    //[HideInInspector]public Vector3 target;                          // 목표(적 위치)
//    //[HideInInspector]public Vector3 dest;                            // 유닛 목적지
//    public Rigidbody unitRigid = null;

//    protected EnemyUnitSM sm;
//    protected NavMeshAgent navMesh;
//    protected EnemyHealth health;
//    //protected PlayerUnit unitScript;
//    //protected Vector3 beforeDest;


//    protected EnemyUnitState(GameObject unit)
//    {
//        this.unit = unit;

//        //unitScript = unit.GetComponent<PlayerUnit>();
//        sm = unit.GetComponent<EnemyUnitSM>();
//        navMesh = unit.GetComponent<NavMeshAgent>();
//        health = unit.GetComponent<EnemyHealth>();
//    }

//    public override void OnStateEnter()
//    {
//        Debug.LogFormat("{0}은 현재 {1} 상태", unit.name, sm.currentState);
//    }

//    // Start is called before the first frame update
//    public override void OnStateUpdate()
//    {

//    }

//    // Update is called once per frame
//    public override void OnStateExit()             // 상태 종료
//    {
//        //Debug.LogFormat("{0} : {1} 상태 종료", unit.name, sm.currentState);
//        unitRigid = null;

//        //sm.enemyAudioSource.Stop();                 // 상태 변경 때마다 이에 해당하는 오디오 클립 종료
//    }
//}
