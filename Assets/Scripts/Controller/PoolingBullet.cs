using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingBullet : MonoBehaviour
{
    /*[HideInInspector]*/
    public GameObject m_BulletPrefab;
    Queue<GameObject> m_BulletPool = new Queue<GameObject>();
    //private PoolingBullet() { }                                         // 생성자를 비공개로 설정
    private static PoolingBullet m_Instance = null;
    //[HideInInspector] public int m_MaxNumberOfBullet;
    //private bool m_AbleMakeBullet = true;
    GameObject m_Bullet;


    public static PoolingBullet Instance
    {
        get                                             // 외부로 값을 전달
        {
            Debug.LogFormat("Instance 실행");
            if (null == m_Instance)
            {
                return null;
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);                          // 다른 씬으로 넘어가도 사라지지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.LogFormat("현재 큐 내부의 실탄 개수 : {0}", m_BulletPool.Count);
    }
    public void CreateObject()
    {
        //Debug.LogFormat("실탄 발사 위치 : {0}", transform);
        m_Bullet = Instantiate(m_BulletPrefab);
        m_Bullet.SetActive(false);
        m_BulletPool.Enqueue(m_Bullet);
        //m_Bullet.transform.SetParent(transform);

        //Debug.LogFormat("실탄 생성, 실탄 개수 : {0}", m_BulletPool.Count);

    }
    public GameObject GetObject()
    {
        //GameObject m_Bullet;

        //while (true)
        //{
        Debug.LogFormat("Pooling :: GetObject() : 현재 큐 내부의 실탄 개수 : {0}", m_BulletPool.Count);
        if (m_BulletPool.Count <= 0)
        {
            Debug.Log("Pooling :: GetObject() : 실탄 생성");
            CreateObject();
        }

        //else
        //{
        m_Bullet = m_BulletPool.Dequeue();
        m_Bullet.transform.SetParent(null);
        m_Bullet.gameObject.SetActive(true);

        return m_Bullet;
        //}
        //}
    }

    public void ReturnObjectToQueue(GameObject Obj)
    {
        Debug.LogFormat("Pooling : 실탄 삭제 시작");
        Obj.gameObject.SetActive(false);
        Obj.transform.SetParent(Instance.transform);
        m_BulletPool.Enqueue(Obj);
        Debug.LogFormat("Pooling :: ReturnObjectToQueue() :  실탄이 큐 안으로 이동, 실탄 개수 : {0}", m_BulletPool.Count);
    }
}
