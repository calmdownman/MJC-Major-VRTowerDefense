using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletImpact; //총알 파편 효과
    ParticleSystem bulletEffect;//총알 파편 파티클 시스템
    AudioSource bulletAudio; // 총알 발사 사운드
    public Transform crosshair;
    // Start is called before the first frame update
    void Start()
    {
        bulletEffect = bulletImpact.GetComponent<ParticleSystem>();
        bulletAudio = bulletImpact.GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked; //마우스 커서 잠금
    }

    // Update is called once per frame
    void Update()
    {
        ARAVRInput.DrawCrosshair(crosshair);
        //사용자가 IndexTrigger 버튼을 누르면
        if(ARAVRInput.GetDown(ARAVRInput.Button.IndexTrigger))
        {
            //컨트롤러의 진동 재생
            ARAVRInput.PlayVibration(ARAVRInput.Controller.RTouch);
            //총알 오디오 재생
            bulletAudio.Stop();
            bulletAudio.Play();

            Ray ray = new Ray(ARAVRInput.RHandPosition, ARAVRInput.RHandDirection);
            RaycastHit hitInfo;
            int playerLayer = 1 << LayerMask.NameToLayer("Player");
            int towerLayer = 1 << LayerMask.NameToLayer("Tower");
            int layerMask = playerLayer | towerLayer;
            if (Physics.Raycast(ray, out hitInfo, 200, ~layerMask))
            {
                //총알 이펙트 진행되고 있으면 멈추고 재생
                bulletEffect.Stop();
                bulletEffect.Play();
                //부딪힌 지점 바로 위에서 이펙트가 보이도록 설정
                bulletImpact.position = hitInfo.point;
                //부딪힌 지점의 방향으로 총알 이펙트의 방향을 설정
                bulletImpact.forward = hitInfo.normal;
                if(hitInfo.transform.name.Contains("Drone")) //ray에 부딪힌 객체가 drone이라면
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
