using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportStraight : MonoBehaviour
{
    public Transform teleportCircleUI;
    LineRenderer lr;
    Vector3 originScale = Vector3.one* 0.02f; //������ �ڷ���Ʈ UI ũ��
    // Start is called before the first frame update
    void Start()
    {
        //������ �� ��Ȱ��ȭ
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>(); //���� ������ ������Ʈ
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
                //�ڷ���Ʈ UI��ġ�� ���� �̵�
                transform.position = teleportCircleUI.position + Vector3.up;
                GetComponent<CharacterController>().enabled = true;
            }
            teleportCircleUI.gameObject.SetActive(false);
        }

        //���� ��Ʈ�ѷ��� One ��ư�� �����ٸ�
        else if (ARAVRInput.Get(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if(Physics.Raycast(ray, out hitInfo, 200, layer)) 
            {
                //Ray�� �ε��� ������ ���� �׸���
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitInfo.point);
                //�ε��� ������ �ڷ���Ʈ UI ǥ��
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                //�ڷ���Ʈ UI�� ���� �����ֵ��� ���� ����
                teleportCircleUI.forward = hitInfo.normal;
                //�ڷ����� UI�� ũ�Ⱑ �Ÿ��� ���� �����ǵ��� ����
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitInfo.distance);
            } 
            else
            {//Ray �浹�� �߻����� ������...
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }
    }
}
