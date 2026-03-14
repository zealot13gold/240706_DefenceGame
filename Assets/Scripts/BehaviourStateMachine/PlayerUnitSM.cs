using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerUnitSM : MonoBehaviour
{
    public IState idleState;
    public IState moveState;
    public IState attackState;
    public IState moveToAttackState;
    public IState deadState;

    IState currentState = null;

    [HideInInspector] public Collider[] enemies;
    [HideInInspector] public GameObject targetEnemy;

    [Header("플레이어 유닛 기능")]
    [Header("적 인식")]
    [Tooltip("적 레이어")] public LayerMask enemyLayerMask;
    [Tooltip("플레이어의 적 인식 범위")] public float viewRange;

    [Header("공격")]
    [Tooltip("플레이어의 공격 범위")] public float attackRange;
    [Tooltip("플레이어의 공격 간격")] public float attackDelayTime;
    [Tooltip("플레이어의 공격력")] public float attackDemage;

    [Header("이동")]
    [Tooltip("플레이어 NavMesh")] public NavMeshAgent navMesh;
    [Tooltip("플레이어 이동 속도")] public float moveSpeed;                                     // 유닛의 이동속도

    [Header("체력")]
    [Tooltip("플레이어 체력")] public PlayerHealth health;

    [Header("플레이어 행동 애니메이션")]
    [Tooltip("플레이어 애니메이션")] public Animator anim;

    [Header("플레이어 선택 여부")]
    [Tooltip("선택 오브젝트")] public GameObject selectObj;
    [Tooltip("플레이어 체릭바")] public Slider hpSlider;


    public Vector3 dest;                                        // 유닛 강제 이동, 목적지는 마우스 우클릭 시 업데이트됨

    //[HideInInspector] public Rigidbody unitRigidbody;           // 유닛의 rigidbody
   

    // 이동, 공격 여부
    public enum unitStateList
    {
        idle,
        attackMove,
        forceMove,
        attack,
        death
    }
    unitStateList unitState;
    [HideInInspector] public bool isForceMove;
    [HideInInspector] public bool isAttackMove;
    [HideInInspector] public bool isFire;

    // 음향효과
    //public AudioSource gunFireSound;
    //public AudioSource playerUnitVoice;
    //public AudioClip playerSelectedVoice;
    //public AudioClip playerForcedMoveVoice;
    //public AudioClip playerDiscoverEnemyVoice;
    //public AudioClip playerAttackVoice;
    //public AudioClip playerDeadVoice;

    // 적 정보
    [HideInInspector] public float targetEnemyHealth;
    [HideInInspector] public bool targetEnemyIsDead;

    // 코루틴
    Coroutine coroutine;

    void Awake()
    {
        // 아래 두줄은 플레이어/적 사망 state 작성 후 사망 state로 이동
        targetEnemy = null;
        //dest = PlayerUnitPooling.Instance.playerUnitSpawnPoint.position;

        idleState = new PlayerIdleState(gameObject);
        moveState = new PlayerMoveState(gameObject);
        attackState = new PlayerAttackState(gameObject);
        moveToAttackState = new PlayerMoveToAttackState(gameObject);
        deadState = new PlayerDeadState(gameObject);
    }

    private void OnEnable()
    {
        Debug.LogFormat("PlayerUnitSM: {0} 활성화", gameObject.name);
        Init();
    }

    void OnDisable()
    {
        Debug.LogFormat("PlayerUnitSM: {0} 비활성화", gameObject.name);
    }

    void Init()
    {
        isForceMove = false;
        isAttackMove = false;
        isFire = false;

        unitState = unitStateList.idle;
        currentState = null;
        UnitStateChange(idleState);
    }

    public void UnitStateChange(IState state)
    {
        Debug.LogFormat("PlayerSM: {0} 현재 상태: {1}", gameObject.name, currentState);
        if (state != currentState)
        {
            if (currentState != null)
            {
                currentState.Exit();
                if (coroutine != null) StopCoroutine(coroutine);
            }
            state.Enter();
            currentState = state;
            coroutine = StartCoroutine(StateUpdate(currentState));
        }
    }

    public void UnitSelect(bool select)
    {
        if(select)
        {
            selectObj.SetActive(true);
        }
        else
        {
            selectObj.SetActive(false);
        }
    }

    public void ForceMove()                                                                     // 강제이동, 플레이어 유닛이 지정된 목적지에 도달하였는지 확인, isMove 값을 변경
    {
        Debug.LogFormat("{0} 목적지: {1}", gameObject, dest);

        if(Mathf.Abs((dest- transform.position).magnitude)>1.5f)
        {
            UnitStateChange(idleState);
        }
    }

    void AttackMove()
    {
        if (Mathf.Abs((targetEnemy.transform.position - transform.position).magnitude) > attackRange)               // 적이 플레이어 유닛의 공격 사정거리보다 먼 곳에 위치하면
        {
            UnitStateChange(attackState);                                                            // 플레이어 유닛이 공격을 위해 이동
        }
    }

    public void Dead()
    {

        UnitStateChange(deadState);
    }

    IEnumerator StateUpdate(IState state)
    {
        while(true)
        {
            state.Update();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void FindEnemy()                                                                         // 적이 존재하는지 확인(idleState 상태에서 지속적으로 확인), isAttack 값을 변경
    {

        enemies = Physics.OverlapSphere(transform.position, viewRange, enemyLayerMask);             // 유닛의 현재 위치에서 시야(viewPoint) 내에 적이 존재하는지 확인

        Vector3 bufferEnemyPos;                                                                          // 적의 위치를 임시 저장
        float bufferEnemyDist = 10000f;                                                                  // 플레이어 유닛과 적 사이의 거리를 임시 저장

        for (int i = 0; i < enemies.Length; i++)
        {
            bufferEnemyPos = enemies[i].transform.position;                                              // 공격 대상의 위치를 임시저장

            if (bufferEnemyDist > Mathf.Abs((bufferEnemyPos - transform.position).magnitude))                    // 현재 위치로부터 가장 가까운 공격 대상을 찾음
            {
                targetEnemyHealth = targetEnemy.GetComponent<Health>().currentHP;
                if (targetEnemyHealth <= 0) continue;

                targetEnemy = enemies[i].gameObject;                                                    // 가까운 적의 오브젝트를 저장

                //targetEnemyIsDead = targetEnemy.GetComponent<Health>().isDead;
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
