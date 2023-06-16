using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // 싱글톤 접근용 프로퍼티
    public static UIManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }

            return m_instance;
        }
    }

    private static UIManager m_instance; // 싱글톤이 할당될 변수

    [Header("게임 오브젝트 모음")]
    public GameObject pauseWindow;
    public GameObject txt_Caution;
    public GameObject alert_NS;
    public GameObject alert_Tower;
    public Slider towerHpBar;
    public GameObject readySystem;

    [Header("텍스트 모음")]
    public Text txt_Level;
    public Text txt_Kill;
    public Text txt_GameTime;
    public Text txt_DroneCnt;
    public Text txt_SelectItem;
    public Text txt_RemainCount;
    public Text txt_NoChoice;
    public Text txt_Damage;

    [Header("숫자 조절")]
    public int maxGameTime;
    
    private int wallIndex;
    private int droneCnt;

    public int WallIndex
    {
        set { wallIndex = value; }
        get { return wallIndex; }
    }
    public int DroneCnt
    {
        set { droneCnt = value; }
        get { return droneCnt; }
    }

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (Instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        maxGameTime = 5;
        droneCnt = 0;
        pauseWindow.SetActive(false);
        txt_Caution.SetActive(false);
        alert_NS.SetActive(false);
        StartTimer();
    }

    public void StartTimer() 
    {
        StartCoroutine(Timer(maxGameTime));
    }

    public void StartNoChoice()
    {
        StopCoroutine(NoChoice());
        StartCoroutine(NoChoice());
    }

    IEnumerator NoChoice()
    {
        txt_NoChoice.color = new Color(txt_NoChoice.color.r, txt_NoChoice.color.g, txt_NoChoice.color.b, 1);
        
        while (txt_NoChoice.color.a > 0.0f)
        {

            txt_NoChoice.color = new Color(txt_NoChoice.color.r, txt_NoChoice.color.g, txt_NoChoice.color.b, txt_NoChoice.color.a - (Time.deltaTime / 1f));
            yield return null;
        } 
    }

    IEnumerator Timer(int maxTime)
    {
        while (maxTime > 0) 
        {
            yield return new WaitForSeconds(1);
            UpdateTime(--maxTime);
        }
        pauseWindow.SetActive(true);
        GameManager.Instance.gState = GameManager.GameState.Pause;
    }

    public void UpdateDroneCnt(int cnt)
    {
        txt_DroneCnt.text = $"현재 남은 드론 수 : {cnt}대";
        if (DroneCnt <= 0)
        {
            alert_NS.SetActive(true);
        }
    }

    public void UpdateTime(int time)
    {
        txt_GameTime.text = time.ToString("D2");
    }

    public void UpdateDamage()
    {
        txt_Damage.text = $"DMG : {GameManager.Instance.playerDamage}";
    }

    public void UpdateKillText(int kill)
    {
        txt_Kill.text = kill.ToString();
    }

    public void UpdateLvText()
    {
        txt_Level.text = $"LV. {GameManager.Instance.GameLevel}";
    }

    public void UpdateTowerHP(int hp,int maxHp )
    {
        towerHpBar.value = (float)hp/ (float)maxHp;
    }

    public void UpdateItemText(string item_Name)
    {
        txt_SelectItem.text = $"선택한 아이템 : '{item_Name}'";
    }

    public void UpdateRemainCntText(int cnt)
    {
        txt_RemainCount.text = $"남은 횟수 : {cnt}";
    }

    public void SetActiveCauitionUI(bool active)
    {
        txt_Caution.SetActive(active);
    }

    public void SetActivePauseUI(bool active)
    {
        pauseWindow.SetActive(active);
    }

    public void SetActiveReadyUI(bool active)
    {
        readySystem.SetActive(active);
    }

}
