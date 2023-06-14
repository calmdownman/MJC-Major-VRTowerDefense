using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManger : MonoBehaviour
{
    //���� �ð��� ����
    private readonly float minTime = 1;
    private readonly float[] maxTime= {5f,4f,3f,2.5f,2f,1.5f};
    float createTime; //���� �ð�
    float currentTime; //��� �ð�
    public Transform[] spawnPoints; //��� ���� ��ġ
    public GameObject[] droneFactory; //��� ����
    // Start is called before the first frame update
    void Start()
    {
        createTime = Random.Range(minTime, maxTime[GameManager.Instance.GameLevel-1]); //���� �ð��� ���� �������� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gState != GameManager.GameState.Run) return;

        currentTime += Time.deltaTime;
        if (currentTime > createTime) //��� �ð��� �����ð��� �ʰ��Ͽ��ٸ�...
        { 
            GameObject drone = Instantiate(droneFactory[Random.Range(0,2)]); //��� ����
            int index = Random.Range(0, spawnPoints.Length); //��� ��ġ ���� ����
            drone.transform.position = spawnPoints[index].position;
            UIManager.Instance.UpdateDroneCnt(++UIManager.Instance.DroneCnt);
            currentTime = 0; //��� �ð� �ʱ�ȭ
            createTime = Random.Range(minTime, maxTime[GameManager.Instance.GameLevel - 1]); //���� �ð� ���Ҵ�
        }
    }
}
