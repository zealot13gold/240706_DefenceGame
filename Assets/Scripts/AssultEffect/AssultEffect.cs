using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultEffect : MonoBehaviour
{
    public ParticleSystem gunFireEffect;

    public PlayerUnitSM sm;

    

    //public ParticleSystem shotEffect;

    private void Start()
    {
        gunFireEffect.Stop();
        gunFireEffect.gameObject.SetActive(false);
    }

    void Update()
    {
        //Debug.LogFormat("{0} 공격 여부 : {1}", sm.gameObject.name, sm.isFire);
        if (sm.isFire)
        {
            gunFireEffect.gameObject.SetActive(true);
            gunFireEffect.Play();
            //Debug.LogFormat("{0} 공격: {1}", sm.gameObject.name, sm.isFire);
        }
        else
        {
            gunFireEffect.Stop();
            gunFireEffect.gameObject.SetActive(false);
        }
    }
}
