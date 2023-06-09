using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    //드론의 상태 상수 정의
    enum DroneState
    {
        Idle,
        Move, 
        Attack,
        Damage,
        Die 
    }
    DroneState state = DroneState.Idle; //초기 시작 상태를 Idle
    public float idleDelayTime = 2; //대기 상태의 지속 시간
    float currentTime; //경과 시간
    public float moveSpeed = 1; //이동 속도
    Transform tower; //타워 위치
    NavMeshAgent agent; //길 찾기를 수행할 내비게이션 메시 에이전트
    public float attackRange = 3; //공격 범위
    public float attackDelayTime = 2; //공격 지연 시간

    private void Start()
    {
        tower = GameObject.Find("Tower").transform; //타워 찾기
        agent = GetComponent<NavMeshAgent>(); //내비메쉬에이전트 컴포넌트
        agent.enabled = false; //비활성화
        agent.speed = moveSpeed; //속도 설정
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DroneState.Idle: Idle(); break;
            case DroneState.Move: Move();  break;
            case DroneState.Attack: Attack(); break; 
            case DroneState.Damage: Damage(); break;
            case DroneState.Die: Die(); break;
        }
    }
    private void Idle()
    {
        currentTime += Time.deltaTime; //시간을 누적
        if (currentTime > idleDelayTime) //경과시간이 지났다면
        {
            state = DroneState.Move; //무브 상태로 전환
            agent.enabled = true;
        }
    }
    private void Move()
    {
        agent.SetDestination(tower.position); //타워를 향해 이동
        //공격 범위 안에 들어오면 공격 상태로 전환
        if(Vector3.Distance(transform.position,tower.position) < attackRange)
        {
            state= DroneState.Attack;
            agent.enabled = false; //에이전트 동작 일시 정지
        }
    }
    private void Attack()
    {
       currentTime+= Time.deltaTime; //시간을 누적
       if (currentTime > attackDelayTime) //공격 지연 시간이 지났다면
        {
            Tower.Instance.HP--;
            currentTime = 0;
        }
    }
    private void Damage()
    {

    }
    private void Die()
    {

    }
}
