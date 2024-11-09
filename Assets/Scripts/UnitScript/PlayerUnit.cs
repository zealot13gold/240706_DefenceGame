using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/*
 * 역할
 - 유닛의 상태 표시, 사망 처리

 * 입력값
 - 역할(일꾼/공격 유닛)
 - 사망 모션/효과음
 - 이동-목표 지점
 - 유닛 생산 시 필요한 자원/인구 수
 - 유닛 HP/MP
 - 유닛 Max HP/MP -> 상수화
 - 유닛 시야
 - 공격력/방어력
 - 공격속도
 - 사거리

 * Awake()
 - new Unit (일반) / new Worker (일꾼)

 * OnEnable()
 - 유닛 HP = 유닛 Max HP

 * FixedUpdate()
 - 데미지 계산
 - 사망
 - 자원 채굴(상속되는 일꾼 클래스에서 작성)
 - 메뉴 인터페이스(유닛 HP 표현, 메뉴 행렬)

 * 함수
 - 데미지 계산 : 상대 유닛이 공격 시 공격값 입력 -> 유닛의 방어력을 적용한 연산 수행(공격력-방어력) -> 연산 결과를 유닛 HP에 적용
 - 사망 : 유닛 HP를 입력값으로 함 -> 유닛 HP가 0 이하인 경우 사망처리(object 삭제, 사망 모션 출력 등)
 - 자원 채굴 : 목표 자원을 입력값으로 함 -> 일꾼 이동(A*) -> 채굴 시간 -> 자원 조각 획득 -> 가까운 baseBuilding으로 이동 -> 자원 사라짐, 자원 연산 수행
 - 메뉴 인터페이스 : 유닛이 선택된 상태인지 확인 -> 메뉴 인터페이스 교체
 */
public class PlayerUnit : MonoBehaviour
{
    // 선택 여부
    [HideInInspector]public bool isSelected;

    // 애니매이션
    public Animator unitAnim;

    // 이동
    protected PlayerUnitSM sm;
    public PlayerUnitState unitState;
    public Vector3 dest;                    // 오른쪽 클릭 시 목적지
    [HideInInspector] public bool forceMove;
    [HideInInspector] public bool isAttack;

    // 체력
    [HideInInspector] public Health unitHealth;

    // 유닛 가격
    public int price;

    // 쉐이더
    public MeshRenderer meshRenderer;
    protected Material unitShader;
    protected bool buffer;

    // 유닛 선택
    public GameObject selection;

    protected virtual void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = Instantiate(meshRenderer.sharedMaterial);
        unitShader = meshRenderer.material;

        sm = GetComponent<PlayerUnitSM>();
        unitHealth = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        isSelected = false;
        buffer = false;

        dest = transform.position;
    }

    protected virtual void FixedUpdate()
    {
        if (isSelected)                                     // 현재 선택된 유닛만 해당
        {            selection.SetActive(true);        }
        else
        {            selection.SetActive(false);        }
    }
}
