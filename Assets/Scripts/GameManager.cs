using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����


    public enum GameState
    {
        Ready,
        Run,
        Pause,
        ShoppingWall,
        GameOver
    }
    public GameState gState; //���� ���� ���

    [Header("#Player Info")]
    private int gameLevel = 1;
    public int kill = 0;

    public int GameLevel
    {
        set { gameLevel = value; }
        get { return gameLevel;}
    }

    void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (Instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
        gameLevel = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Ready;
        GameLevel = 1;
        kill = 0;
        Time.timeScale = 1f;
        gState = GameState.Run;
    }
}
