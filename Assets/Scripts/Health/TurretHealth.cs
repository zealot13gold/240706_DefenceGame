using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHealth : Health
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void CalculateHP(float damage)
    {
        base.CalculateHP(damage);
    }
}
