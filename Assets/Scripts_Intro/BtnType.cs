using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    bool isSound;

    public TextMeshProUGUI tmpUGUI;
    SoundManager audioSource;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
        tmpUGUI = GetComponentInChildren<TextMeshProUGUI>();
        audioSource = FindObjectOfType<SoundManager>();
        isSound = true;
    }
    public void OnBtnClick()
    {
        switch(currentType)
        {
            case BTNType.Start:
                SceneManager.LoadScene(1);
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
                break;
            case BTNType.Sound:
                if(isSound)
                {
                    tmpUGUI.text = "Sound On";
                    audioSource.bgm.Pause();
                    Debug.Log("사운드OFF");
                    isSound = false;
                }
                else
                {
                    tmpUGUI.text = "Sound Off";
                    audioSource.bgm.Play();
                    Debug.Log("사운드ON");
                    isSound = true;
                }
                break;
            case BTNType.Back:
                CanvasGroupOn(mainGroup);
                CanvasGroupOff(optionGroup);
                break;
            case BTNType.Quit:
                Application.Quit();
                break;
            case BTNType.Restart:
                SceneManager.LoadScene(1);
                break;
            case BTNType.RetunMain:
                SceneManager.LoadScene(0);
                break;
            case BTNType.Resume:
                UIManager.Instance.SetActiveReadyUI(false); //일시정지창 닫기
                Time.timeScale = 1;
                GameManager.Instance.gState = GameManager.GameState.Run;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup group)
    {
        group.alpha = 1;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    public void CanvasGroupOff(CanvasGroup group)
    {
        group.alpha = 0;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
