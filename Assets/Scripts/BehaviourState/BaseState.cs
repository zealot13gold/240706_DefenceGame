using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/*
 * 역할
 - 유닛의 이동, 공격 시 행동

 * 입력값
 - 역할(일꾼/공격 유닛)
 - 이동 모션/효과음
 - 공격 모션/효과음
 - 이동-목표 지점
 - 공격속도
 - 사거리

 * Awake()
 - new Unit (일반) / new Worker (일꾼)

 * OnEnable()
 - 유닛 HP = 유닛 Max HP

 * FixedUpdate()
 - 이동
 - 공격
 - 자원 채굴(상속되는 일꾼 클래스에서 작성)
 - 메뉴 인터페이스(유닛 HP 표현, 메뉴 행렬)

 * 함수
 - 이동 : 목표 지점(월드 좌표)을 입력값으로 함 -> 유닛 이동(A*)
 - 공격 : 목표 지점(월드 좌표)을 입력값으로 함 -> 유닛 이동 -> 이동 도중 유닛 시야에 상대 유닛이 존재할 경우 해당 유닛 쪽으로 이동 -> 유닛 사망 시 다시 목표 지점으로 이동
 - 자원 채굴 : 목표 자원을 입력값으로 함 -> 일꾼 이동(A*) -> 채굴 시간 -> 자원 조각 획득 -> 가까운 baseBuilding으로 이동 -> 자원 사라짐, 자원 연산 수행
 */
public abstract class BaseState
{ 
    //protected StateMachine stateManager;
    public abstract void OnStateEnter();            // 행동 시작 시

    public abstract void OnStateUpdate();           // 상태 유지 중
    public abstract void OnStateExit();             // 상태 종료

}