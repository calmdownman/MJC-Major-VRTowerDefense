using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    CharacterController cc;
    public float gravity = -20;
    float yVelocity = 0;
    public float jumpPower = 5f;
    public Transform towerPos;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cc.transform.position.y < 2 && GameManager.Instance.gState == GameManager.GameState.Run)
        {
            Debug.Log("������");
            UIManager.Instance.SetActiveCauitionUI(true);
            
            //����� �Է��� �޴´�.
            if (Input.GetKeyUp(KeyCode.R))
            {
                UIManager.Instance.SetActiveCauitionUI(false);
                cc.enabled = false;
                transform.position = towerPos.position;
                cc.enabled = true;
            }
        }

        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");
        //������ �����.
        Vector3 dir = new Vector3(h, 0, v);
        //����ڰ� �ٶ󺸴� �������� �Է� �� ��ȭ��Ű��
        dir = Camera.main.transform.TransformDirection(dir);

        //�߷� ������ ���� ���� ���� �߰�
        yVelocity += gravity * Time.deltaTime;
        if(cc.isGrounded)
        {
            yVelocity = 0;
        }
        /*if(ARAVRInput.GetDown(ARAVRInput.Button.Two, ARAVRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }*/
        dir.y = yVelocity;
        //�̵�
        cc.Move(dir * speed * Time.deltaTime); //�̵��Ѵ�
    }
}
