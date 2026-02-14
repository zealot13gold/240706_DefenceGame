using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetSoliderButton : MonoBehaviour
{
    public GameObject cashUI;
    public GameObject costUI;

    public Transform unitTrans;
    Vector3 pos;
    Quaternion rot;

    public Text costText;
    int maxProd;
    string prodMessage;

    private void OnEnable()
    {
        pos = unitTrans.position;
        rot = unitTrans.rotation;
    }

    public void GetSolider()
    {
        if(StageManager.instance.soliderCost <= StageManager.instance.cash)
        {
            //Debug.LogFormat("Solider 구입, 남은 자금: {1}", gm.money - gm.soliderCost);
            //StageManager.instance.playerManager.CreatePlayerUnit();
            StageManager.instance.cash-=StageManager.instance.soliderCost;
            PoolManager.instance.SpawnObject("AssaultMan", pos, rot);
            StageManager.instance.producedPlayerUnitInStage++;                           // 현재 스테이지에서 생산된 플레이어 유닛 수 1 증가
            //Debug.LogFormat("버튼 클릭, 플레이어 유닛 수: {0}", StageManager.instance.playerManager.playerUnitList.Count);
            Debug.LogFormat("버튼 클릭, 생산된 플레이어 유닛 수: {0}", StageManager.instance.producedPlayerUnitInStage);
        }
        else
        {
            Debug.LogFormat("자금 {0} 부족", StageManager.instance.soliderCost-StageManager.instance.cash);
        }
    }

    public void MouseOnButton()
    {
        cashUI.SetActive(false);

        maxProd = StageManager.instance.cash / StageManager.instance.soliderCost;
        if (maxProd >=1)
        {
            prodMessage = "Able to get " + maxProd.ToString() + " soliders";
        }
        else
        {
            prodMessage = "Not enough cash";
            costText.color = Color.red;
        }
        costText.text = "Cash: " + StageManager.instance.cash.ToString()+ '\n' 
            + '\n' + "AssultMan: " + StageManager.instance.soliderCost.ToString() + " cash" + '\n' + prodMessage;

        costUI.SetActive(true);
    }

    public void MouseOffButton()
    {
        cashUI.SetActive(true);
        costUI.SetActive(false);
        costText.color = Color.green;
    }
}
