using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : IState
{
    EnemyUnitSM sm;
    EnemyHealth health;
    GameObject unit;
    public EnemyDeadState(GameObject unit)  
    {
        sm = unit.GetComponent<EnemyUnitSM>();
        health = unit.GetComponent<EnemyHealth>();
        this.unit = unit;
    }

    float delayTime=10f;
    float time;

    // Start is called before the first frame update
    public  void Enter()
    {
        //sm.enemyAudioSource.clip = sm.enemyDeadVoice;
        //sm.enemyAudioSource.Play();

        sm.anim.SetBool("Death", true);

        unit.GetComponent<BoxCollider>().enabled = false;

        time = 0f;
    }

    // Update is called once per frame
    public void Update()
    {
        if(time< delayTime)
        {
            time += Time.deltaTime;
        }
        else
        {
            Exit();
        }
    }

    public void Exit()
    {
        //base.OnStateExit();
        //unit.SetActive(false);
        //PoolManager.instance.PickUpEnemy(unit);
    }
}
