using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierCollision : MonoBehaviour
{
    [HideInInspector] public bool installable;
    public LayerMask groundLayer;           // ??? ?? ?? ??


    // ??? ?? ?? ??
    public MeshRenderer cannotBulidMeshRender;
    private Material negativeMaterial;

    private void Awake()
    {
        installable = false;
        negativeMaterial = cannotBulidMeshRender.material;
        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        DetectBarrierInstallableArea();
    }

    private void DetectBarrierInstallableArea()
    {
           if(installable)
           {
            negativeMaterial.SetColor("_Color", new Vector4(1f, 1f, 1f, 1f));
            Debug.LogFormat("BarrierCollision: ??? ?? ??");
           }
           else
           {
            negativeMaterial.SetColor("_Color", new Vector4(1f, 0.5f, 0.5f, 1f));
            Debug.LogFormat("BarrierCollision: ??? ?? ???");
           }
    }

    // ??? ?? ?? ?? ???
    void OnDrawGizmos()
    {
        

        // BoxCast ???
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
        
        // BoxCast ?? ?? ???
        Gizmos.color = Color.red;
        Vector3 endPosition = transform.position + Vector3.down * 10f;
        Gizmos.DrawLine(transform.position, endPosition);
        
        // BoxCast ???
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(endPosition, transform.localScale);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.LogFormat("Collision, ���� ���� ����: {0}", GameManager.Instance.currentState);
    //    if (collision.gameObject.layer != 8 && GameManager.Instance.currentState == GameManager.Instance.stagePrepare)              // ���̾ ground�̰�, ������ �غ� ������ ���?
    //     {
    //         isCollide = true;
    //         negativeMaterial.SetColor("_Color", new Vector4 (1f, 0.5f, 0.5f, 1f));
    //     }
    // }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer != 8 && GameManager.Instance.currentState == GameManager.Instance.stagePrepare)              // ���̾ ground�̰�, ������ �غ� ������ ���?
    //     {
    //         isCollide = false;
    //         negativeMaterial.SetColor("_Color", new Vector4(1f, 1f, 1f, 1f));
    //     }
    // }
}
