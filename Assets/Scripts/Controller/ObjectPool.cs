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
    }

    public void CreateObject()
    {
        GameObject obj = GameObject.Instantiate(pref);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
    }

    public GameObject SetObject(Vector3 pos, Quaternion rot)
    {
        if (objectPool.Count <= 0) CreateObject();
        
        GameObject obj = objectPool.Dequeue();
        obj.SetActive(true);
        usingObj.Add(obj);

        obj.transform.position = pos;
        obj.transform.rotation = rot;

        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
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
