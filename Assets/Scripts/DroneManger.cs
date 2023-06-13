using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManger : MonoBehaviour
{
    //���� �ð��� ����
    public float minTime = 1;
    public float maxTime = 5;
    float createTime; //���� �ð�
    float currentTime; //��� �ð�
    public Transform[] spawnPoints; //��� ���� ��ġ
    public GameObject[] droneFactory; //��� ����
    // Start is called before the first frame update
    void Start()
    {
        createTime = Random.Range(minTime, maxTime); //���� �ð��� ���� �������� ����
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > createTime) //��� �ð��� �����ð��� �ʰ��Ͽ��ٸ�...
        { 
            GameObject drone = Instantiate(droneFactory[Random.Range(0,2)]); //��� ����
            int index = Random.Range(0, spawnPoints.Length); //��� ��ġ ���� ����
            drone.transform.position = spawnPoints[index].position;
            currentTime = 0; //��� �ð� �ʱ�ȭ
            createTime = Random.Range(minTime, maxTime); //���� �ð� ���Ҵ�
        }
    }
}
