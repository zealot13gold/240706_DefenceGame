using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : IState
{
    PlayerUnitSM sm;
    PlayerHealth health;
    GameObject unit;
    public PlayerDeadState(GameObject unit)
    {
        sm = unit.GetComponent<PlayerUnitSM>();
        health = unit.GetComponent<PlayerHealth>();
        this.unit = unit;
    }

    float delayTime;

    public void Enter()
    {
        //sm.playerUnitVoice.clip = sm.playerDeadVoice;
        //sm.playerUnitVoice.Play();
        Debug.LogFormat("PlayerDeadState: {0} 사망", unit.name);

        sm.anim.SetBool("Dead", true);

        unit.GetComponent<BoxCollider>().enabled = false;

        delayTime = 0;
    }

    public void Update()
    {
        //if (delayTime < 10f)
        //{
        //    delayTime += Time.deltaTime;
        //}
        //else
        //{
        //    OnStateExit();
        //}
    }

    public void Exit()
    {
        PoolManager.instance.assaultPool.ReturnObject(unit);
    }
}
