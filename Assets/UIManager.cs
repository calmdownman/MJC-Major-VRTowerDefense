using UnityEngine;

public class UIManager : MonoBehaviour
{
    void OnClickHorizonWall()
    {
        GameManager.Instance.wallIndex = 0;
    }

    void OnClickVerticalWall()
    {
        GameManager.Instance.wallIndex = 1;
    }
}
