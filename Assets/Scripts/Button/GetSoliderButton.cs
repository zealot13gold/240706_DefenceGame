using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSoliderButton : MonoBehaviour
{
   public void GetSolider()
    {
        if(GameManager.Instance.soliderCost <= GameManager.Instance.cash)
        {
            //Debug.LogFormat("Solider 구입, 남은 자금: {1}", gm.money - gm.soliderCost);
            GameManager.Instance.playerManager.CreatePlayerUnit();
            GameManager.Instance.cash-=GameManager.Instance.soliderCost;
            GameManager.Instance.producedPlayerUnitInStage++;                           // 현재 스테이지에서 생산된 플레이어 유닛 수 1 증가
            Debug.LogFormat("버튼 클릭, 플레이어 유닛 수: {0}", GameManager.Instance.playerManager.playerUnitList.Count);
            Debug.LogFormat("버튼 클릭, 생산된 플레이어 유닛 수: {0}", GameManager.Instance.producedPlayerUnitInStage);
        }
        else
        {
            Debug.LogFormat("자금 {0} 부족", GameManager.Instance.soliderCost-GameManager.Instance.cash);
        }
    }
}
