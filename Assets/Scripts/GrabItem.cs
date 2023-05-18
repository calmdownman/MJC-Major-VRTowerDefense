using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    bool isGrabbing = false; //물체를 잡고 있는지의 여부
    GameObject grabbedObject; //잡고 있는 물체
    public LayerMask grabbedLayer; //잡을 물체의 종류
    public float grabRange = 0.2f; //잡을 수 있는 거리
                                   // Start is called before the first frame update

    Vector3 prevPos; //이전 위치
    public float throwPower = 10; //던질 힘

    Quaternion prevRot;
    public float rotPower = 5f;

    // Update is called once per frame
    void Update()
    {
        if(!isGrabbing)
        {
            TryGrab();
        }
        else
        {
            TryUnGrab();
        }
    }

    void TryGrab()
    {
        //[Grab] 버튼을 누르면 일정 영역 안에 있는 폭탄을 집는다
        if(ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            //일정 영역 안에 있는 모든 폭탄 검출
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange,
                grabbedLayer);
            int closest = 0; //가장 가까운 폭탄 인덱스
            //손과 가장 가까운 물체 선택
            for(int i = 0; i < hitObjects.Length; i++)
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
                grabbedObject = hitObjects[closest].gameObject; //잡을 물체에 대한 기억
                grabbedObject.transform.parent = ARAVRInput.RHand; //잡은 물체를 손의 자식으로
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                prevPos = ARAVRInput.RHandPosition; //초기 위치값 지정
                prevRot = ARAVRInput.RHand.rotation; //초기 회전값 저장
            }
        }
    }

    void TryUnGrab()
    {
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos); //던질 방향
        prevPos = ARAVRInput.RHandPosition;
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        prevRot = ARAVRInput.RHand.rotation;

        //버
        if(ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger,ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false; //잡지 않은 상태로 전환
            grabbedObject.GetComponent<Rigidbody>().isKinematic=false; //물리 기능 활성화
            grabbedObject.transform.parent = null; //손에서 폭탄 떼어내기
            //grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower; //VR 던지기
            grabbedObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwPower; //PC 던지기
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            grabbedObject = null; //잡은 물체가 없도록 설정
            
        }
    }
}
