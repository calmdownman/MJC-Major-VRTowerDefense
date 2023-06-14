using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameComplete,
        GameOver
    }

    [Header("#Game Control")]
    public float gameTime;
    public float maxGameTime = 20f;
    public int wallIndex;

    private int gameLevel;
    public int GameLevel
    {
        set { gameLevel = value; }
        get { return gameLevel;}
    }

    public GameState gState; //게임 상태 상수


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        gameLevel = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Run;
    }
}
