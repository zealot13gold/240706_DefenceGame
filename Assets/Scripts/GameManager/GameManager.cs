using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 역할
 - 준비: 플레이어 생성
 - 스테이지 시작 시 플레이어/적 유닛 소환
 - 스테이지 진행: 플레이어/적 유닛이 0이 될때까지 진행
 - 게임 종료 판정: 플레이어 수가 0이면 게임 종료

 * 입력값
 - 플레이어 유닛 목록
 - 자금
 - 점수(플레이어가 죽인 적)
 - 초기 플레이어 유닛 수
 - 현재 맵에 존재 가능한 적의 수 -> 스테이지 번호에 따라 증가

 * Awake()
 - 

 * FixedUpdate()
 - 게임 시작시: 바로 준비 상태로 돌입
 - 준비 시간이 지난 후: 스테이지 진행 상태로 돌입
 - 스테이지 종료 후: 승패 판별 -> 승리 시 스테이지 번호를 1 올리고 준비 상태로 돌입, 패배 시 게임 종료 메시지 출력 후 게임 종료
 */



public class GameManager : StateMachine
{
    private static GameManager gameInstance;

    public StagePrepareState stagePrepare;
    public StageDoingState stageDoing;
    public StageResultState stageResult;

    // 스테이지 관리
    [HideInInspector] public int stageNumber;       // 스테이지 번호
    public float prepareTime;     // 스테이지 준비 시간

    // UI
    public Text stageTextMessage;                          // 스테이지 시작/종료 시 텍스트
    public Text remainTimeMessage;                   // 남은 시간 표시
    public Text remainPlayerMessage;                    // 남은 플레이어 유닛 표시
    public Text numOfEnemiesMessage;                    // 남은 적 유닛 표시
    public Text stageTextUI;                           // 상단에 스테이지 번호 표시
    public Text cashUI;                        // 자원 표시
    public Text scoreUI;                           // 점수 표시
    public GameObject buttons;                  // 버튼 모음
    public Image resultBoard;                   // 결과창
    public Text gameResultTextInBoard;          // 스테이지 결과(결과창)
    public Text obtainCashInStageInBoard;       // 현재 스테이지에서 얻은 자금
    public Text obtainScoreInStageInBoard;      // 현재 스테이지에서 얻은 점수
    public Text producedPlayerUnitsInStageInBoard;  // 현재 스테이지에서 생산한 플레이어 유닛 수
    public Text killedPlayerToEnemyInStageInBoard;  // 현재 스테이지에서 적에게 사망한 플레이어 유닛 수
    public Text invadedEnemyUnitsInStageInBoard;    // 현재 스테이지에서 침입한 적 유닛 수
    public Text killedEnemyToPlayerInStageInBoard;  // 현재 스테이지에서 플레이어에게 사망한 적 유닛 수

    // 플레이어 관리
    public PlayerManager playerManager;
    // 적 관리
    public EnemyManager enemyManager;

    // 점수, 자금
    [HideInInspector] public int score;                         // 점수
    public int cash;                                           // 초기 자본금

    [HideInInspector] public int scoreInStage;                  // 현재 스테이지에서 획득한 점수
    [HideInInspector] public int cashInStage;                  // 현재 스테이지에서 획득한 자금

    // 플레이어 유닛, 장애물 가격
    public int soliderCost;
    public int barrierCost;

    // 스테이지에서 생산/제거된 플레이어, 적 유닛 수
    public int producedPlayerUnitInStage;
    public int killedPlayerUnitInStage;
    public int invadedEnemyUnitInStage;
    public int killedEnemyUnitInStage;

    public static GameManager Instance
    {
        get
        {
            if (gameInstance == null) 
            {
                gameInstance = new GameManager();
            }
            return gameInstance;
        }
    }
    
    protected override void Awake()
    {

        if(gameInstance == null)
        {
            gameInstance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        enemyManager = new EnemyManager();
        playerManager = new PlayerManager();

        stagePrepare = new StagePrepareState(gameObject);
        stageDoing = new StageDoingState(gameObject);
        stageResult = new StageResultState(gameObject);
    }

    protected override void Start()
    {
        score = 0;
        stageNumber = 0;
        stageTextMessage.gameObject.SetActive(false);

        ChangeState(stagePrepare);
;    }

    protected override void FixedUpdate()
    {
        currentState.OnStateUpdate();
    }
}
