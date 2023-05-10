using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    CharacterController cc;
    public float gravity = -20f;
    float yVelocity = 0;
    public float jumpPower = 5f;
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //����� �Է��� �޴´�.
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");
        //������ �����
        Vector3 dir = new Vector3(h,0,v);
        //����ڰ� �ٶ󺸴� �������� �Է� �� ��ȭ��Ű��
        dir = Camera.main.transform.TransformDirection(dir);

        //�߷� ������ ���� ���� ���� �߰�
        yVelocity += gravity * Time.deltaTime;
        if(cc.isGrounded)
        {
            yVelocity = 0;
        }
        if(ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch)){
            yVelocity = jumpPower;
        }
        dir.y = yVelocity;
        //�̵��Ѵ�
        cc.Move(dir * speed * Time.deltaTime);
    }
}
