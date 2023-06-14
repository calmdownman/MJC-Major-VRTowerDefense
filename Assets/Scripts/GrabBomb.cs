using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBomb : MonoBehaviour
{
    bool isGrabbing = false; //물체를 잡고 있는지의 여부
    GameObject grabbedObject; // 잡고 있는 물체
    public LayerMask grabbedLayer; // 잡을 물체의 종류
    public float grabRange = 0.2f; // 잡을 수 있는 거리

    Vector3 prevPos; //이전 위치
    public float throwPower = 20f; // 던질 힘

    Quaternion prevRot;
    public float rotPower = 5f;
    public bool isRemoteGrab = true;
    public float remoteGrabDistance = 20f;

    // Update is called once per frame
    void Update()
    {
        if (isGrabbing == false)
        {
            TryGrab();
        }
        else {
            TryUngrab();
        }
    }
    void TryGrab()
    {
        //[Grab] 버튼을 누르면 일정 영역 안에 있는 폭탄을 집는다.
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            if(isRemoteGrab) //원거리 물체잡기를 사용한다면
            {
                //손 방향으로 Ray 제작
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;
                //SphereCast를 이용해 물체 충돌을 체크
                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance, grabbedLayer))
                {
                    isGrabbing = true; //잡은 상태로 전환
                    grabbedObject = hitInfo.transform.gameObject; // 잡은 물체에 대한 기억
                    StartCoroutine(GrabbingAnimation()); //물체가 끌려오는 기능 실행
                }
                return;
            }
            //일정 영역 안에 있는 모든 폭탄 검출
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange,
                grabbedLayer);
            int closest = 0; //가장 가까운 폭탄 인덱스
            //손과 가장 가까운 물체 선택
            for(int i=0; i<hitObjects.Length; i++)
            {
                //손과 가장 가까운 물체와의 거리
                Vector3 colsestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(colsestPos, ARAVRInput.RHandPosition);
                //다음 물체와 손의 거리
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);
                //다음 물체와의 거리가 더 가깝다면
                if(nextDistance < closestDistance)
                {
                    closest = i; //가장 가까운 물체 인덱스 교체
                }
            }

            //검출된 물체가 있을 경우
            if(hitObjects.Length > 0)
            {
                isGrabbing = true; //잡은 상태로 전환
                grabbedObject = hitObjects[closest].gameObject; // 잡을 물체에 대한 기억
                grabbedObject.transform.parent = ARAVRInput.RHand; //잡은 물체를 손의 자식으로 등록
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                prevPos = ARAVRInput.RHandPosition; //초기 위치값 지정
                prevRot = ARAVRInput.RHand.rotation; //초기 회전값 저장
            }
        }
    }
    void TryUngrab()
    {
        //Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos); //던질 방향
        
        prevPos = ARAVRInput.RHandPosition;
        //쿼터니온 공식
        //angle1 = Q1. angle2 = Q2
        //-angle1 + angle2 = Q1 * Q2
        //-angle2 = Quaternion.Inverse(Q2)
        //angle2-angle1 = Quaternion.FromToRatation(Q1, Q2) = Q2*Quaterniton.Inverse(Q1)
        //회전 방향 = current - previous의 차로 구함. -previous는 Inverse로 구함.
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        //이전 회전 저장
        prevRot = ARAVRInput.RHand.rotation;
        //버튼을 놓았다면..
        if (ARAVRInput.GetUp(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false; //잡지 않은 상태로 전환
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false; // 물리 기능 활성화
            grabbedObject.transform.parent = null; // 손에서 폭탄 떼어내기

            // grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower; //VR던지기
             grabbedObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwPower; //PC던지기
            //각속도 = (1/dt) * dθ(특정 축 기준 변위 각도)
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f/Time.deltaTime)*angle*axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            grabbedObject = null; //잡은 물체가 없도록 설정
            
        }
    }
    IEnumerator GrabbingAnimation() //내 손으로 빨려 들어오는 함수
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true; //물리 기능 정지
        prevPos = ARAVRInput.RHandPosition; //초기 위치 값 지정
        prevRot = ARAVRInput.RHand.rotation; //초기 회전 값 지정
        Vector3 startLocation = grabbedObject.transform.position; 
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;
        //경과율
        float elapsedRate = currentTime / finishTime;
        while(elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);
            yield return null;
        }
        //잡은 물체를 손의 자식으로 등록
        grabbedObject.transform.position = targetLocation;
        grabbedObject.transform.parent = ARAVRInput.RHand;
    }
}
