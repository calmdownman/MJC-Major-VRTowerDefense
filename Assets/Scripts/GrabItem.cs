using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItem : MonoBehaviour
{
    bool isGrabbing = false; //��ü�� ��� �ִ����� ����
    GameObject grabbedObject; //��� �ִ� ��ü
    public LayerMask grabbedLayer; //���� ��ü�� ����
    public float grabRange = 0.2f; //���� �� �ִ� �Ÿ�
                                   // Start is called before the first frame update

    Vector3 prevPos; //���� ��ġ
    public float throwPower = 10; //���� ��

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
        //[Grab] ��ư�� ������ ���� ���� �ȿ� �ִ� ��ź�� ���´�
        if(ARAVRInput.GetDown(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.RTouch))
        {
            //���� ���� �ȿ� �ִ� ��� ��ź ����
            Collider[] hitObjects = Physics.OverlapSphere(ARAVRInput.RHandPosition, grabRange,
                grabbedLayer);
            int closest = 0; //���� ����� ��ź �ε���
            //�հ� ���� ����� ��ü ����
            for(int i = 0; i < hitObjects.Length; i++)
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
                grabbedObject = hitObjects[closest].gameObject; //���� ��ü�� ���� ���
                grabbedObject.transform.parent = ARAVRInput.RHand; //���� ��ü�� ���� �ڽ�����
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                prevPos = ARAVRInput.RHandPosition; //�ʱ� ��ġ�� ����
                prevRot = ARAVRInput.RHand.rotation; //�ʱ� ȸ���� ����
            }
        }
    }

    void TryUnGrab()
    {
        Vector3 throwDirection = (ARAVRInput.RHandPosition - prevPos); //���� ����
        prevPos = ARAVRInput.RHandPosition;
        Quaternion deltaRotation = ARAVRInput.RHand.rotation * Quaternion.Inverse(prevRot);
        prevRot = ARAVRInput.RHand.rotation;

        //��
        if(ARAVRInput.GetUp(ARAVRInput.Button.HandTrigger,ARAVRInput.Controller.RTouch))
        {
            isGrabbing = false; //���� ���� ���·� ��ȯ
            grabbedObject.GetComponent<Rigidbody>().isKinematic=false; //���� ��� Ȱ��ȭ
            grabbedObject.transform.parent = null; //�տ��� ��ź �����
            //grabbedObject.GetComponent<Rigidbody>().velocity = throwDirection * throwPower; //VR ������
            grabbedObject.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * throwPower; //PC ������
            float angle;
            Vector3 axis;
            deltaRotation.ToAngleAxis(out angle, out axis);
            Vector3 angularVelocity = (1.0f / Time.deltaTime) * angle * axis;
            grabbedObject.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
            grabbedObject = null; //���� ��ü�� ������ ����
            
        }
    }
}
