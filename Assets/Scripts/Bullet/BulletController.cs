using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public LayerMask m_Player;
    public float m_DamageRange;
    public float m_Damage;

    public PoolingBullet Instance;                  // 실탄 풀링

    private void Awake()
    {
        Instance = PoolingBullet.Instance;
    }

    private void OnEnable()
    {
        //Destroy(gameObject, 50f*Time.deltaTime);            // 사격 대상이 collider가 없는 경우 일정 시간 후에 자동으로 삭제
        Invoke("EnQueue", 0.5f);
        //Debug.LogFormat("실탄은 0.5초 후에 자동 삭제");
        //StartCoroutine("EnqueueCo");
    }

    //private void Update()
    //{

    //    EnQueue();

    //}

    void EnQueue()
    {
        if (gameObject.activeSelf)
        {
            //if (gameObject.transform.position.x < -100 || gameObject.transform.position.x > 100 || gameObject.transform.position.z < -100 || gameObject.transform.position.z > 100 || gameObject.transform.position.y > 50)
            //{
            Instance.ReturnObjectToQueue(this.gameObject);
            //Debug.LogFormat("Bullet : 실탄 자동 삭제");
            //}
        }
    }

    void OnTriggerEnter(Collider other)
    {

        Collider[] enimies = Physics.OverlapSphere(transform.position, m_DamageRange, m_Player);        // 실탄이 trigger와 부딪힌 지점으로부터 m_DamageRange 범위의 모든 m_Player 탐색
        for (int i = 0; i < enimies.Length; i++)
        {
            Rigidbody targetRigidbody = enimies[i].GetComponent<Rigidbody>();                       // 탐색된 적들의 rigidbody

            if (!targetRigidbody) continue;

            Health targetHealth = targetRigidbody.GetComponent<Health>();               // 적들의 PlayerHealth

            if (!targetHealth) continue;

            float damage = DamageRange(targetRigidbody.position, transform.position);

            targetHealth.CalculateHP(damage);

            //Debug.Log(targetHealth.name + " Current HP : " + targetHealth.m_CurrentHP);
        }

        //gameObject.SetActive(false);
        //Destroy(gameObject);
        Instance.ReturnObjectToQueue(this.gameObject);
    }

    float DamageRange(Vector3 enemy, Vector3 bullet)
    {
        float damage = 0;

        Vector3 bullet_enemy_vector = enemy - bullet;

        float bullet_enemy_distance = Mathf.Abs(Vector3.Magnitude(bullet_enemy_vector));

        if (bullet_enemy_distance <= m_Damage)
            damage = m_Damage * (m_DamageRange - bullet_enemy_distance) / m_DamageRange;



        return damage;
    }
}
