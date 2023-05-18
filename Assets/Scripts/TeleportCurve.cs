using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportCurve : MonoBehaviour
{
    public Transform teleportCircleUI; //�ڷ����� ǥ�� UI
    LineRenderer lr; //���� ������
    Vector3 oringinScale = Vector3.one * 0.02f; //���� �ڷ���Ʈ UI ũ��
    public int lineSmooth = 40;// Ŀ���� �ε巯�� ����
    public float curveLength = 50f; //Ŀ���� ����
    public float gravity = -60f; //Ŀ���� �߷�
    public float simulateTime = 0.02f; //� �ùķ��̼��� ���� �� �ð�
    List<Vector3> lines = new List<Vector3>(); //��� �̷�� ����

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
        //���� ��Ʈ�ѷ��� One��ư�� ������ ��
        if(ARAVRInput.GetDown(ARAVRInput.Button.One,ARAVRInput.Controller.LTouch))
        {
            lr.enabled = true; //���η����� Ȱ��ȭ
        }
        //���� ��Ʈ�ѷ��� One��ư���� ���� ����
        else if (ARAVRInput.GetUp(ARAVRInput.Button.One, ARAVRInput.Controller.LTouch))
        {
            lr.enabled = false; //���η����� ��Ȱ��ȭ
            //�ڷ����� UI�� Ȱ��ȭ �Ǿ� �ִٸ�...
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
        lines.RemoveRange(0, lines.Count); //����Ʈ�� ��� �����
        //���� ����� ������ ���Ѵ�
        Vector3 dir = ARAVRInput.LHandDirection * curveLength;
        //���� �׷��� ��ġ�� �ʱⰪ ����
        Vector3 pos = ARAVRInput.LHandPosition;
        //���� ��ġ�� ����Ʈ�� ��´�

        for(int i=0; i<lineSmooth; i++) //lineSmooth ������ŭ �ݺ�
        {
            Vector3 lastPos = pos; //���� ��ġ ���
            //�߷��� ������ �ӵ� ���
            //�̷��ӵ�(v) = ����ӵ�(v0) = ���ӵ�(a) * �ð�(simulateTime)
            dir.y += gravity * simulateTime;
            //��� ����� ���� ��ġ ���
            //�̷� ��ġ(p) = ������ġ(p0) + �ӵ�(v) * �ð�(simulateTime)
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
        //����(lastPos)���� ������(pos)�� ���ϴ� ���� ���
        Vector3 rayDir = pos - lastPos;
        Ray ray = new Ray(lastPos,rayDir);
        RaycastHit hitInfo;
        //Raycast �� �� ������ ũ�⸦ �� ���� ���� �� ������ �Ÿ��� ����
        if(Physics.Raycast(ray, out hitInfo, rayDir.magnitude))
        {
            //���� ���� ��ġ�� �浹�� �������� ����
            pos = hitInfo.point;
            int layer = LayerMask.NameToLayer("Terrain");
            if(hitInfo.transform.gameObject.layer == layer) 
            {
                teleportCircleUI.gameObject.SetActive(true); //�ڷ���Ʈ UIȰ��ȭ
                teleportCircleUI.position = pos; //��ġ ����
                teleportCircleUI.forward = hitInfo.normal; //���� ����
                float distance = (pos - ARAVRInput.LHandPosition).magnitude;
                //�ڷ���Ʈ UI�� ���� ũ�� ����
                teleportCircleUI.localScale= oringinScale * Mathf.Max(1, distance);
            }
            return true;
        }
        return false;
    }
}
