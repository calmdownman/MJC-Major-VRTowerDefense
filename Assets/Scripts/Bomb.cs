using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //폭발 효과
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;
    //폭발 영역
    public float range = 5;
    // Start is called before the first frame update
    void Start()
    {
        //씬에서 Explision 객체를 찾아 transform 정보 가져오기
        explosion = GameObject.Find("Explosion").transform;
        expEffect = explosion.GetComponent<ParticleSystem>();
        expAudio = explosion.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Drone");
        Collider[] drones = Physics.OverlapSphere(transform.position, range, layerMask);
        foreach (Collider drone in drones)
        {
            UIManager.Instance.UpdateKillText(++GameManager.Instance.kill);
            UIManager.Instance.UpdateDroneCnt(--UIManager.Instance.DroneCnt);
            Destroy(drone.gameObject);
        }
        explosion.position = transform.position; //폭발 효과의 위치 지정
        expEffect.Play(); //이첵트 재생
        expAudio.Play(); //이펙트 사운드 재생
        Destroy(gameObject); //폭탄 없애기
    }
}
