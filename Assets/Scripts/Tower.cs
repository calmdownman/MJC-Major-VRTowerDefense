
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public static Tower Instance; //싱글톤 객체

    public Transform damageUI;
    public Image damageImage;
    public GameObject damageText;
    public GameObject gameOverUI;
    public int maxHP = 10; //타워의 최초 체력 
    int _hp = 0; //내부 체력
    public float damageTime = 0.1f;
    public int HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
            StopAllCoroutines();
            StartCoroutine(DamageEvent()); //깜박거리는 피격 처리할 코루틴 함수 실행
            
            if (_hp <= 0)
            {
                gameOverUI.SetActive(true);
                GameManager.Instance.gState = GameManager.GameState.GameOver; //엔딩씬 넘어가기
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
            }
        }
    }

    private void Awake()
    {
        if(Instance == null) Instance = this;
        //싱글톤 객체 값 할당
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOverUI.SetActive(false);
        maxHP = 10;
        _hp = maxHP;
        //카메라의 nearClipPlane 값을 기억해둔다
        float z = Camera.main.nearClipPlane + 0.01f;
        damageUI.SetParent(Camera.main.transform); //damageUI 부모를 메인 카메라로
        damageUI.localPosition = new Vector3(0,0,z);
        damageImage.enabled= false; //처음에는 비활성화
    }

    IEnumerator DamageEvent()
    {
        damageText.SetActive(true);
        damageImage.enabled= true; //피격 UI 실행
        yield return new WaitForSeconds(damageTime);
        damageText.SetActive(false);
        damageImage.enabled= false; //피격 UI 해제
        UIManager.Instance.UpdateTowerHP(_hp, maxHP);
    }
}
