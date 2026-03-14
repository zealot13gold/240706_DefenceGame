using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static PlayerUnitSM;

public class EnemyUnitSM : MonoBehaviour
{
    public IState idleState;
    public IState moveState;
    public IState attackState;
    public IState moveToAttackState;
    public IState deadState;

    IState currentState = null;

    // 적 정보
    [HideInInspector] public Collider[] players;
    [HideInInspector] public GameObject targetPlayer;
    //[HideInInspector] public float targetPlayerHealth;
    //[HideInInspector] public bool targetPlayerIsDead;

    public LayerMask playerLayerMask;
    public float viewRange;
    public float attackRange;
    public float attackDelayTime;
    public float attackDemage;

    //public Vector3 dest;                                        // 유닛 강제 이동, 목적지는 마우스 우클릭 시 업데이트됨
    public NavMeshAgent navMesh;
    //[HideInInspector] public Rigidbody unitRigidbody;           // 유닛의 rigidbody
    public float moveSpeed;                                     // 유닛의 이동속도
   //public Vector3 finalDest;                                       // 유닛의 최종 목적지

    // 이동, 공격 여부
    public enum unitStateList
    {
        idle,
        move,
        attackMove,
        forceMove,
        attack,
        death
    }
    public unitStateList unitState;
    //[HideInInspector] public bool isAttackMove;

    // 애니메이션
    public Animator anim;

    // 오디오 클립
    //public AudioSource enemyAudioSource;
    //public AudioClip enemySearchingVoice;                        // 대기 상태일 때
    //public AudioClip enemyScreamingVoice;                        // 이동 상태일 때
    //public AudioClip enemyAttackVoice;                           // 공격 상태일 때
    //public AudioClip enemyDeadVoice;                             // 사망하였을 때

    // 코루틴
    Coroutine coroutine;

    protected void Awake()
    {
        //base.Awake();

        // 유닛 상태 정보
        idleState = new EnemyIdleState(gameObject);
        moveState = new EnemyMoveState(gameObject);
        attackState = new EnemyAttackState(gameObject);
        moveToAttackState = new EnemyMoveToAttackState(gameObject);
        deadState = new EnemyDeadState(gameObject);

        targetPlayer = null;

        //enemyAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        Init();
    }

    void Init()
    { 
        //isAttackMove = false;


        currentState = null;

        UnitStateChange(moveState);
    }

    public void UnitStateChange(IState state)
    {
        Debug.LogFormat("EnemySM: {0} 현재 상태: {1}", gameObject.name, currentState);
        if (state != currentState)
        {
            if (currentState != null)
            {
                currentState.Exit();
                if(coroutine != null) StopCoroutine(coroutine);
            }
            state.Enter();
            currentState = state;
           coroutine = StartCoroutine(StateUpdate(currentState));
        }
    }

    public void AttackMove()
    {
        if (Mathf.Abs((targetPlayer.transform.position - transform.position).magnitude) > attackRange)               // 적이 플레이어 유닛의 공격 사정거리보다 먼 곳에 위치하면
        {
            UnitStateChange(attackState);                                                            // 플레이어 유닛이 공격을 위해 이동
        }
    }

    IEnumerator StateUpdate(IState state)
    {
        while (true)
        {
            state.Update();
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 적 
    //public void FindEnemy()                                                                         // 적이 존재하는지 확인(idleState 상태에서 지속적으로 확인), isAttack 값을 변경
    //{
    //    Debug.LogFormat("EnemyUnitSM: 플레이어 유닛 검색");
    //    players = Physics.OverlapSphere(transform.position, viewRange, playerLayerMask);             // 유닛의 현재 위치에서 시야(viewPoint) 내에 적이 존재하는지 확인

    //    Vector3 bufferPlayerPos;                                                                          // 적의 위치를 임시 저장
    //    float bufferEnemyDist = 10000f;                                                                  // 플레이어 유닛과 적 사이의 거리를 임시 저장

    //    if (players.Length <= 0) return;

    //    for (int i = 0; i < players.Length; i++)
    //    {
    //        Debug.LogFormat("EnemyUnitSM: {0}이 {1}을 발견", gameObject.name, players[i].gameObject.name);
    //        bufferPlayerPos = players[i].transform.position;                                              // 공격 대상의 위치를 임시저장

    //        if (bufferEnemyDist > Mathf.Abs((bufferPlayerPos - transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
    //        {
    //            targetPlayer = players[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장

    //            Debug.LogFormat("EnemyUnitSM: {0}의 타겟은 {1}", gameObject.name, targetPlayer.name);                                                                                         
    //            targetPlayerHealth = targetPlayer.GetComponent<PlayerHealth>().currentHP;                                      // 타겟 정보 설정
    //            targetPlayerIsDead = targetPlayer.GetComponent<PlayerHealth>().isDead;

    //            if (targetPlayerIsDead) continue;

    //            bufferEnemyDist = Mathf.Abs((targetPlayer.transform.position - transform.position).magnitude);    // 가까운 적과의 거리를 저장
    //        }
    //    }

    //    if (targetPlayer!=null && targetPlayerHealth>0)                                                                            // 적 오브젝트가 null이 아닐 경우(적이 존재할 경우)
    //    {
    //        //target = targetEnemy.transform.position;                                                // 가장 가까운 적의 위치를 target에 저장
    //        AttackMove();
    //    }
    //}
}
