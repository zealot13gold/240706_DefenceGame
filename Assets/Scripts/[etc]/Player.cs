using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * [직렬화]
 * 역할
 - 플레이어별 자원, 인구수 관리

 * 입력값
 - 플레이어 타입(인간/AI)
 - Controller 클래스 : 플레이어 타입에 따라 변동
 - 플레이어별 빌딩 리스트 : 건설/파괴로 인한 빌딩 개수를 체크하기 위함 
 - 플레이어별 유닛 리스트 : 생성/사망으로 인한 유닛 인구수를 체크하기 위함
 - 플레이어 종족
 - 플레이어 자원(가스, 미네랄, 인구수)
 - 플레이어 초기 시작 위치

 * Awake()
 - 인간/AI 선택(switch(플레이어 타입) -> new HumanController/AIController)

 * FixedUpdate()
 - 건물 건설/파괴로 인한 빌딩 개수 체크
 - 유닛 생성/사망으로 인한 유닛 인구수 체크
 - behaviour 클래스로 자원 상황을 실시간으로 전달
 */

public class Player:MonoBehaviour                                                 // 플레이어별 정보(자원/인구 상황 등) 저장
{
    public enum PlayerType { Human, AI };
    //public enum Tribe { A, B, C };                        // 향후 종족 추가

    // 종족
    //public IdleState idleState;
    //public MoveState moveState;
    //public AttackState attackState;

    // 플레이어별 자원 상황
    public struct _Resources
    {
        public int mineral;
        public int gas;
        public int supply;
    }

    // 플레이어별로 건설된 건물 개수, 생성된 유닛 인구수 표현
    List<Building> buildingList = new List<Building>();             // 리스트에 현재 존재하는 건물의 목록 저장
    List<PlayerUnit> unitSupplyList = new List<PlayerUnit>();                   // 리스트에 현재 존재하는 유닛의 목록 저장

    [HideInInspector] public int buildingNum;                       // 플레이어별 건물의 개수를 체크

    //protected Player player;                                        // 플레이어 클래스

    public PlayerType playerType;                                   // 플레이어 타입(인간/AI)
    public Tribe playerTribe;                                       // 플레이어 종족

    public _Resources resources;                                    // 플레이어 자원(가스, 미네랄, 인구수)
    protected int usableSupply;                                     // 현재 사용 가능한 최대 인구수(보급소 건설 시 증가)
    protected int maxSupply = 200;                                  // 최대 인구수(건물을 지어도 증가하지 않음)

    public Transform startingPoint;                                 // 플레이어 초기 시작 위치
    public GameObject baseBuilding;                                 // 플레이어 baseBuilding

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {

    }
    protected virtual void FixedUpdate()
    {

    }


    // 건물이 건설/파괴될 경우  -> building 객체 각각에서 수행

    // 유닛 생산/제거에 따라 유닛 인구수 계산 -> unit 객체 각각에서 수행
}
