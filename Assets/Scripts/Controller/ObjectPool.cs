using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    GameObject pref;
    Queue<GameObject> objectPool;
    HashSet<GameObject> usingObj;

    public ObjectPool(GameObject obj)
    {
        pref = obj;
        objectPool = new Queue<GameObject>();
        usingObj = new HashSet<GameObject>();
        Debug.LogFormat("{0} 오브젝트 풀 생성", pref.name);
    }

    public void CreateObject()
    {
        Debug.LogFormat("{0} 오브젝트 생성 시작", pref.name);
        GameObject obj = GameObject.Instantiate(pref);
        Init(obj);

        Debug.LogFormat("ObjectPool: {0} 생성 완료, 현재 큐 내부의 {0} 갯수: {1}", obj.name, objectPool.Count);
    }

    public GameObject SetObject(Vector3 pos, Quaternion rot)
    {
        Debug.LogFormat("ObjectPool: {0} 위치에 오브젝트 출현", pos);
        if (objectPool.Count <= 0) CreateObject();

        GameObject obj = objectPool.Dequeue();

        obj.transform.parent = null;
        obj.SetActive(true);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        usingObj.Add(obj);

        Debug.LogFormat("ObjectPool: {0} 필드에 생성, {0} 상태: {1}", obj.name, obj.activeSelf);
        Debug.LogFormat("ObjectPool: {0} 필드에 생성, 현재 큐 내부의 {0} 갯수: {1}", obj.name, objectPool.Count);

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        Init(obj);
        usingObj.Remove(obj);
    }

    void Init(GameObject obj)
    {
        obj.SetActive(false);
        Health objHealth = obj.GetComponent<Health>();
        objHealth.currentHP = objHealth.maxHP;
        objectPool.Enqueue(obj);
        obj.name = pref.name;
        obj.transform.parent = PoolManager.instance.gameObject.transform;
    }

    public void ReturnAll()
    {
        foreach(GameObject obj in usingObj)
        {
            ReturnObject(obj);
        }
    }
}
