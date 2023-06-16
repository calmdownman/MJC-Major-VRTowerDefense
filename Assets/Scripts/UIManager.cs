using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // �̱��� ���ٿ� ������Ƽ
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

    private static UIManager m_instance; // �̱����� �Ҵ�� ����

    [Header("���� ������Ʈ ����")]
    public GameObject pauseWindow;
    public GameObject txt_Caution;
    public GameObject alert_NS;
    public GameObject alert_Tower;
    public Slider towerHpBar;
    public GameObject readySystem;

    [Header("�ؽ�Ʈ ����")]
    public Text txt_Level;
    public Text txt_Kill;
    public Text txt_GameTime;
    public Text txt_DroneCnt;
    public Text txt_SelectItem;
    public Text txt_RemainCount;
    public Text txt_NoChoice;
    public Text txt_Damage;

    [Header("���� ����")]
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
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (Instance != this)
        {
            // �ڽ��� �ı�
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
        txt_DroneCnt.text = $"���� ���� ��� �� : {cnt}��";
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
        txt_SelectItem.text = $"������ ������ : '{item_Name}'";
    }

    public void UpdateRemainCntText(int cnt)
    {
        txt_RemainCount.text = $"���� Ƚ�� : {cnt}";
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
