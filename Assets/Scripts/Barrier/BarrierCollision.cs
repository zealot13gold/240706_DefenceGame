using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCollision : MonoBehaviour
{
    [HideInInspector] public bool isCollide;

    //public GameManager gm;

    // 쉐이더
    public MeshRenderer cannotBulidMeshRender;
    private Material negativeMaterial;

    private void Awake()
    {
        isCollide = false;
        negativeMaterial = cannotBulidMeshRender.material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.LogFormat("Collision, 현재 게임 상태: {0}", GameManager.Instance.currentState);
        if (collision.gameObject.layer != 8 && GameManager.Instance.currentState == GameManager.Instance.stagePrepare)              // 레이어가 ground이고, 게임이 준비 상태일 경우
        {
            isCollide = true;
            negativeMaterial.SetColor("_Color", new Vector4 (1f, 0.5f, 0.5f, 1f));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer != 8 && GameManager.Instance.currentState == GameManager.Instance.stagePrepare)              // 레이어가 ground이고, 게임이 준비 상태일 경우
        {
            isCollide = false;
            negativeMaterial.SetColor("_Color", new Vector4(1f, 1f, 1f, 1f));
        }
    }
}
