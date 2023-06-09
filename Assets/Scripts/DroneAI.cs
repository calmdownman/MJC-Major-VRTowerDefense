using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    //����� ���� ��� ����
    enum DroneState
    {
        Idle,
        Move, 
        Attack,
        Damage,
        Die 
    }
    DroneState state = DroneState.Idle; //�ʱ� ���� ���¸� Idle
    public float idleDelayTime = 2; //��� ������ ���� �ð�
    float currentTime; //��� �ð�
    public float moveSpeed = 1; //�̵� �ӵ�
    Transform tower; //Ÿ�� ��ġ
    NavMeshAgent agent; //�� ã�⸦ ������ ������̼� �޽� ������Ʈ
    public float attackRange = 3; //���� ����
    public float attackDelayTime = 2; //���� ���� �ð�
    [SerializeField]
    private int hp = 3;

    [Header("����ȿ��")]
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;

    private void Start()
    {
        tower = GameObject.Find("Tower").transform; //Ÿ�� ã��
        agent = GetComponent<NavMeshAgent>(); //����޽�������Ʈ ������Ʈ
        agent.enabled = false; //��Ȱ��ȭ
        agent.speed = moveSpeed; //�ӵ� ����
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
    }
    private void Idle()
    {
        currentTime += Time.deltaTime; //�ð��� ����
        if (currentTime > idleDelayTime) //����ð��� �����ٸ�
        {
            state = DroneState.Move; //���� ���·� ��ȯ
            agent.enabled = true;
        }
    }
    private void Move()
    {
        agent.SetDestination(tower.position); //Ÿ���� ���� �̵�
        
        //���� ���� �ȿ� ������ ���� ���·� ��ȯ
        if(Vector3.Distance(transform.position,tower.position) < attackRange)
        {
            state= DroneState.Attack;
            
            agent.enabled = false; //������Ʈ ���� �Ͻ� ����
            
            currentTime = attackDelayTime; // �ٷ� ������ �� �ֵ��� ���� �ð� ����
        }
    }
    private void Attack()
    {
       currentTime += Time.deltaTime; //�ð��� ����
       if (currentTime > attackDelayTime) //���� ���� �ð��� �����ٸ�
        {
            Tower.Instance.HP--;
            currentTime = 0;
        }
    }
    IEnumerator Damage()
    {
        agent.enabled = false; //��ã�� ����
        //�ڽ� ��ü�� MeshRenderer ������Ʈ ���� ������
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        Color oringinalColor = mat.color;
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f); //0.1�� ���� ���������� �ǰ� ȿ��
        mat.color = oringinalColor;
        state = DroneState.Idle;
        currentTime = 0;
    }
    
    public void OnDamageProcess()
    {
        hp--; 
        if (hp > 0)
        {
            state = DroneState.Damage; //���¸� �������� ��ȯ
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
        else
        {
            explosion.position = transform.position; //���� ȿ�� ��ġ ����
            expEffect.Play();
            expAudio.Play();
            Destroy(gameObject);
        }
    }
}
