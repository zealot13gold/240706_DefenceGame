using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marine : PlayerUnit
{
    public Animator marineAnim;

    protected override void Awake()
    {
        base.Awake();

        marineAnim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        Debug.LogFormat("{0} 애니메이션 실행", gameObject.name);
        if (unitSM.currentState == unitSM.moveState)
        {
            marineAnim.SetTrigger("Walk");
            Debug.LogFormat("{0} {1} 애니메이션 발동", gameObject.name, unitSM.currentState);
        }
    }

}
