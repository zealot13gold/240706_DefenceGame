using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class SetBarrierButton : MonoBehaviour
{
    /*public */GameObject barrierPrefab;                // 장애물 원본
    public LayerMask layer;                         // 장애물 배치 시 배치를 고려하는 지역(해당 레이어 위에 장애물이 그려짐)
    public LayerMask ground;                        // 장애물 배치가 실제로 가능한 지역

    private GameObject barrier=null;                     // 장애물
    private Vector3 mousePosition;
    private bool setBarrier;
    float delayTime;

    private void Awake()
    {
        barrierPrefab = BarrierPooling.Instance.barrierPrefab;      // 장애물 원본은 풀링의 것과 동일함
    }


    public void SetBarrier()
    {

        if (GameManager.Instance.currentState == GameManager.Instance.stagePrepare)                         // 게임이 스테이지 준비 상태일 경우
        {
            //Debug.LogFormat("장애물 버튼 클릭");
            //Debug.LogFormat("장애물 == {0}", barrier);
            // 버튼 클릭 시 마우스 위치에 장애물(모니터 뷰) -> 마우스 이동에 따라 장애물도 같이 이동
            if (barrier == null && GameManager.Instance.cash >= GameManager.Instance.barrierCost)           // 장애물이 선택되지 않은 상태에서, 현재 자금이 장애물 가격보다 크거나 같으면
            {
                mousePosition = Input.mousePosition+CameraDepth();           // 장애물은 배치 전까지 마우스 커서를 따라다님
                //barrierRotation = Quaternion.identity;

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

    public void Update()
    {
        if (setBarrier && GameManager.Instance.currentState == GameManager.Instance.stagePrepare)           // 게임이 준비 상태이고, 장애물 설치 버튼을 눌렀을 경우
        {
            mousePosition = Input.mousePosition+CameraDepth();            // 장애물은 배치 전까지 마우스 커서를 따라다님

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            //Debug.LogFormat("최종 검색 위치: {0}", worldPosition);

            RaycastHit hit;

            if (Physics.Raycast(worldPosition + new Vector3(0f, 5f, 0f), Vector3.down, out hit, Mathf.Infinity, layer))
            {
                barrier.transform.position = hit.point /*+ new Vector3(0f, barrier.transform.lossyScale.y/2f, 0f)*/;
                barrier.transform.Rotate(new Vector3(0f, Input.mouseScrollDelta.y*5f, 0f));

                //Debug.LogFormat("{0} 스크린 좌표: {1}, 월드 좌표: {2}", barrier, mousePosition, Camera.main.ScreenToWorldPoint(mousePosition));

                barrier.SetActive(true);

                

                if (delayTime < 1f)                                                 // 마우스 중복 클릭을 방지하기 위해 시간 간격을 둠
                {
                    delayTime += Time.deltaTime;
                }
                else
                {
                    // 다른 부분과 충돌 확인
                    if (!barrier.GetComponent<BarrierCollision>().isCollide)
                    {
                        if (Input.GetMouseButton(0) && barrier != null)                                 // 장애물이 마우스 커서를 따라다니는 상태에서 클릭하였을 경우
                        {
                            //Debug.LogFormat("마우스 클릭, {0} 배치 완료", barrier);

                            //Instantiate(barrierPrefab, barrier.transform.position, barrier.transform.rotation);
                            GameManager.Instance.cash -= GameManager.Instance.barrierCost;
                            //barrier.SetActive(false);
                            barrier = null;                                                         // 장애물 삭제

                            setBarrier = false;                                                     // 장애물을 설치하였다면 setBarrier를 false로 변경                                                                                                
                        }
                    }
                    else
                    {
                        Debug.LogFormat("이 곳에 장애물을 설치할 수 없음");
                    }
                }
            }

            if ((barrier != null && Input.GetKey(KeyCode.Escape)) || GameManager.Instance.currentState != GameManager.Instance.stagePrepare)
            {
                Cancle(barrier);
            }
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
