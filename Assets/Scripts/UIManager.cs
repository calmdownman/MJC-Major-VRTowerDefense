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

    public GameObject pauseWindow, txt_Caution, alert_NS,alert_Tower;
    public Text txt_Level, txt_Kill,txt_GameTime,txt_DroneCnt,txt_SelectItem,txt_RemainCount;
    public Text txt_NoChoiceCaution;
    public Slider towerHpBar;

    [Header("숫자 조절")]
    public int maxGameTime = 60;
    public float blinckTime = 0.5f;
    
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
        StartTimer();
        pauseWindow.SetActive(false);
        txt_Caution.SetActive(false);
        alert_NS.SetActive(false);
    }

    public void StartTimer() 
    {
        StartCoroutine(Timer(maxGameTime));
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
        txt_RemainCount.text = $"남은 횟수 : {cnt}'";
    }

    public void SetActiveCauitionUI(bool active)
    {
        txt_Caution.SetActive(active);
    }

    public void SetActivePauseUI(bool active)
    {
        pauseWindow.SetActive(active);
    }
}
