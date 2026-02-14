using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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



public class StageManager : MonoBehaviour
{
    public static StageManager instance = null;

    public IState stagePrepare;
    public IState stagePlay;
    public IState stageResult;

    public IState currentStageState = null;

    // 스테이지 관리
    [HideInInspector] public int stageNumber;               // 스테이지 번호
    public int prepareTime;                                 // 스테이지 준비 시간
    [HideInInspector] public Coroutine stageRoutine;

    public enum stageStepList
    {
        stagePrepare,
        stagePlay,
        stageResult
    }
    public stageStepList stageStep;
    public event Action<stageStepList> stageStepChanged;

    // UI
    [Header("스테이지 UI")]
    [Header("스테이지 시작 시 메시지")]
    [Tooltip("")] public Text stageNumTitle;                          // 스테이지 번호 표시
    [Tooltip("")] public Text stageTextMessage;                       // 스테이지 시작/종료 시 텍스트
    [Header("스테이지 준비 메시지")]
    [Tooltip("")] public Image displayRemainTime;                     // 남은 시간 표시
    [Tooltip("")] public Text remainTimeMessage;                      // 남은 시간 텍스트
    [Header("스테이지 진행 정보")]
    [Tooltip("남은 플레이어 유닛(상태창)")] public Text remainPlayerMessage;                    // 남은 플레이어 유닛 표시
    [Tooltip("남은 적 유닛(상태창)")] public Text numOfEnemiesMessage;                    // 남은 적 유닛 표시
    [Tooltip("스테이지 번호(상단 표시)")] public Text stageTextUI;                            // 상단에 스테이지 번호 표시
    [Tooltip("소지 자금 표시")] public Text cashUI;                                 // 자원 표시
    [Tooltip("획득 점수 표시")] public Text scoreUI;                                // 점수 표시
    [Header("버튼 목록")]
    [Tooltip("버튼 모음")] public GameObject buttons;                          // 버튼 모음
    [Header("결과창")]
    [Tooltip("결과창")] public Image resultBoard;                           // 결과창

    


    //// 플레이어 관리
    //public PlayerManager playerManager;
    //// 적 관리
    //public EnemyManager enemyManager;

    // 점수, 자금
    [HideInInspector] public int score;                         // 점수
    public int cash;                                           // 초기 자본금

    [HideInInspector] public int scoreInStage;                  // 현재 스테이지에서 획득한 점수
    [HideInInspector] public int cashInStage;                  // 현재 스테이지에서 획득한 자금

    // 플레이어 유닛, 장애물 가격
    public int soliderCost;
    public int barrierCost;

    // 현재 스테이지에서 등장하는 적의 수 -> 해당 수 만큼 풀에서 생산
    public int invadedEnemyUnitInStage;

    // 스테이지에서 생산/제거된 플레이어, 적 유닛 수
    public int producedPlayerUnitInStage;
    public int killedPlayerUnitInStage;
    
    public int killedEnemyUnitInStage;

    // 승리, 패배 여부
    bool isGameOver;
    bool isWin;

    void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;

        //enemyManager = new EnemyManager();
        //playerManager = new PlayerManager();

        stagePrepare = new StagePrepareState(gameObject);
        stagePlay = new StagePlayState(gameObject);
        stageResult = new StageResultState(gameObject);

        GameInit();
        GameStageChange(stagePrepare);
    }

    void GameInit()
    {
        // 게임 환경 초기화
        score = 0;
        
        stageNumber = 0;
        stageTextMessage.gameObject.SetActive(false);

        StageInit();
    }

    void StageInit()
    {
        // 이전 스테이지의 정보 초기화
        scoreInStage = 0;          // 이전 스테이지에서 획득한 점수 초기화
        cashInStage = 0;           // 이전 스테이지에서 획득한 자금 초기화

        producedPlayerUnitInStage = 0; // 이전 스테이지에서 생산된 플레이어 유닛 수 초기화
        killedPlayerUnitInStage = 0;   // 이전 스테이지에서 사망한 플레이어 유닛 수 초기화
        invadedEnemyUnitInStage = 0;   // 이전 스테이지에서 출현한 적 유닛 수 초기화
        killedEnemyUnitInStage = 0;    // 이전 스테이지에서 사망한 적 유닛 수 초기화

        // 스테이지 승패 초기화
        isGameOver = false;
        isWin = false;
    }

    void StageSet()
    {
        // 해당 스테이지 설정(스테이지 번호, 출현한 적의 수)
        stageNumber++;
        invadedEnemyUnitInStage = stageNumber * 10;
    }

    public void GameStageChange(IState state)
    {
        if (state != currentStageState)
        {
            if(currentStageState!=null) currentStageState.Exit();
            state.Enter();
            currentStageState = state;
        }
    }

    public void GameStagePrepare()
    {
        StageInit();
        StageSet();
        // 게임 시작 - 스테이지 준비 단계
        stageRoutine = StartCoroutine("PrepareRoutine");
    }

    public void GameStagePlay()
    {
        // 스테이지 시작 메시지 출력 -> 몇 초 후 사라짐
        stageRoutine = StartCoroutine("DisplayStageBeginMessage");

        // 적의 수 표시
        //numOfEnemiesMessage.gameObject.SetActive(true);

        // 적 유닛 생산
        //if (!isSpawnEnemy)
        //{
        //    SpawnEnemies();
        //    isSpawnEnemy = true;
        //}

        //// 플레이어/적 유닛 수 체크 -> 둘 중 하나가 0이 되면 스테이지 결과 상태로 이동
        //remainPlayerUnit = CheckRemainPlayer();
        //remainEnemyUnit = CheckRemainEnemy();

        // 디스플레이의 점수, 남은 적의 수 갱신
        //DisplayStageUI();

        //if (remainPlayerUnit <= 0 || remainEnemyUnit <= 0)
        //{
        //    //StageManager.instance.ChangeState(StageManager.instance.stageResult);
        //}
    }

    public void GameStageResult()
    {
        stageRoutine = StartCoroutine("DisplayStageEndMessage");
    }

    IEnumerator PrepareRoutine()
    {
        int remainTime = prepareTime;
        while (remainTime > 0)
        {
            remainTimeMessage.text = "Prepare Time: " + remainTime.ToString();

            yield return new WaitForSeconds(1.0f);
            remainTime--;
        }
        // 스테이지 준비 종료 -> 스테이지 시작 단계 실행
        GameStageChange(stagePlay);
    }

    IEnumerator DisplayStageBeginMessage()
    {
        Debug.LogFormat("스테이지 {0} 시작 메시지 출력", stageNumber);
        stageTextMessage.text = "Stage " + stageNumber + " Start!!!";

        stageTextMessage.gameObject.SetActive(true);

        // 준비시간 표시 메시지를 아래와 같이 변경
        remainTimeMessage.text = "Enemies are Coming!!!";
        remainTimeMessage.color = Color.red;
       
        yield return new WaitForSeconds(2.0f);
        stageTextMessage.gameObject.SetActive(false);

        yield return new WaitForSeconds(3.0f);
        displayRemainTime.gameObject.SetActive(false);         // 스테이지 메시지 숨기기

        stageRoutine = StartCoroutine("ProduceEnemies");
    }

    IEnumerator ProduceEnemies()
    {
        for(int i=0; i<invadedEnemyUnitInStage; i++)
        {
            // 오브젝트 풀로부터 적 유닛 생산

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 스테이지 시작 시 UI

    public void DisplayStageUI()
    {
        // StageManager가 EnemyManager를 구독 -> 적 유닛 사망 이벤트 발생할 때마다 함수 실행
        //numOfEnemiesMessage.text = enemyManager.numberOfEnemyUnit.ToString() + " / " + invadedEnemyUnitInStage.ToString();
        scoreUI.text = "Score: " + score.ToString();
    }

    public void PlayerDefeated()
    {
        isGameOver = true;
        GameStageChange(stageResult);
    }

    public void PlayerWin()
    {
        isGameOver = true;
        isWin = true;
        GameStageChange(stageResult);
    }

    IEnumerator DisplayStageEndMessage()
    {
        if (isGameOver)
        {
            if (isWin)
            {
                stageTextMessage.text = "Stage " + stageNumber + " Clear!!";
            }
            else
            {
                stageTextMessage.text = "Stage " + stageNumber + " Defeated";
            }

            stageTextMessage.gameObject.SetActive(true);

            yield return new WaitForSeconds(2f);

            stageTextMessage.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.5f);
            resultBoard.gameObject.SetActive(true);
        }
    }

    //public int NumberOfEnemiesInStage(int stageNumber)
    //{
    //    //Debug.LogFormat("현재 스테이지 번호: {0}", stageNumber);
    //    int enemies;

    //    // 스테이지에 따라 적의 수 변경
    //    enemies = stageNumber * 10;

    //    return enemies;
    //}
}
