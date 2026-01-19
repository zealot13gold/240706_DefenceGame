using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealth : Health
{
    // 이벤트
    public event Action<bool> enemyManager; 

    // 이펙트
    public ParticleSystem shotEffect;

    // HP 슬라이더
    public Slider healthSlider;

    protected override void OnEnable()
    {
        base.OnEnable();

        enemyManager +=EnemyManager.instance.CheckUnit;
        enemyManager?.Invoke(false);

        shotEffect.Stop();
        shotEffect.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        enemyManager -= EnemyManager.instance.CheckUnit;
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
