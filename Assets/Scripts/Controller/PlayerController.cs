using System.Collections.Generic;
using UnityEngine;

/*
 * 역할
 - 마우스/키 입력에 따른 명령 수행

 * 입력값
 - 카메라 : 이 오브젝트의 위치에 따라 이동

 * 함수
 - 유닛 선택 : 마우스를 이용한 유닛 선택 -> 해당 유닛이 이 오브젝트의 하위 오브젝트인지 확인
 - 유닛 이동/공격 : 선택된 유닛(들)을 입력값으로 함 -> 마우스 클릭으로 인한 유닛 이동/공격
 */

public class PlayerController : MonoBehaviour
{   
    private static PlayerController instance;

    // 선택된 유닛 관리
    [HideInInspector] public List<GameObject> chosenObject;    // 선택된 유닛목록 리스트

    // 레이어 목록
    public LayerMask ground;                                   // 필드 레이어 -> 유닛 이동 시 사용
    public LayerMask unitLayer;                                // 유닛 레이어 -> 유닛 선택 시 사용

    // 마우스 조작
    bool onClick;                                             // 클릭한 상태인지 확인
    bool isDrag;                                              // 드래그 실행 중인지 확인
    Rect dragRect;                                            // 드래그 범위(사각형)
    Vector3 dragBegin;                                        // 드래그 시작점
    Vector3 dragEnd;                                          // 드래그 끝점
    public GameObject dragArea;                                // 드래그 선택 영역
    RectTransform dragAreaTransform;                            // 드래그 선택 영역의 rectTransform

    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerController();
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        chosenObject = new List<GameObject>(); 
        dragAreaTransform = dragArea.GetComponent<RectTransform>();
    }

     void Start()
    {
        onClick = false;
        isDrag = false;

        dragArea.SetActive(false);
        dragAreaTransform.sizeDelta = Vector2.zero;
    }

     void FixedUpdate()
    {
        // 유닛 선택
        MouseSelection();
        if (isDrag) MouseDragSelection();

        // 유닛 이동 좌표 전달
        UnitDestination();

        // 사망한 유닛 검색 후 제거
        DeselctDeadUnit();
    }

    void MouseSelection()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectOne(Input.mousePosition);
        }
    }

    void SelectOne(Vector3 position)                                                // 한 번 클릭
    {
        //Debug.LogFormat("클릭 실행");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(position);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))                // 유닛 선택
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
            //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(실행 전) : {0}", chosenObject.Count);
            if (chosenObject.Count > 0)                                               // 기존의 선택 리스트에 유닛/건물이 존재할 경우
            {

                DeselectAllUnits();                                                         // 선택한 모든 유닛/건물 리스트 삭제
            }
            else
            {
                //Debug.LogFormat("아무 것도 선택되지 않음->Drag");
                isDrag = true;                                                      // 단순 클릭이 아니라 드래그 -> 드래그 함수 실행
            }
            //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(실행 후) : {0}", chosenObject.Count);
        }

        // 선택된 유닛의 isSelected는 모두 true로 변경
        for (int i = 0; i < chosenObject.Count; i++)
        {
            //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(istrue) : {0}", chosenObject.Count);
            //Debug.LogFormat("{0}이 chosenObject 내에 존재함", chosenObject[i].name);

            bool isBuffer = chosenObject[i].GetComponent<PlayerUnit>().isSelected = true;
            //Debug.LogFormat("{0}의 istrue를 {1}로 함", chosenObject[i].name, chosenObject[i].GetComponent<PlayerUnit>().isSelected);

            chosenObject[i].GetComponent<PlayerUnitSM>().playerUnitVoice.clip = chosenObject[i].GetComponent<PlayerUnitSM>().playerSelectedVoice;
            chosenObject[i].GetComponent<PlayerUnitSM>().playerUnitVoice.Play();
        }
    }

    void MouseDragSelection()
    {
        
        if(Input.GetMouseButton(0))                             // 선택 범위가 늘어나는 중
        {
            //Debug.LogFormat("드래그 실행");
            if (chosenObject.Count > 0)                        // 선택 리스트에 오브젝트가 존재할 경우
            {
                DeselectAllUnits();                                 // 선택 리스트의 오브젝트를 모두 제거
            }

            //Debug.LogFormat("드래그 시작, 상태 : {0}", isDrag);
            dragRect = new Rect();
            if (!onClick)
            {
                dragBegin = Input.mousePosition;                 // 클릭 한번으로 시작점 저장, 시작점은 바뀌지 않음
                
                //Debug.LogFormat("왼쪽 버튼 누름 시작, dragBegin 위치 : {0}", dragBegin);
                onClick = true;                                  // 클릭한 상태 저장
            }

            dragEnd = Input.mousePosition;                 // 끝점은 갱신
            //Debug.LogFormat("왼쪽 버튼 누르는 중, dragEnd 위치 : {0}", dragEnd);

            // 사각형 생성
            dragRect.xMin = MinValue(dragBegin.x, dragEnd.x);
            dragRect.xMax = MaxValue(dragBegin.x, dragEnd.x);
            dragRect.yMin = MinValue(dragBegin.y, dragEnd.y);
            dragRect.yMax = MaxValue(dragBegin.y, dragEnd.y);

            DrawRectArea();

            Debug.LogFormat("사각형 생성, width: {0}, height: {1}", dragRect.xMax-dragRect.xMin, dragRect.yMax-dragRect.yMin);
            Debug.LogFormat("사각형 크기: width: {0}, height: {1}", Mathf.Abs(dragBegin.x - dragEnd.x), Mathf.Abs(dragBegin.y - dragEnd.y));

            SearchUnitInCamera();

            //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(실행 후) : {0}", chosenObject.Count);
            // 선택된 유닛의 isSelected는 모두 true로 변경
            for (int i = 0; i < chosenObject.Count; i++)
            {
                //Debug.LogFormat("chosenObject 리스트 내 오브젝트 개수(istrue) : {0}", chosenObject.Count);
                //Debug.LogFormat("{0}의 istrue를 true로 함", chosenObject[i].name);
                chosenObject[i].GetComponent<PlayerUnit>().isSelected = true;
            }
        }

       if(Input.GetMouseButtonUp(0))                            // 선택 완료
        {
            //SearchUnitInCamera();
            // 선택 범위 삭제
            // 사각형 삭제
            //Debug.LogFormat("왼쪽 버튼 누름 끝");
            onClick = false;
            isDrag = false;                                         // 드래그 종료
            //Debug.LogFormat("드래그 종료, 상태 : {0}", isDrag);
            eraseDrawArea();
        }
     }

    // Drag 시 사각형 왼쪽 위와 오른쪽 아래를 구분하는 함수
    float MinValue(float a, float b)
    {
        if (a < b)
            return a;
        else
            return b;
    }
    float MaxValue(float a, float b)
    {
        if (a > b)
            return a;
        else
            return b;
    }

    // 선택 영역을 그리는 함수
    void DrawRectArea()
    {
        dragAreaTransform.position = dragBegin;                 // 영역 사각형 시작지점 선택
        dragArea.SetActive(true);                               // 영역 사각형 활성화

        Vector2 rectPivot;                                      // 영역 피벗 설정

        if(dragBegin.x < dragEnd.x)                             // 시작점 x, y값이 끝점 x, y 값보다 작으면 피벗을 0으로, 그렇지 않으면 1로 설정
        {
            rectPivot.x = 0f;
        }
        else 
        {
            rectPivot.x = 1f;
        }

        if (dragBegin.y < dragEnd.y)
        {
            rectPivot.y = 0f;
        }
        else
        {
            rectPivot.y = 1f;
        }

        dragAreaTransform.pivot = rectPivot;

        // 영역 width, height 설정
        dragAreaTransform.sizeDelta = new Vector2(Mathf.Abs(dragRect.xMax-dragRect.xMin), Mathf.Abs(dragRect.yMax-dragRect.yMin));
        Debug.LogFormat("{0} 크기의 사각형 생성", dragAreaTransform.sizeDelta);
    }

    void eraseDrawArea()
    {
        dragArea.SetActive(false);
    }

    void SearchUnitInCamera()
    {
       //Debug.LogFormat("dragRact의 선택범위 : ({0}, {1}), ({2}, {3})", dragRect.xMin, dragRect.yMin, dragRect.xMax, dragRect.yMax);
        Vector3 point;

        for (int i=0; i< StageManager.instance.playerManager.playerUnitList.Count; i++)           // -> foreach문으로 변경
        {
            point = Camera.main.WorldToScreenPoint(StageManager.instance.playerManager.playerUnitList[i].transform.position);
            //Debug.LogFormat("{0}의 좌표 변경 : {1}", GameManager.Instance.playerManager.playerUnitList[i].name, point);
            if (point.x >= dragRect.xMin && point.x <= dragRect.xMax && point.y >= dragRect.yMin && point.y <= dragRect.yMax)
            {
                chosenObject.Add(StageManager.instance.playerManager.playerUnitList[i]);
                //Debug.LogFormat("{0}은 카메라에 검색됨", GameManager.Instance.playerManager.playerUnitList[i].name);
            }
            else
            {
                if (chosenObject.Count > 0)                        // 선택 리스트에 오브젝트가 존재할 경우
                {
                    DeselectAllUnits();                                 // 선택 리스트의 오브젝트를 모두 제거
                }
                //Debug.LogFormat("{0}은 검색 안됨", GameManager.Instance.playerManager.playerUnitList[i].name);
            }
        }
    }

    void DeselectAllUnits()
    {
        if (chosenObject.Count > 0)
        {

            foreach(GameObject unit in chosenObject)
            {
                //Debug.LogFormat("기존 선택된 유닛 모두 삭제 : {0}", unit.name);
                unit.GetComponent<PlayerUnit>().isSelected = false;
            }
            chosenObject.Clear();
        }
        return;
    }

   void DeselctDeadUnit()
    {
        List<GameObject> deadUnitList = new List<GameObject>();

        if (chosenObject.Count > 0)
        {
            foreach (GameObject unit in chosenObject)
            {
                if (unit.GetComponent<Health>().currentHP <= 0)
                {
                   deadUnitList.Add(unit);
                }
            }

            foreach(GameObject unit in deadUnitList)
            {
                chosenObject.Remove(unit);
            }
        }
    }

    void UnitDestination()
    {
        Vector3 unitDestiation;                                 // 목적지 좌표를 유닛이 이동할 수 없는 지점으로 초기화, UnitDestination()이 실행될 때마다 초기화

        if(Input.GetMouseButtonDown(1))                                                             // 오른쪽 클릭을 실행하였을 경우
        {
            if (chosenObject.Count > 0)                                                             // 선택된 오브젝트(유닛)이 1개 이상일 경우
            {
                Vector3 mousePosition = Input.mousePosition;                                        // 현재 마우스가 위치한 좌표(스크린)을 mousePosition에 저장

                Ray rayPosition = Camera.main.ScreenPointToRay(mousePosition);                      // 

                if (Physics.Raycast(rayPosition, out RaycastHit hit, Mathf.Infinity, ground))
                {
                    unitDestiation = hit.point /*+ new Vector3(0f, 0.5f, 0f)*/;                             // 유닛이 이동할 위치를 지정, hit.point(바닥)에 y=0.5를 더함으로써 유닛 오브젝트의 중심점과의 차이를 상쇄
                    Debug.LogFormat("목적지(오른쪽 클릭) : {0}", hit.point);


                    for (int i = 0; i < chosenObject.Count; i++)                                    // chosenUnit 내부에 저장된 유닛별로 목적지 지정
                    {
                        if (Mathf.Abs((unitDestiation - chosenObject[i].transform.position).magnitude) >= 1.5)   // 목적지 좌표(unitDestination)가 유닛의 현재 위치가 아니면
                        {
                            // 선택된 유닛이 Human 유닛인지 확인하는 작업 필요
                            if (chosenObject[i].CompareTag("Human"))
                            {
                                chosenObject[i].GetComponent<PlayerUnitSM>().dest = unitDestiation;             // 유닛의 목적지를 설정
                                chosenObject[i].GetComponent<PlayerUnitSM>().ForceMove();                     // 이동 상태를 true로 설정
                                chosenObject[i].GetComponent<PlayerUnitSM>().playerUnitVoice.clip = chosenObject[i].GetComponent<PlayerUnitSM>().playerForcedMoveVoice;
                                chosenObject[i].GetComponent<PlayerUnitSM>().playerUnitVoice.Play();
                            }
                        }
                    }
                }

            }
            //else { Debug.LogFormat("선택된 유닛이 없음"); }
        }
        else
        {                                                                               // 오른쪽 클릭을 하지 않았을 경우
            return;
        }
    }
}
