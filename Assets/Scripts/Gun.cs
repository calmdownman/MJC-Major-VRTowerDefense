using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact; //�Ѿ� ���� ȿ��
    ParticleSystem bulletEffect;//�Ѿ� ���� ��ƼŬ �ý���
    AudioSource bulletAudio; // �Ѿ� �߻� ����
    public Transform crosshair;
    // Start is called before the first frame update
    void Start()
    {
        bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        bulletAudio = bulletImpact.GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; //���콺 Ŀ�� ���
    }

    // Update is called once per frame
    void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);
        //����ڰ� IndexTrigger ��ư�� ������
        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            //��Ʈ�ѷ��� ���� ���
            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
            //�Ѿ� ����� ���
            bulletAudio.Stop();
            bulletAudio.Play();

            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;
            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;
            if (Physics.Raycast(ray, out hitInfo, 200, ~layerMask))
            {
                //�Ѿ� ����Ʈ ����ǰ� ������ ���߰� ���
                bulletEffect.Stop();
                bulletEffect.Play();
                //�ε��� ���� �ٷ� ������ ����Ʈ�� ���̵��� ����
                bulletImpact.position = hitInfo.point;
                //�ε��� ������ �������� �Ѿ� ����Ʈ�� ������ ����
                bulletImpact.forward = hitInfo.normal;
                if(hitInfo.transform.name.Contains("Drone")) //ray�� �ε��� ��ü�� drone�̶��
                {
                    DroneAI drone = hitInfo.transform.GetComponent<DroneAI>();
                    if(drone) 
                    { 
                        drone.OnDamageProcess();
                    }
                }
            }
        }
    }
}
