using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{

    public float maxHP;
    [HideInInspector] public float currentHP;
    [HideInInspector] public bool IsDead;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHP = maxHP;
        IsDead = false;

        Debug.LogFormat("{0} 초기 체력: {1}", gameObject, currentHP);
    }

    protected virtual void FixedUpdate()
    {
        Test_Death();
        //Debug.LogFormat("{0} 체력: {1}/{2}", gameObject.name, currentHP, maxHP);
    }

    void Test_Death()
    {
        if (Input.GetKey(KeyCode.Delete))
        {
            OnDeath();
        }
    }

    public virtual void CalculateHP(float damage)                                   // 데미지 계산
    {
        currentHP -= damage;                                                // 현재 체력에 적의 공격력을 뺀 값
        //Debug.LogFormat("{0}이 공격받고 있음, 체력: {1}/{2}", gameObject.name, currentHP, maxHP);
    }

    protected virtual void OnDeath()
    {
       
    }
}
