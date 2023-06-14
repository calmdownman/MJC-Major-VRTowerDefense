using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBomb : MonoBehaviour
{
    bool isGrabbing = false; //��ü�� ��� �ִ����� ����
    GameObject grabbedObject; // ��� �ִ� ��ü
    public LayerMask grabbedLayer; // ���� ��ü�� ����
    public float grabRange = 0.2f; // ���� �� �ִ� �Ÿ�

    Vector3 prevPos; //���� ��ġ
    public float throwPower = 20f; // ���� ��

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
        //[Grab] ��ư�� ������ ���� ���� �ȿ� �ִ� ��ź�� ���´�.
        if (ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            if(isRemoteGrab) //���Ÿ� ��ü��⸦ ����Ѵٸ�
            {
                //�� �������� Ray ����
                Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
                RaycastHit hitInfo;
                //SphereCast�� �̿��� ��ü �浹�� üũ
                if (Physics.SphereCast(ray, 0.5f, out hitInfo, remoteGrabDistance, grabbedLayer))
                {
                    isGrabbing = true; //���� ���·� ��ȯ
                    grabbedObject = hitInfo.transform.gameObject; // ���� ��ü�� ���� ���
                    StartCoroutine(GrabbingAnimation()); //��ü�� �������� ��� ����
                }
                return;
            }
            //���� ���� �ȿ� �ִ� ��� ��ź ����
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange,
                grabbedLayer);
            int closest = 0; //���� ����� ��ź �ε���
            //�հ� ���� ����� ��ü ����
            for(int i=0; i<hitObjects.Length; i++)
            {
                //�հ� ���� ����� ��ü���� �Ÿ�
                Vector3 colsestPos = hitObjects[closest].transform.position;
                float closestDistance = Vector3.Distance(colsestPos, ARAVRInput.RHandPosition);
                //���� ��ü�� ���� �Ÿ�
                Vector3 nextPos = hitObjects[i].transform.position;
                float nextDistance = Vector3.Distance(nextPos, ARAVRInput.RHandPosition);
                //���� ��ü���� �Ÿ��� �� �����ٸ�
                if(nextDistance < closestDistance)
                {
                    closest = i; //���� ����� ��ü �ε��� ��ü
                }
            }

            //����� ��ü�� ���� ���
            if(hitObjects.Length > 0)
            {
                isGrabbing = true; //���� ���·� ��ȯ
                grabbedObject = hitObjects[closest].gameObject; // ���� ��ü�� ���� ���
                grabbedObject.transform.parent = ARAVRInput.RHand; //���� ��ü�� ���� �ڽ����� ���
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                prevPos = ARAVRInput.RHandPosition; //�ʱ� ��ġ�� ����
                prevRot = ARAVRInput.RHand.rotation; //�ʱ� ȸ���� ����
            }
        }
    }
    void TryUngrab()
    {
        //Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos); //���� ����
        
        prevPos = ARAVRInput.RHandPosition;
        //���ʹϿ� ����
        //angle1 = Q1. angle2 = Q2
        //-angle1 + angle2 = Q1 * Q2
        //-angle2 = Quaternion.Inverse(Q2)
        //angle2-angle1 = Quaternion.FromToRatation(Q1, Q2) = Q2*Quaterniton.Inverse(Q1)
        //ȸ�� ���� = current - previous�� ���� ����. -previous�� Inverse�� ����.
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        //���� ȸ�� ����
        prevRot = ARAVRInput.RHand.rotation;
        //��ư�� ���Ҵٸ�..
        if (ARAVRInput.GetUp(ARAVRInput.Button.IndexTrigger, ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false; //���� ���� ���·� ��ȯ
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false; // ���� ��� Ȱ��ȭ
            grabbedObject.transform.parent = null; // �տ��� ��ź �����

            // grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower; //VR������
             grabbedObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwPower; //PC������
            //���ӵ� = (1/dt) * d��(Ư�� �� ���� ���� ����)
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f/Time.deltaTime)*angle*axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            grabbedObject = null; //���� ��ü�� ������ ����
            
        }
    }
    IEnumerator GrabbingAnimation() //�� ������ ���� ������ �Լ�
    {
        grabbedObject.GetComponent<Rigidbody>().isKinematic = true; //���� ��� ����
        prevPos = ARAVRInput.RHandPosition; //�ʱ� ��ġ �� ����
        prevRot = ARAVRInput.RHand.rotation; //�ʱ� ȸ�� �� ����
        Vector3 startLocation = grabbedObject.transform.position; 
        Vector3 targetLocation = ARAVRInput.RHandPosition + ARAVRInput.RHandDirection * 0.1f;

        float currentTime = 0;
        float finishTime = 0.2f;
        //�����
        float elapsedRate = currentTime / finishTime;
        while(elapsedRate < 1)
        {
            currentTime += Time.deltaTime;
            elapsedRate = currentTime / finishTime;
            grabbedObject.transform.position = Vector3.Lerp(startLocation, targetLocation, elapsedRate);
            yield return null;
        }
        //���� ��ü�� ���� �ڽ����� ���
        grabbedObject.transform.position = targetLocation;
        grabbedObject.transform.parent = ARAVRInput.RHand;
    }
}
