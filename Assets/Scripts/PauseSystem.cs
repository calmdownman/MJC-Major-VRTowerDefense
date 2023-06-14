using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gState != GameManager.GameState.Pause || UIManager.Instance.DroneCnt > 0) return;
        
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.WallIndex = 0;
            GameManager.Instance.gState = GameManager.GameState.Shopping;
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.Instance.WallIndex = 1;
            GameManager.Instance.gState = GameManager.GameState.Shopping;
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            this.gameObject.SetActive(false);
            GameManager.Instance.GameLevel++;
            UIManager.Instance.UpdateLvText();
            GameManager.Instance.gState = GameManager.GameState.Run;
            UIManager.Instance.StartTimer();
        }
    }
}
