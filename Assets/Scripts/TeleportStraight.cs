using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;
    LineRenderer lr;
    Vector3 originScale = Vector3.one* 0.02f; //최초의 텔레포트 UI 크기
    // Start is called before the first frame update
    void Start()
    {
        //시작할 때 비활성화
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>(); //라인 렌더러 컴포넌트
    }

    // Update is called once per frame
    void Update()
    {
        if(ARAVRInput.GetDown(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch)) 
        {
            lr.enabled = true;
        } 
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch)) 
        {
            lr.enabled = false;
            if(teleportCircleUI.gameObject.activeSelf)
            {  
                GetComponent<CharacterController>().enabled = false;
                //텔레포트 UI위치로 순간 이동
                transform.position = teleportCircleUI.position + Vector3.up;
                GetComponent<CharacterController>().enabled = true;
            }
            teleportCircleUI.gameObject.SetActive(false);
        }

        //왼쪽 컨트롤러의 One 버튼을 눌렀다면
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if(Physics.Raycast(ray, out hitInfo, 200, layer)) 
            {
                //Ray가 부딪힌 지점에 라인 그리기
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitInfo.point);
                //부딪힌 지점에 텔레포트 UI 표시
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                //텔레포트 UI가 위로 누워있도록 방향 설정
                teleportCircleUI.forward = hitInfo.normal;
                //텔레포드 UI의 크기가 거리에 따라 보정되도록 설정
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitInfo.distance);
            } 
            else
            {//Ray 충돌이 발생하지 않으면...
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }
}
