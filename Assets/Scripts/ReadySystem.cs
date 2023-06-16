using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadySystem : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.gState != GameManager.GameState.Ready)
        {
            Debug.Log("ESC");
            GameManager.Instance.gState = GameManager.GameState.Ready;
            UIManager.Instance.SetActiveReadyUI(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
    }
}
