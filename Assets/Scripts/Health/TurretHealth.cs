using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretHealth : Health
{

    // HP 슬라이더
    public Slider healthSlider;

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        healthSlider.value = currentHP;
    }

    public override void CalculateHP(float damage)
    {
        base.CalculateHP(damage);
    }
}
