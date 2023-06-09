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
            Debug.Log("떨어짐");
            UIManager.Instance.SetActiveCauitionUI(true);
            
            //사용자 입력을 받는다.
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
        //방향을 만든다.
        Vector3 dir = new Vector3(h, 0, v);
        //사용자가 바라보는 방향으로 입력 값 변화시키기
        dir = Camera.main.transform.TransformDirection(dir);

        //중력 적용을 위한 수직 방향 추가
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
        //이동
        cc.Move(dir * speed * Time.deltaTime); //이동한다
    }
}
