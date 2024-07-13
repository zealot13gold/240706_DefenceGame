using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : Health
{
    // 이펙트
    public ParticleSystem shotEffect;

    // HP 슬라이더
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
    //    gameObject.SetActive(false);
    //}
}
