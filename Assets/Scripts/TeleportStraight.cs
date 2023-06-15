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
    Vector3 originScale = Vector3.one * 0.02f; //���� �ڷ���Ʈ UIũ��
    /*public bool isWarp = false; //���� ��� ����
    public float warpTime = 0.1f; //������ �ɸ��� �ð�
    public PostProcessVolume post; // ����ϰ� �ִ� ����Ʈ ���μ��� ���� ������Ʈ*/
 
    void Start()
    {
        //�����Ҷ� ��Ȱ��ȭ
        teleportCircleUI.gameObject.SetActive(false);
        lr = GetComponent<LineRenderer>(); //���� ������ ������Ʈ 
    }

    void Update()
    {
        //�������� Ŭ���� �� ���� �Ͻ����� �� �۵� 
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
                /*if(isWarp == false) //���� ��� ����� �ƴ� �� ���� �̵�
                {
                    GetComponent<CharacterController>().enabled = false;
                    //�ڷ���Ʈ UI��ġ�� ���� �̵�
                    transform.position = teleportCircleUI.position + Vector3.up;
                    GetComponent<CharacterController>().enabled = true;
                }
                else // ���� ��� ����� ���� Warp() �ڷ�ƾ �Լ� ȣ��
                {
                    StartCoroutine(Warp());
                }*/
                Instantiate(obstacles[UIManager.Instance.WallIndex], teleportCircleUI.position, obstacles[UIManager.Instance.WallIndex].transform.rotation);
                UIManager.Instance.UpdateItemText("����");
                GameManager.Instance.gState = GameManager.GameState.Pause;
            }

            teleportCircleUI.gameObject.SetActive(false);
        }
        //���� ��Ʈ�ѷ��� One��ư�� �����ٸ�
        else if (ARAVRInput.Get(ARAVRInput.Button.HandTrigger, ARAVRInput.Controller.LTouch))
        {
            Ray ray = new Ray(ARAVRInput.LHandPosition, ARAVRInput.LHandDirection);
            RaycastHit hitInfo;
            int layer = 1 << LayerMask.NameToLayer("Terrain");
            if (Physics.Raycast(ray, out hitInfo, 200, layer))
            {
                //Ray�� �ε��� ������ ���� �׸���
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, hitInfo.point);
                //�ε��� ������ �ڷ���Ʈ UI ǥ��
                teleportCircleUI.gameObject.SetActive(true);
                teleportCircleUI.position = hitInfo.point;
                //�ڷ���Ʈ UI�� ���� �����ֵ��� ���� ����
                teleportCircleUI.forward = hitInfo.normal;
                //�ڷ���Ʈ UI�� ũ�Ⱑ �Ÿ��� ���� �����ǵ��� ����
                teleportCircleUI.localScale = originScale * Mathf.Max(1, hitInfo.distance);
            }
            else { //Ray �浹�� �߻����� ������..
                lr.SetPosition(0, ray.origin);
                lr.SetPosition(1, ray.origin + ARAVRInput.LHandDirection * 200);
                teleportCircleUI.gameObject.SetActive(false);
            }
        }

    }
    /*IEnumerator Warp()
    {
        MotionBlur blur; // ���� ������ ǥ���ϴ� ��Ǻ�
        Vector3 pos = transform.position; // ���� ������
        Vector3 targetPos = teleportCircleUI.position + Vector3.up; //������(���� ����)
        float currentTime = 0; // ���� ��� �ð�
        post.profile.TryGetSettings<MotionBlur>(out blur); // ����Ʈ ���μ��̿��� ��� ���� �������� ������
        blur.active = true; //���� ���� �� �� �ѱ�
        GetComponent<CharacterController>().enabled = false; //�÷��̾� ������ X

        //��� �ð��� �������� ª�� �ð� ���� �̵� ó��
        while (currentTime < warpTime)
        {
            currentTime += Time.deltaTime; //��� �ð� �帣�� �ϱ����
            //���� ���������� �������� �����ϱ� ���� ���� �ð� ���� �̵�
            transform.position = Vector3.Lerp(pos, targetPos, currentTime / warpTime);
            yield return null; //�ڷ�ƾ ���
        }
        transform.position = teleportCircleUI.position + Vector3.up; //�ڷ���Ʈ ��ġ�� �̵�
        GetComponent<CharacterController>().enabled = true; // ĳ���� ��Ʈ�ѷ� �ٽ� �ѱ�
        blur.active = false; //����Ʈ ȿ�� 
    }*/
}
