using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneManger : MonoBehaviour
{
    //랜덤 시간의 범위
    public float minTime = 1;
    public float maxTime = 5;
    float createTime; //생성 시간
    float currentTime; //경과 시간
    public Transform[] spawnPoints; //드론 생성 위치
    public GameObject[] droneFactory; //드론 공장
    // Start is called before the first frame update
    void Start()
    {
        createTime = Random.Range(minTime, maxTime); //생성 시간을 랜덤 범위에서 설정
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > createTime) //경과 시간이 생성시간을 초과하였다면...
        { 
            GameObject drone = Instantiate(droneFactory[Random.Range(0,2)]); //드론 생성
            int index = Random.Range(0, spawnPoints.Length); //드론 위치 랜덤 설정
            drone.transform.position = spawnPoints[index].position;
            currentTime = 0; //경과 시간 초기화
            createTime = Random.Range(minTime, maxTime); //생성 시간 재할당
        }
    }
}
