using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerUnitSM : StateMachine
{
    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerAttackState attackState;
    public PlayerMoveToAttackState moveToAttackState;
    public PlayerDeadState deadState;

    [HideInInspector] public Collider[] enemies;
    [HideInInspector] public GameObject targetEnemy;

    public LayerMask enemyLayerMask;
    public float viewRange;
    public float attackRange;
    public float attackDelayTime;
    public float attackDemage;

    public Vector3 dest;                                        // 유닛 강제 이동, 목적지는 마우스 우클릭 시 업데이트됨
    public NavMeshAgent navMesh;
    //[HideInInspector] public Rigidbody unitRigidbody;           // 유닛의 rigidbody
    public float moveSpeed;                                     // 유닛의 이동속도

    // 이동, 공격 여부
    [HideInInspector] public bool isForceMove;
    [HideInInspector] public bool isAttackMove;
    [HideInInspector] public bool isFire;

    // 애니메이션
    public Animator anim;

    // 음향효과
    public AudioSource gunFireSound;
    public AudioSource playerUnitVoice;
    public AudioClip playerSelectedVoice;
    public AudioClip playerForcedMoveVoice;
    public AudioClip playerDiscoverEnemyVoice;
    public AudioClip playerAttackVoice;
    public AudioClip playerDeadVoice;

    protected override void Awake()
    {
        base.Awake();

        // 아래 두줄은 플레이어/적 사망 state 작성 후 사망 state로 이동
        targetEnemy = null;
        dest = PlayerUnitPooling.Instance.playerUnitSpawnPoint.position;

        idleState = new PlayerIdleState(gameObject);
        moveState = new PlayerMoveState(gameObject);
        attackState = new PlayerAttackState(gameObject);
        moveToAttackState = new PlayerMoveToAttackState(gameObject);
        deadState = new PlayerDeadState(gameObject);


        //unitRigidbody = GetComponent<Rigidbody>();
    }

    protected override void Start()
    {
        isForceMove = false;
        isAttackMove = false;
        isFire = false;

        ChangeState(idleState);
    }

    protected override void OnEnable()
    {
        isForceMove = false;
        isAttackMove = false;
        isFire = false;

        ChangeState(idleState);
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        currentState.OnStateUpdate();

    }

    public void ForceMove()                                                                     // 강제이동, 플레이어 유닛이 지정된 목적지에 도달하였는지 확인, isMove 값을 변경
    {
        Debug.LogFormat("{0} 목적지: {1}", gameObject, dest);

        if(Mathf.Abs((dest- transform.position).magnitude)>1.5f)
        {
            isForceMove = true;
        }
        else
        {
            isForceMove = false;
        }
    }

    public void AttackMove()
    {
        if (Mathf.Abs((targetEnemy.transform.position - transform.position).magnitude) > attackRange)               // 적이 플레이어 유닛의 공격 사정거리보다 먼 곳에 위치하면
        {
            isAttackMove = true;                                                            // 플레이어 유닛이 공격을 위해 이동
        }
        else
        {
            isAttackMove = false;                                                           // 적이 사정거리 안에 위치하면 더 이상 이동하지 않음
        }
    }

    // 적 
    public void FindEnemy()                                                                         // 적이 존재하는지 확인(idleState 상태에서 지속적으로 확인), isAttack 값을 변경
    {
        enemies = Physics.OverlapSphere(transform.position, viewRange, enemyLayerMask);             // 유닛의 현재 위치에서 시야(viewPoint) 내에 적이 존재하는지 확인

        Vector3 bufferEnemyPos;                                                                          // 적의 위치를 임시 저장
        float bufferEnemyDist = 10000f;                                                                  // 플레이어 유닛과 적 사이의 거리를 임시 저장

        float targetEnemyHealth = GetComponent<Health>().currentHP;
        bool targetEnemyIsDead = GetComponent<Health>().IsDead;

        //if (enemies.Length <= 0)
        //{
        //    //Debug.LogFormat("{0} 주변에 적이 발견되지 않음", gameObject.name);
        //    return;
        //}

        for (int i = 0; i < enemies.Length; i++)
        {
            //Rigidbody targetRigidbody = enemies[i].GetComponent<Rigidbody>();                       // 탐색된 적들의 rigidbody를 가져옴

            //if (!targetRigidbody)                                                                   // 적이 발견되지 않았을 경우 함수를 종료
            //{
            //    //Debug.LogFormat("{0} 주변에 적이 발견되지 않음", gameObject.name);
            //    return;
            //}

            //Debug.LogFormat("{0}이 {1}을 발견", gameObject.name, enemies[i].gameObject.name);
            bufferEnemyPos = enemies[i].transform.position;                                              // 공격 대상의 위치를 임시저장

            if (bufferEnemyDist > Mathf.Abs((bufferEnemyPos - transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
            {
                targetEnemy = enemies[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장
                bufferEnemyDist = Mathf.Abs((targetEnemy.transform.position - transform.position).magnitude);    // 가까운 적과의 거리를 저장
            }
        }

        if (targetEnemy!=null && targetEnemyHealth>0)                                                                            // 적 오브젝트가 null이 아닐 경우(적이 존재할 경우)
        {
            //target = targetEnemy.transform.position;                                                // 가장 가까운 적의 위치를 target에 저장
            AttackMove();
        }
    }

}
