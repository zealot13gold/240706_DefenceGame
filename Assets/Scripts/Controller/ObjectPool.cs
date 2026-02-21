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
        GameObject obj = GameObject.Instantiate(pref, Vector3.zero, Quaternion.Euler(0, 0, 0));
        obj.SetActive(false);
        objectPool.Enqueue(obj);
        obj.transform.parent = PoolManager.instance.gameObject.transform;

        Debug.LogFormat("ObjectPool: {0} 생성 완료, 현재 큐 내부의 {0} 갯수: {1}", obj.name, objectPool.Count);
    }

    public GameObject SetObject(Vector3 pos, Quaternion rot)
    {
        Debug.LogFormat("ObjectPool: {0} 위치에 오브젝트 출현", pos);
        if (objectPool.Count <= 0) CreateObject();

        GameObject obj = objectPool.Dequeue();
        obj.transform.parent = null;
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        obj.SetActive(true);
        usingObj.Add(obj);


        Debug.LogFormat("ObjectPool: {0} 필드에 생성, 현재 큐 내부의 {0} 갯수: {1}", obj.name, objectPool.Count);

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = PoolManager.instance.gameObject.transform;
        objectPool.Enqueue(obj);
        usingObj.Remove(obj);
    }

    public void ReturnAll()
    {
        foreach(GameObject obj in usingObj)
        {
            ReturnObject(obj);
        }
    }
}
