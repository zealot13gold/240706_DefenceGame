using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    // 이펙트
    public ParticleSystem shotEffect;

    // 체력바
    public Slider healthSlider;

    protected override void Start()
    {
        base.Start();

        shotEffect.Stop();
        shotEffect.gameObject.SetActive(false);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        healthSlider.value = currentHP;
    }

    public override void CalculateHP(float damage)
    {
        base.CalculateHP(damage);

        if (currentHP > 0)
        {
            shotEffect.gameObject.SetActive(true);
            shotEffect.Play();
        }
        //else
        //{
        //    OnDeath();
        //}
    }

    //protected override void OnDeath()
    //{
    //    PlayerUnitPooling.Instance.PickUpPlayerUnit(gameObject);
    //}
}
