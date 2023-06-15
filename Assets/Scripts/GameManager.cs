using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수


    public enum GameState
    {
        Ready,
        Run,
        Pause,
        ShoppingWall,
        GameOver
    }
    public GameState gState; //게임 상태 상수

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
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (Instance != this)
        {
            // 자신을 파괴
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
