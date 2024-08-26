using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerUnitState
{
    public PlayerDeadState(GameObject unit) : base(unit) { }

    float delayTime;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        sm.playerUnitVoice.clip = sm.playerDeadVoice;
        sm.playerUnitVoice.Play();

        sm.anim.SetBool("Dead", true);

        unit.GetComponent<BoxCollider>().enabled = false;

        delayTime = 0;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (delayTime < 10f)
        {
            delayTime += Time.deltaTime;
        }
        else
        {
            OnStateExit();
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        //unit.SetActive(false);
        PlayerUnitPooling.Instance.PickUpPlayerUnit(unit);
    }
}
