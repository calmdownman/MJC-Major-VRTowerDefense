using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //���� ȿ��
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;
    //���� ����
    public float range = 5;
    // Start is called before the first frame update
    void Start()
    {
        //������ Explision ��ü�� ã�� transform ���� ��������
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
        explosion.position = transform.position; //���� ȿ���� ��ġ ����
        expEffect.Play(); //��ýƮ ���
        expAudio.Play(); //����Ʈ ���� ���
        Destroy(gameObject); //��ź ���ֱ�
    }
}
