using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    private int remain_Count;
    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.gState != GameManager.GameState.Pause || UIManager.Instance.DroneCnt > 0) return;
        
        if(Input.GetKeyDown(KeyCode.Alpha1) && remain_Count > 0)
        {
            remain_Count--;
            UIManager.Instance.WallIndex = 0;
            UIManager.Instance.UpdateItemText("수평 벽");
            UIManager.Instance.UpdateRemainCntText(remain_Count);
            GameManager.Instance.gState = GameManager.GameState.ShoppingWall;
        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2) && remain_Count > 0)
        {
            remain_Count--;
            UIManager.Instance.WallIndex = 1;
            UIManager.Instance.UpdateItemText("수직 벽");
            UIManager.Instance.UpdateRemainCntText(remain_Count);
            GameManager.Instance.gState = GameManager.GameState.ShoppingWall;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && remain_Count > 0)
        {
            if (UIManager.Instance.maxGameTime < 30) 
            {
                UIManager.Instance.StartNoChoice();
                return;
            }

            remain_Count--;
            UIManager.Instance.maxGameTime -= 5;
            UIManager.Instance.UpdateItemText("시간 단축");
            UIManager.Instance.UpdateRemainCntText(remain_Count); 
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && remain_Count > 0)
        {
            remain_Count--;
            UIManager.Instance.UpdateItemText("타워 회복");
            UIManager.Instance.UpdateRemainCntText(remain_Count);
            Tower.Instance.HP = Tower.Instance.maxHP;
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

    private void OnEnable()
    {
        remain_Count = GameManager.Instance.GameLevel;
        UIManager.Instance.UpdateRemainCntText(remain_Count);
        transform.GetChild(0).gameObject.SetActive(false);
        UIManager.Instance.UpdateItemText("없음");
    }
}
