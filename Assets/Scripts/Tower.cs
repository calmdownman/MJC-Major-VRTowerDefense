
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public static Tower Instance; //�̱��� ��ü

    public Transform damageUI;
    public Image damageImage;
    public GameObject damageText;
    public GameObject gameOverUI;
    public int maxHP = 10; //Ÿ���� ���� ü�� 
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
                gameOverUI.SetActive(true);
                GameManager.Instance.gState = GameManager.GameState.GameOver; //������ �Ѿ��
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
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
        gameOverUI.SetActive(false);
        maxHP = 10;
        _hp = maxHP;
        //ī�޶��� nearClipPlane ���� ����صд�
        float z = Camera.main.nearClipPlane + 0.01f;
        damageUI.SetParent(Camera.main.transform); //damageUI �θ� ���� ī�޶��
        damageUI.localPosition = new Vector3(0,0,z);
        damageImage.enabled= false; //ó������ ��Ȱ��ȭ
    }

    IEnumerator DamageEvent()
    {
        damageText.SetActive(true);
        damageImage.enabled= true; //�ǰ� UI ����
        yield return new WaitForSeconds(damageTime);
        damageText.SetActive(false);
        damageImage.enabled= false; //�ǰ� UI ����
        UIManager.Instance.UpdateTowerHP(_hp, maxHP);
    }
}
