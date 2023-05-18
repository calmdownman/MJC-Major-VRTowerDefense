using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCircleUI; //텔레포드 표시 UI
    LineRenderer lr; //라인 렌더러
    Vector3 oringinScale = Vector3.one * 0.02f; //최초 텔레포트 UI 크기
    public int lineSmooth = 40;// 커브의 부드러운 정도
    public float curveLength = 50f; //커브의 길이
    public float gravity = -60f; //커브의 중력
    public float simulateTime = 0.02f; //곡선 시뮬레이션의 간격 및 시간
    List<Vector3> lines = new List<Vector3>(); //곡선을 이루는 점들

    void Start()
    {
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.0f;
        lr.endWidth = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        //왼쪽 컨트롤러의 One버튼을 눌렀을 때
        if(ARAVRInput.GetDown(ARAVRInput.Button.One,ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true; //라인렌더러 활성화
        }
        //왼쪽 컨트롤러의 One버튼에서 손을 떼면
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false; //라인렌더러 비활성화
            //텔레포드 UI가 활성화 되어 있다면...
            if (teleportCircleUI.gameObject.activeSelf)
            {
                GetComponent<CharacterController>().enabled = false;
                transform.position = teleportCircleUI.position + Vector3.up;
                GetComponent<CharacterController>().enabled = true;
            }
            teleportCircleUI.gameObject.SetActive(false);
        }
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            MakeLines();
        }
    }

    void MakeLines()
    {
        lines.RemoveRange(0, lines.Count); //리스트를 모두 비워줌
        //선이 진행될 방향을 정한다
        Vector3 dir = ARAVRInput.LHandDirection * curveLength;
        //선이 그려질 위치의 초기값 설정
        Vector3 pos = ARAVRInput.LHandPosition;
        //최초 위치를 리스트에 담는다

        for(int i=0; i<lineSmooth; i++) //lineSmooth 개수만큼 반복
        {
            Vector3 lastPos = pos; //현재 위치 기억
            //중력을 적용한 속도 계산
            //미래속도(v) = 현재속도(v0) = 가속도(a) * 시간(simulateTime)
            dir.y += gravity * simulateTime;
            //등속 운동으로 다음 위치 계산
            //미래 위치(p) = 현재위치(p0) + 속도(v) * 시간(simulateTime)
            pos += dir * simulateTime;
            if(CheckHitRay(lastPos, ref pos))
            {
                lines.Add(pos); 
                break;
            }
            else
            {
                teleportCircleUI.gameObject.SetActive(false);
            }
            lines.Add(pos);
        }
        lr.positionCount = lines.Count;
        lr.SetPositions(lines.ToArray());
    }
    private bool CheckHitRay(Vector3 lastPos, ref Vector3 pos)
    {
        //앞점(lastPos)에서 다음점(pos)로 향하는 벡터 계산
        Vector3 rayDir = pos - lastPos;
        Ray ray = new Ray(lastPos,rayDir);
        RaycastHit hitInfo;
        //Raycast 할 때 레이의 크기를 앞 점과 다음 점 사이의 거리로 한정
        if(Physics.Raycast(ray, out hitInfo, rayDir.magnitude))
        {
            //다음 점의 위치를 충돌한 지점으로 설정
            pos = hitInfo.point;
            int layer = LayerMask.NameToLayer("Terrain");
            if(hitInfo.transform.gameObject.layer == layer) 
            {
                teleportCircleUI.gameObject.SetActive(true); //텔레포트 UI활성화
                teleportCircleUI.position = pos; //위치 지정
                teleportCircleUI.forward = hitInfo.normal; //방향 설정
                float distance = (pos - ARAVRInput.LHandPosition).magnitude;
                //텔레포트 UI가 보일 크기 설정
                teleportCircleUI.localScale= oringinScale * Mathf.Max(1, distance);
            }
            return true;
        }
        return false;
    }
}
