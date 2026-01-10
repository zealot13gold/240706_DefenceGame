using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierHealth : Health 
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void CalculateHP(float damage)
    {
        base.CalculateHP(damage);

        if(currentHP<=0)
        {
            OnDeath();
        }
    }

    protected override void OnDeath()
    {
        BarrierPooling.Instance.PickUpBarrier(gameObject);
    }
}
