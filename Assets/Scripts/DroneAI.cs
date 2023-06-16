using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    
    float currentTime; //경과 시간
    
    Transform tower; //타워 위치
    NavMeshAgent agent; //길 찾기를 수행할 내비게이션 메시 에이전트
    public float attackRange = 3; //공격 범위
    public float attackDelayTime = 2; //공격 지연 시간

    [Header("적 정보")]
    private int hp;
    public int hpMax;
    public float moveSpeed; //이동 속도
    public float idleDelayTime; //대기 상태의 지속 시간
    public Slider hpSlider;
    public Text level,hpText;

    [Header("폭발효과")]
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;

    private void Start()
    {
        hpMax *= GameManager.Instance.GameLevel;
        moveSpeed *= GameManager.Instance.GameLevel*1.15f;
        hp = hpMax;
        tower = GameObject.Find("Tower").transform; //타워 찾기
        agent = GetComponent<NavMeshAgent>(); //내비메쉬에이전트 컴포넌트
        agent.enabled = false; //비활성화
        agent.speed = moveSpeed; //속도 설정
        explosion = GameObject.Find("Explosion").transform;
        expEffect = explosion.GetComponent<ParticleSystem>();
        expAudio = expEffect.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DroneState.Idle: Idle(); break;
            case DroneState.Move: Move();  break;
            case DroneState.Attack: Attack(); break; 
        }
        hpSlider.value= (float)hp/ (float)hpMax;
        level.text = $"LV. {GameManager.Instance.GameLevel}";
        hpText.text = $"HP {hp}/{hpMax}";
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
        if (Vector3.Distance(transform.position,tower.position) < attackRange)
        {
            state= DroneState.Attack;
            
            agent.enabled = false; //에이전트 동작 일시 정지
            
            currentTime = attackDelayTime; // 바로 공격할 수 있도록 공격 시간 설정
        }
    }
    private void Attack()
    {
       currentTime += Time.deltaTime; //시간을 누적
       if (currentTime > attackDelayTime) //공격 지연 시간이 지났다면
        {
            Tower.Instance.HP--;
            currentTime = 0;
        }
    }

    IEnumerator Damage()
    {
        agent.enabled = false; //길찾기 중지
        //자식 객체의 MeshRenderer 컴포넌트 정보 얻어오기
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        Color oringinalColor = mat.color;
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f); //0.1초 동안 빨간색으로 피격 효과
        mat.color = oringinalColor;
        state = DroneState.Idle;
        currentTime = 0;
    }
    
    public void OnDamageProcess()
    {
        hp-=GameManager.Instance.playerDamage; 
        if (hp > 0)
        {
            state = DroneState.Damage; //상태를 데미지로 전환
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
        else
        {
            UIManager.Instance.UpdateKillText(++GameManager.Instance.kill);
            UIManager.Instance.UpdateDroneCnt(--UIManager.Instance.DroneCnt);
            explosion.position = transform.position; //폭발 효과 위치 지정
            expEffect.Play();
            expAudio.Play();
            Destroy(gameObject);
        }
    }
}
