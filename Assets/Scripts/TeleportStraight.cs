using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TeleportStraight : MonoBehaviour
{
    public GameObject[] obstacles;

    public Transform teleport;
    public Transform teleportCircleUI;
    LineRenderer lr;
    Vector3 originScale = Vector3.one * 0.02f; //최초 텔레포트 UI크기
    /*public bool isWarp = false; //워프 사용 여부
    public float warpTime = 0.1f; //워프에 걸리는 시간
    public PostProcessVolume post; // 사용하고 있는 포스트 프로세싱 볼륨 컴포넌트*/
 
    void Start()
    {
        //시작할때 비활성화
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>(); //라인 렌더러 컴포넌트 
    }

    void Update()
    {
        //스테이지 클리어 후 게임 일시정지 시 작동 
        if (GameManager.Instance.gState != GameManager.GameState.ShoppingWall) return;

        if (ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true;
        }
        else if(ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false;

            if(teleportCircleUI.gameObject.activeSelf)
            {
                /*if(isWarp == false) //워프 기능 사용이 아닐 때 순간 이동
                {
                    GetComponent<CharacterController>().enabled = false;
                    //텔레포트 UI위치로 순간 이동
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                }
                else // 워프 기능 사용할 때는 Warp() 코루틴 함수 호출
                {
                    StartCoroutine(Warp());
                }*/
                Instantiate(obstacles[UIManager.Instance.WallIndex], teleportCircleUI.position, obstacles[UIManager.Instance.WallIndex].transform.rotation);
                UIManager.Instance.UpdateItemText("없음");
                GameManager.Instance.gState = GameManager.GameState.Pause;
            }

            teleportCircleUI.gameObject.SetActive(false);
        }
        //왼쪽 컨트롤러의 One버튼을 눌렀다면
        else if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if (Physics.Raycast(ray, out hitInfo, 200, layer))
            {
                //Ray가 부딪힌 지점에 라인 그리기
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitInfo.point);
                //부딪힌 지점에 텔레포트 UI 표시
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                //텔레포트 UI가 위로 누워있도록 방향 설정
                teleportCircleUI.forward = hitInfo.normal;
                //텔레포트 UI의 크기가 거리에 따라 보정되도록 설정
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitInfo.distance);
            }
            else { //Ray 충돌이 발생하지 않으면..
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }

    }
    /*IEnumerator Warp()
    {
        MotionBlur blur; // 워프 느낌을 표현하는 모션블러
        Vector3 pos = transform.position; // 워프 시작점
        Vector3 targetPos = teleportCircleUI.position + Vector3.up; //목적지(워프 끝점)
        float currentTime = 0; // 워프 경과 시간
        post.profile.TryGetSettings<MotionBlur>(out blur); // 포스트 프로세싱에서 사용 중인 프로파일 얻어오기
        blur.active = true; //워프 시작 전 블러 켜기
        GetComponent<CharacterController>().enabled = false; //플레이어 움직임 X

        //경과 시간이 워프보다 짧은 시간 동안 이동 처리
        while (currentTime < warpTime)
        {
            currentTime += Time.deltaTime; //경과 시간 흐르게 하기끄기
            //워프 시작점에서 도착점에 도착하기 위해 워프 시간 동안 이동
            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);
            yield return null; //코루틴 대기
        }
        transform.position = teleportCircleUI.position + Vector3.up; //텔레포트 위치로 이동
        GetComponent<CharacterController>().enabled = true; // 캐릭터 컨트롤러 다시 켜기
        blur.active = false; //포스트 효과 
    }*/
}
