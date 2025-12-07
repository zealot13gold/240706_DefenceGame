using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MinimapCameraController : MonoBehaviour
{

    public RawImage miniMap;
    RectTransform rt;
    public Transform center;                                // GameManager Transform
    Vector3 centerPosition;
    Vector2 mousePositionInMiniMap;
    Vector3 cameraPosition;

    void Awake()
    {
        rt =  miniMap.transform as RectTransform;
        centerPosition = center.position;
        mousePositionInMiniMap = Vector2.zero;
        cameraPosition = Vector3.zero;
    }

    //private void Start()
    //{
    //    Debug.LogFormat("미니맵의 스크린 상의 좌표: {0}", rt.position);
    //}

    void Update()
    {
        // 클릭하였을 경우
        if (Input.GetMouseButtonDown(0))
        {
            Debug.LogFormat("마우스 월드 좌표: {0}", Input.mousePosition);
            // 마우스 포인터가 미니맵 위에 있을 경우,
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, Input.mousePosition, null))
            {
                MoveMainCamera();
            }
        }
    }

    Vector2 PointingMiniMap()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, Input.mousePosition, null, out mousePositionInMiniMap);

        Debug.LogFormat("미니맵 상의 마우스 위치: {0}", mousePositionInMiniMap);
            
        // 마우스 포인터 위치(상대 좌표) 가져옴
        return mousePositionInMiniMap;
    }

    // 마우스 포인터 위치를 월드 좌표로 변경하는 함수
    Vector3 MousePositionInMinimapToWorldPosition()
    {
        Vector2 position = PointingMiniMap();
        Vector3 cameraPosition = Vector3.zero;

        float xPosRatio = position.x / (rt.rect.width / 2f);
        float zPosRatio = position.y / (rt.rect.height / 2f);

        // 월드 x축: -45 ~ 45 (0)
        // 월드 z축: -75 ~ 15 (-30)

        float world_X = centerPosition.x + (xPosRatio * 45f);
        float world_Z = centerPosition.z + (zPosRatio * 45f);

        Quaternion cameraRot = Camera.main.transform.rotation;

        float world_Y = Camera.main.transform.position.y;
        float rot_Y = (90f - cameraRot.x) * Mathf.PI / 180f;

        //Debug.LogFormat("Tan {0}: {1}", rot_Y*180f/Mathf.PI, (Mathf.Tan((90f - Camera.main.transform.rotation.x) * Mathf.PI / 180f)));
        world_Z = world_Z - (world_Y * Mathf.Atan(rot_Y));

        return cameraPosition = new Vector3(world_X, world_Y, world_Z);
    }

    // 메인 카메라를 미니맵 클릭 지점(월드 좌표)으로 이동하는 함수
    void MoveMainCamera()
    {
        Camera.main.transform.position = MousePositionInMinimapToWorldPosition();
        Debug.LogFormat("카메라는 {0}으로 이동", Camera.main.transform.position);
    }

    //void DrawCameraView(Vector3 cameraPosition)
    //{
    //    Vector3 cameraCenter = Vector3.zero;
    //    Vector3 cameraBelow = Vector3.zero;

    //    float hFOV = Camera.main.fieldOfView * (Screen.width / Screen.height);
    //    Quaternion CameraPerpendicular = Quaternion.Euler(90f, 0f, 0f);                         // 카매라 위치로부터 지면까지의 각도
    //    Quaternion cameraRot = Camera.main.transform.rotation;                                  // 카메라 회전각(쿼터니언)
    //    Quaternion cameraBelowRot = Quaternion.Euler(Camera.main.fieldOfView / 2f, 0f, hFOV/2f);     // 카메라 중심으로부터 카메라 화면 바닥까지 회전각

    //    float cameraRotAngle =Quaternion.Angle(cameraRot, CameraPerpendicular);
    //    float cameraBelowRotAngle = cameraRotAngle - Quaternion.Angle(cameraRot, cameraBelowRot);

    //    cameraCenter.z = cameraPosition.z + (cameraPosition.y * Mathf.Tan(cameraRotAngle * Mathf.PI / 180f));
    //    cameraBelow.z = cameraPosition.z + (cameraPosition.y * Mathf.Tan(cameraBelowRotAngle * Mathf.PI / 180f));

    //    cameraCenter.x = cameraPosition.x + (cameraPosition.y * Mathf.Tan(cameraRotAngle * Mathf.PI / 180f));
    //    cameraBelow.x = cameraPosition.x + (cameraPosition.y * Mathf.Tan(cameraBelowRotAngle * Mathf.PI / 180f));

    //    //cameraCenter.x = cameraPosition.x + (cameraPosition.y * Mathf.Tan((90f - Camera.main.transform.rotation.z) * Mathf.PI / 180f));
    //    //cameraCenter.x = cameraPosition.x;

    //    Debug.LogFormat("카메라 회전각 {0}: {1}",  Quaternion.Angle(cameraRot, CameraPerpendicular), cameraRotAngle);

    //    Debug.LogFormat("카메라 센터 좌표: ({0}, {1})", cameraCenter.x, cameraCenter.z);

    //    float distanceFromCenter_z = Mathf.Abs(cameraCenter.z - cameraBelow.z);
    //    float distanceFromCenter_x = Mathf.Abs(cameraCenter.x - cameraBelow.x);
    //    //float distanceFromCenter_x = Mathf.Abs(cameraCenter.z - (cameraPosition.y * Mathf.Tan((90f - Camera.main.transform.rotation.x - hFOV / 2f) * Mathf.PI / 180f)));

    //    //float distanceFromCenter_x = Screen.width / 2f;
    //    Debug.LogFormat("카메라 센터로부터 거리: ({0}, {1})", distanceFromCenter_x, distanceFromCenter_z);

    //    //Rect viewRT = Rect.zero;

    //    Rect viewRT = new Rect(cameraCenter.x - distanceFromCenter_x, cameraCenter.z - distanceFromCenter_z, distanceFromCenter_x * 2f, distanceFromCenter_z * 2f);
    //    Debug.LogFormat("메인 카메라 사각형 범위: {0}", viewRT);
    //    EditorGUI.DrawRect(viewRT, Color.green);
    //}

}
