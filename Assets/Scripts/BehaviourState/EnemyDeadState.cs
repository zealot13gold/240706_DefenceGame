using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyUnitState
{
    public EnemyDeadState(GameObject unit) : base(unit) { }
    float delayTime=10f;
    float time;

    // Start is called before the first frame update
    public override void OnStateEnter()
    {
        sm.enemyAudioSource.clip = sm.enemyDeadVoice;
        sm.enemyAudioSource.Play();

        sm.anim.SetBool("Death", true);

        unit.GetComponent<BoxCollider>().enabled = false;

        time = 0f;
    }

    // Update is called once per frame
    public override void OnStateUpdate()
    {
        if(time< delayTime)
        {
            time += Time.deltaTime;
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
        EnemyUnitPooling.Instance.PickUpEnemy(unit);
    }
}
