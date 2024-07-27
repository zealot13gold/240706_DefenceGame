using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeadState : TurretState
{
    public TurretDeadState(GameObject unit) : base(unit) { }

    float delayTime;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        delayTime = 0;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        if (delayTime < 3f)
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
        unit.SetActive(false);
    }
}
