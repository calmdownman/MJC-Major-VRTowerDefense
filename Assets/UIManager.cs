using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseWindow;

    public void OnClickHorizonWall()
    {
        GameManager.Instance.wallIndex = 0;
    }

    public void OnClickVerticalWall()
    {
        GameManager.Instance.wallIndex = 1;
    }

    private void Update()
    {
       if(GameManager.Instance.gameTime <= 0)
        {
            pauseWindow.SetActive(true);
        }
    }
}
