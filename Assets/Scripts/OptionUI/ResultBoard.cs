using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ResultBoard : MonoBehaviour
{
    public Text gameResultTextInBoard;                  // 스테이지 결과(결과창)
    public Text obtainCashInStageInBoard;               // 현재 스테이지에서 얻은 자금
    public Text obtainScoreInStageInBoard;              // 현재 스테이지에서 얻은 점수
    public Text producedPlayerUnitsInStageInBoard;      // 현재 스테이지에서 생산한 플레이어 유닛 수
    public Text killedPlayerToEnemyInStageInBoard;      // 현재 스테이지에서 적에게 사망한 플레이어 유닛 수
    public Text invadedEnemyUnitsInStageInBoard;        // 현재 스테이지에서 침입한 적 유닛 수
    public Text killedEnemyToPlayerInStageInBoard;      // 현재 스테이지에서 플레이어에게 사망한 적 유닛 수

    //InputAction input;

    private void OnEnable()
    {
        gameResultTextInBoard.text = StageManager.instance.stageTextMessage.text;
        obtainCashInStageInBoard.text = StageManager.instance.cashInStage.ToString();
        obtainScoreInStageInBoard.text = StageManager.instance.scoreInStage.ToString();

        producedPlayerUnitsInStageInBoard.text = StageManager.instance.producedPlayerUnitInStage.ToString();
        killedPlayerToEnemyInStageInBoard.text = StageManager.instance.killedPlayerUnitInStage.ToString();
        invadedEnemyUnitsInStageInBoard.text = StageManager.instance.invadedEnemyUnitInStage.ToString();
        killedEnemyToPlayerInStageInBoard.text = StageManager.instance.killedEnemyUnitInStage.ToString();
    }

    private void Update()
    {
        if (/*gameObject.activeSelf &&*/ Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
    }
}
