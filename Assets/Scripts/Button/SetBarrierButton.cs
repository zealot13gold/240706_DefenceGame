using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SetBarrierButton : MonoBehaviour
{
    /*public */GameObject barrierPrefab;                // 장애물 원본
    public LayerMask layer;                         // 장애물 배치 시 배치를 고려하는 지역(해당 레이어 위에 장애물이 그려짐)
    public LayerMask ground;                        // 장애물 배치가 실제로 가능한 지역

    private GameObject barrier=null;                     // 장애물
    private Vector3 mousePosition;
    private bool setBarrier;
    float delayTime;

    public GameObject cashUI;
    public GameObject costUI;

    public Text costText;
    int maxProd;
    string prodMessage;

    private void Awake()
    {
        barrierPrefab = BarrierPooling.Instance.barrierPrefab;      // 장애물 원본은 풀링의 것과 동일함
    }


    public void SetBarrier()
    {
        if (StageManager.instance.currentState == StageManager.instance.stagePrepare)                         // 게임이 스테이지 준비 상태일 경우
        {
            //Debug.LogFormat("장애물 버튼 클릭");
            //Debug.LogFormat("장애물 == {0}", barrier);
            // 버튼 클릭 시 마우스 위치에 장애물(모니터 뷰) -> 마우스 이동에 따라 장애물도 같이 이동
            if (barrier == null && StageManager.instance.cash >= StageManager.instance.barrierCost)           // 장애물이 선택되지 않은 상태에서, 현재 자금이 장애물 가격보다 크거나 같으면
            {
                mousePosition = Input.mousePosition+CameraDepth();           // 장애물은 배치 전까지 마우스 커서를 따라다님

                if(BarrierPooling.Instance.barrierQueue.Count<=0)
                {
                    BarrierPooling.Instance.CreateBarrier();
                }

                barrier = BarrierPooling.Instance.SetBarrier();
                setBarrier = true;
                //Debug.LogFormat("{0} 초기 위치: {1}", barrier.name, barrier.transform.position);

                delayTime = 0f;
            }
        }
    }

    public void MouseOnButton()
    {
        cashUI.SetActive(false);

        maxProd = StageManager.instance.cash / StageManager.instance.barrierCost;
        if (maxProd >= 1)
        {
            prodMessage = "Able to set " + maxProd.ToString() + " barriers";
        }
        else
        {
            prodMessage = "Not enough cash";
            costText.color = Color.red;
        }
        costText.text = "Cash: " + StageManager.instance.cash.ToString() + '\n' + '\n' + "Barrier: " + StageManager.instance.barrierCost.ToString() + " cash" + '\n' + prodMessage;

        costUI.SetActive(true);
    }

    public void MouseOffButton()
    {
        cashUI.SetActive(true);
        costUI.SetActive(false);
        costText.color = Color.green;
    }

    public void Update()
    {
        if (setBarrier && StageManager.instance.currentState == StageManager.instance.stagePrepare)           // 게임이 준비 상태이고, 장애물 설치 버튼을 눌렀을 경우
        {
            mousePosition = Input.mousePosition+CameraDepth();            // 장애물은 배치 전까지 마우스 커서를 따라다님

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //Debug.LogFormat("최종 검색 위치: {0}", worldPosition);
            // 실시간으로 장애물 위치 및 회전 설정 업데이트
            barrier.transform.position = worldPosition;
            barrier.transform.Rotate(new Vector3(0f, Input.mouseScrollDelta.y*5f, 0f));
            

            RaycastHit hit;

            // 장애물 배치 가능 여부 확인
            if (Physics.BoxCast(barrier.transform.position+Vector3.up*5f, barrier.GetComponent<Collider>().bounds.size/1.5f, Vector3.down, out hit, barrier.transform.rotation, Mathf.Infinity))
            {
                barrier.transform.position = hit.point;                 // 장애물 위치 재설정
                Debug.LogFormat("SetbarrierButton: 충돌 오브젝트 이름: {0}", hit.collider.gameObject.name);
                
                if(hit.collider.name == "Bridge")
                {
                    barrier.GetComponent<BarrierCollision>().installable=true;           // 장애물 배치 가능
                    Debug.LogFormat("SetbarrierButton: 장애물 배치 가능");
                    ClickToSetBarrier();        // 장애물 설치 함수
                }
                else
                {
                    barrier.GetComponent<BarrierCollision>().installable=false;          // 장애물 배치 불가능   
                    Debug.LogFormat("SetbarrierButton: 장애물 배치 불가능");
                }
            }

            if ((barrier != null && Input.GetKey(KeyCode.Escape)) || StageManager.instance.currentState != StageManager.instance.stagePrepare)
            {
                Cancle(barrier);
            }
        }
        else
        {
            Debug.LogFormat("SetBarrierButton: 게임 상태가 준비 상태가 아님");
            Cancle(barrier);
        }
    }

    Vector3 CameraDepth()
    {

        float verticalMousePositionRatio = 1f-(Input.mousePosition.y / Screen.height);
        float verticalMouseAngle = Camera.main.fieldOfView * verticalMousePositionRatio;

        Debug.LogFormat("마우스 y축 위치: {0}, 스크린 y축 크기: {1}, 마우스/전체화면: {2}", Input.mousePosition.y, Screen.height, verticalMouseAngle);

        float camHeight = Camera.main.transform.position.y;
        float camRotation = (Mathf.Abs(Camera.main.transform.rotation.eulerAngles.x) - (Camera.main.fieldOfView / 2.0f));
        Debug.LogFormat("카메라 회전: {0}, 카메라 FOV: {1}", Mathf.Abs(Camera.main.transform.rotation.eulerAngles.x), Camera.main.fieldOfView);
        Debug.LogFormat("카메라 높이: {0}, 각도: {1}", camHeight, camRotation);

        float angle = camRotation + verticalMouseAngle;

        float depth = camHeight / Mathf.Sin(angle*Mathf.Deg2Rad);
        Debug.LogFormat("마우스 각도: {0}, 깊이: {1}", angle, depth);

        return new Vector3(0,0, depth);
    }

    void ClickToSetBarrier()
    {
        if (Input.GetMouseButton(0) && barrier != null)                                 // 장애물이 마우스 커서를 따라다니는 상태에서 클릭하였을 경우
        {
            Debug.LogFormat("SetBarrierButton: 마우스 클릭, {0} 배치 완료", barrier);

            StageManager.instance.cash -= StageManager.instance.barrierCost;
            barrier = null;                                                         // 장애물 삭제

            setBarrier = false;                                                     // 장애물을 설치하였다면 setBarrier를 false로 변경                                                                                                
        }
    }
    
    void Cancle(GameObject barrier)
    {
        setBarrier = false;
            //BarrierPooling.Instance.PickUpBarrier(barrier);
            Destroy(barrier);
            //barrier = null;
            Debug.LogFormat("장애물 설치 취소, 장애물 == {0}", barrier == null);
        
        // 스테이지 시작 시 마우스 커서를 따라다니는 장애물 제거

    }
}
