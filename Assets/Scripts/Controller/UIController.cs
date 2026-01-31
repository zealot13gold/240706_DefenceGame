using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/*
 * 역할(기존의 PlayerController 역할 수행, 기능 개선)
 - 마우스/키 입력에 따른 명령 수행

 * 입력값
 - 카메라 : 이 오브젝트의 위치에 따라 이동

 * 함수
 - 유닛 선택 : 마우스를 이용한 유닛 선택 -> 해당 유닛이 이 오브젝트의 하위 오브젝트인지 확인
 - 유닛 이동/공격 : 선택된 유닛(들)을 입력값으로 함 -> 마우스 클릭으로 인한 유닛 이동/공격
 */

public class UIController : MonoBehaviour
{
    public static UIController instance = null;

    // 선택된 유닛 관리
    [HideInInspector] public List<GameObject> chosenObject;    // 선택된 유닛목록 리스트

    // 레이어 목록
    [Header("레이어")]
    [Tooltip("배경 레이어")] public LayerMask ground;                                   // 필드 레이어 -> 유닛 이동 시 사용
    [Tooltip("유닛 레이어")] public LayerMask unitLayer;                                // 유닛 레이어 -> 유닛 선택 시 사용

    // 마우스 조작
    bool onClick;                                             // 클릭한 상태인지 확인
    bool isDrag;                                              // 드래그 실행 중인지 확인
    Rect dragRect;                                            // 드래그 범위(사각형)
    [Header("유닛 선택 범위 이미지")]
    [Tooltip("선택 이미지 오브젝트")] public GameObject dragArea;                                // 드래그 선택 영역
    RectTransform dragAreaTransform;                            // 드래그 선택 영역의 rectTransform
    //Vector3 dragBegin;                                        // 드래그 시작점
    //Vector3 dragEnd;                                          // 드래그 끝점

    private void OnEnable()
    {
        if(instance!=null && instance!=this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        chosenObject = new List<GameObject>();
        dragAreaTransform = dragArea.GetComponent<RectTransform>();

        Init();
    }

    void Init()
    {
        onClick = false;
        isDrag = false;

        dragArea.SetActive(false);
        dragAreaTransform.sizeDelta = Vector2.zero;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 마우스 클릭 이벤트 발생 시 -> 유닛 선택
            SelectUnit(Input.mousePosition);
        }

        //if(Input.GetMouseButtonUp(0))
        //{
        //    // 마우스 클릭 이벤트 종료 -> 드래그 유닛 선택
        //    SelectUnit();
        //}

        if(Input.GetMouseButtonDown(1))
        {
            // 마우스 오른쪽 클릭 이벤트 발생 시 -> 유닛 명령 이동
            OrderUnit(Input.mousePosition);
        }

        
    }

    //private void FixedUpdate()
    //{
    //    if(Input.GetMouseButton(0))
    //    {
    //        // 계속 누르고 있을 경우

    //    }
    //}

    void SelectUnit(Vector3 input)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(input);
        // 유닛을 선택한 경우
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
        {
            if (Input.GetButton("AddUnit"))                                         // LeftShift 버튼을 누른 상태일 때
            {
                chosenObject.Add(hit.transform.gameObject);                         // 선택 리스트에 방금 선택한 유닛 추가
                Debug.LogFormat("shift + 선택 후 선택된 유닛 수: {0}", chosenObject.Count);
            }
            else
            {                                                                  // 일반적인 클릭
                DeselectAllUnits();                                                         // 새로운 선택 리스트 생성
                chosenObject.Add(hit.transform.gameObject);                         // 선택 리스트에 방금 선택한 유닛 추가
                //Debug.LogFormat("{0} 선택", hit.transform.gameObject.name);
            }
        }
        else                                                                        // 아무것도 선택하지 않았을 경우
        {
            // 배경 클릭
            DeselectAllUnits();

            // 유닛 드래그 시작
            Vector3 dragBegin = Input.mousePosition;
            isDrag = true;

            StartCoroutine(DragUnit(dragBegin));
            //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(실행 전) : {0}", chosenObject.Count);
            //if (chosenObject.Count > 0)                                               // 기존의 선택 리스트에 유닛/건물이 존재할 경우
            //{

            //    DeselectAllUnits();                                                         // 선택한 모든 유닛/건물 리스트 삭제
            //}
            //else
            //{
            //    //Debug.LogFormat("아무 것도 선택되지 않음->Drag");
            //    isDrag = true;                                                      // 단순 클릭이 아니라 드래그 -> 드래그 함수 실행
            //}
            //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(실행 후) : {0}", chosenObject.Count);
        }
    }

    IEnumerator DragUnit(Vector3 begin)
    {
        // 프레임 단위 별로 마우스 드래그 종료 여부 확인
        yield return null;
        Vector3 dragEnd = Input.mousePosition;
        while (true)
        {
            // 사각형 업데이트
            DrawRectArea(begin, dragEnd);

            // 드래그 종료
            if (Input.GetMouseButtonUp(0))
            {
                onClick = false;
                isDrag = false;
                dragArea.SetActive(false);
                break;
            }
            yield return null;
        }
        // 드래그 종료 -> 범위 내 모든 유닛 선택
        SelectUnit(begin, dragEnd);
    }

    void SelectUnit(Vector3 begin, Vector3 end)
    {
        Vector3 point;
        foreach (GameObject unit in PlayerManager.instance.playerUnitList)           // -> foreach문으로 변경
        {
            point = Camera.main.WorldToScreenPoint(unit.transform.position);
            //Debug.LogFormat("{0}의 좌표 변경 : {1}", GameManager.Instance.playerManager.playerUnitList[i].name, point);
            if (point.x >= dragRect.xMin && point.x <= dragRect.xMax && point.y >= dragRect.yMin && point.y <= dragRect.yMax)
            {
                chosenObject.Add(unit);
                //Debug.LogFormat("{0}은 카메라에 검색됨", GameManager.Instance.playerManager.playerUnitList[i].name);
            }
            //else
            //{
            //    if (chosenObject.Count > 0)                        // 선택 리스트에 오브젝트가 존재할 경우
            //    {
            //        DeselectAllUnits();                                 // 선택 리스트의 오브젝트를 모두 제거
            //    }
            //    //Debug.LogFormat("{0}은 검색 안됨", GameManager.Instance.playerManager.playerUnitList[i].name);
            //}
        }

        //    //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(실행 후) : {0}", chosenObject.Count);
        //    // 선택된 유닛의 isSelected는 모두 true로 변경
        //    for (int i = 0; i < chosenObject.Count; i++)
        //    {
        //        //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(istrue) : {0}", chosenObject.Count);
        //        //Debug.LogFormat("{0}의 istrue를 true로 함", chosenObject[i].name);
        //        chosenObject[i].GetComponent<PlayerUnit>().isSelected = true;
        //    }


        //if (Input.GetMouseButtonUp(0))                            // 선택 완료
        //{
        //    //SearchUnitInCamera();
        //    // 선택 범위 삭제
        //    // 사각형 삭제
        //    //Debug.LogFormat("왼쪽 버튼 누름 끝");
        //    onClick = false;
        //    isDrag = false;                                         // 드래그 종료
        //    //Debug.LogFormat("드래그 종료, 상태 : {0}", isDrag);
        //    dragArea.SetActive(false);
        //}
    }

    void DrawRectArea(Vector3 begin, Vector3 end)
    {
        dragAreaTransform.position = begin;                 // 영역 사각형 시작지점 선택

        dragRect = new Rect();
        Vector2 rectPivot;                                      // 영역 피벗 설정

        // 사각형 생성
        dragRect.xMin = Mathf.Min(begin.x, end.x);
        dragRect.xMax = Mathf.Max(begin.x, end.x);
        dragRect.yMin = Mathf.Min(begin.y, end.y);
        dragRect.yMax = Mathf.Max(begin.y, end.y);

        dragArea.SetActive(true);                               // 영역 사각형 활성화

        if (begin.x < end.x)                             // 시작점 x, y값이 끝점 x, y 값보다 작으면 피벗을 0으로, 그렇지 않으면 1로 설정
        {
            rectPivot.x = 0f;
        }
        else
        {
            rectPivot.x = 1f;
        }

        if (begin.y < end.y)
        {
            rectPivot.y = 0f;
        }
        else
        {
            rectPivot.y = 1f;
        }

        dragAreaTransform.pivot = rectPivot;

        // 영역 width, height 설정
        dragAreaTransform.sizeDelta = new Vector2(Mathf.Abs(dragRect.xMax - dragRect.xMin), Mathf.Abs(dragRect.yMax - dragRect.yMin));
        Debug.LogFormat("{0} 크기의 사각형 생성", dragAreaTransform.sizeDelta);
    }

    void OrderUnit(Vector3 input)
    {
        //if (chosenObject.Count > 0)                                                             // 선택된 오브젝트(유닛)이 1개 이상일 경우
        //{
        //    Vector3 mousePosition = Input.mousePosition;                                        // 현재 마우스가 위치한 좌표(스크린)을 mousePosition에 저장

            Ray rayPosition = Camera.main.ScreenPointToRay(input);                      // 

            if (Physics.Raycast(rayPosition, out RaycastHit hit, Mathf.Infinity, ground))
            {
                Vector3 dest = hit.point /*+ new Vector3(0f, 0.5f, 0f)*/;                             // 유닛이 이동할 위치를 지정, hit.point(바닥)에 y=0.5를 더함으로써 유닛 오브젝트의 중심점과의 차이를 상쇄
                Debug.LogFormat("목적지(오른쪽 클릭) : {0}", hit.point);

                foreach (GameObject unit in chosenObject)                                    // chosenUnit 내부에 저장된 유닛별로 목적지 지정
                {
                    if (Mathf.Abs((dest - unit.transform.position).magnitude) >= 1.5)   // 목적지 좌표(unitDestination)가 유닛의 현재 위치가 아니면
                    {
                        // 선택된 유닛이 Human 유닛인지 확인하는 작업 필요
                        if (unit.CompareTag("Human"))
                        {
                            unit.GetComponent<PlayerUnitSM>().dest = dest;             // 유닛의 목적지를 설정
                            unit.GetComponent<PlayerUnitSM>().ForceMove();                     // 이동 상태를 true로 설정
                            //chosenObject[i].GetComponent<PlayerUnitSM>().playerUnitVoice.clip = chosenObject[i].GetComponent<PlayerUnitSM>().playerForcedMoveVoice;
                            //chosenObject[i].GetComponent<PlayerUnitSM>().playerUnitVoice.Play();
                        }
                    }
                }
            }

        //}
    }

    void DeselectAllUnits()
    {
        if (chosenObject.Count > 0)
        {
            foreach (GameObject unit in chosenObject)
            {
                //Debug.LogFormat("기존 선택된 유닛 모두 삭제 : {0}", unit.name);
                unit.GetComponent<PlayerUnit>().isSelected = false;
            }
            chosenObject.Clear();
        }
        return;
    }
}
