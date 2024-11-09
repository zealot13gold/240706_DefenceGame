using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierPooling : MonoBehaviour
{
    // 인스턴스
    private static BarrierPooling barrierPoolingInstance;

    // 장애물 생산
    public GameObject barrierPrefab;                            // 장애물
    [HideInInspector] public Queue<GameObject> barrierQueue;    // 장애물 저장 공간

    public static BarrierPooling Instance
    {
        get
        {
            if (barrierPoolingInstance == null)
            {
                barrierPoolingInstance = new BarrierPooling();
            }
            return barrierPoolingInstance;
        }
    }

    private void Awake()
    {
        barrierQueue = new Queue<GameObject>();

        if (barrierPoolingInstance == null)
        {
            barrierPoolingInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject CreateBarrier()
    {
        GameObject barrierObject = Instantiate(barrierPrefab);
        barrierObject.SetActive(false);
        barrierObject.transform.SetParent(transform);
        barrierQueue.Enqueue(barrierObject);

        return barrierObject;
    }

    public GameObject SetBarrier()
    {
        if (barrierQueue.Count > 0)
        {
            GameObject barrierObject;
            barrierObject = barrierQueue.Dequeue();
            barrierObject.transform.SetParent(null);
            barrierObject.name = "barrier";
            barrierObject.gameObject.SetActive(true);

            return barrierObject;
        }
        else
            return null;
    }

    public void PickUpBarrier(GameObject barrier)
    {
        barrierQueue.Enqueue(barrier);
        barrier.SetActive(false);
        barrier.transform.SetParent(transform);
    }
}
