using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Building : Monobehaviour

 * 역할
    건물의 건설, 유닛 생산, 파괴 처리

 * 입력값
    역할(baseBuilding/일반 건물)                        -> 건물 별 상속, 디펜스 게임에서는 필요 없음
    건설 모션/효과음
    파괴 모션/효과음
    생산 가능 유닛 목록                              -> 디펜스 게임에서는 필요 없음, 삭제
    유닛 생산창(스택, 최대값이 정해진 배열로 구현) -> 디펜스 게임에서는 필요 없음, 삭제
    건물 생산 시 필요한 자원
    건물 생산 후 제공되는 인구 수(일반 건물은 0으로 함)
    건물 방어력

 * Awake()
    new baseBuilding/일반 건물

 * FixedUpdate()
    건설
    유닛 생산
    건물 데미지 계산
    건물 파괴

 * 함수
    건설 : 건설 진행상황에 따라 건물의 형태가 변함
    유닛 생산 : 인터페이스 메뉴를 통해 선택된 유닛을 입력값으로 함 -> 스택에 저장 -> 생산 시간이 다 되면 스택의 유닛 삭제
    건물 데미지 계산 : 상대 유닛의 공격력을 입력값으로 함 -> 건물 방어력을 적용한 연산 수행 -> 연산 결과를 건물 HP에 적용
    건물 파괴 : 건물 HP를 입력값으로 함 -> 건물 HP가 0 이하일 경우 건물 파괴 애니매이션/효과음 출력 -> 건물 object 삭제
 */

public class Building : MonoBehaviour
{
    public int buildCost;
    public int supply;
    public GameObject buildingPrefab;
    public GameObject OnBuild;
    protected GameObject buildingObject;
    public Health buildingHealth;
    public int buildingArmor;


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
