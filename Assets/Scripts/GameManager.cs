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
    public int wallIndex;

    public GameState gState; //게임 상태 상수

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gState = GameState.Run;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
