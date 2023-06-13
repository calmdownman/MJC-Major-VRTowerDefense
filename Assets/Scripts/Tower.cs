
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public static Tower Instance; //�̱��� ��ü

    public Transform damageUI;
    public Image damageImage;
    public int initialHP = 10; //Ÿ���� ���� ü�� 
    int _hp = 0; //���� ü��
    public float damageTime = 0.1f;
    public int HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
            StopAllCoroutines();
            StartCoroutine(DamageEvent()); //���ڰŸ��� �ǰ� ó���� �ڷ�ƾ �Լ� ����
            
            if (_hp <= 0)
            {
                //Destroy(gameObject); //������ �Ѿ��
            }
        }
    }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        //�̱��� ��ü �� �Ҵ�
    }

    // Start is called before the first frame update
    void Start()
    {
        _hp= initialHP;
        //ī�޶��� nearClipPlane ���� ����صд�
        float z = Camera.main.nearClipPlane + 0.01f;
        damageUI.SetParent(Camera.main.transform); //damageUI �θ� ���� ī�޶��
        damageUI.localPosition = new Vector3(0,0,z);
        damageImage.enabled= false; //ó������ ��Ȱ��ȭ
    }

    IEnumerator DamageEvent()
    {
        damageImage.enabled= true; //�ǰ� UI ����
        yield return new WaitForSeconds(damageTime);
        damageImage.enabled= false; //�ǰ� UI ����
    }
}
