//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyUnit : MonoBehaviour
//{
//    // 애니매이션
//    public Animator unitAnim;

//    // 이동
//    protected EnemyUnitSM unitSM;
//    public EnemyUnitState unitState;
//    public Vector3 dest;                    // 오른쪽 클릭 시 목적지
//    [HideInInspector] public bool forceMove;
//    [HideInInspector] public bool isAttack;

//    // 체력
//    [HideInInspector] public Health unitHealth;

//    protected virtual void Awake()
//    {
//        unitSM = GetComponent<EnemyUnitSM>();
//        unitHealth = GetComponent<Health>();
//    }

//    protected virtual void Start()
//    {
//        dest = transform.position;
//    }
//}
