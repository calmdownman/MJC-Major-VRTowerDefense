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

    private void Start()
    {
        tower = GameObject.Find("Tower").transform; //Ÿ�� ã��
        agent = GetComponent<NavMeshAgent>(); //����޽�������Ʈ ������Ʈ
        agent.enabled = false; //��Ȱ��ȭ
        agent.speed = moveSpeed; //�ӵ� ����
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
        }
    }
    private void Attack()
    {
       currentTime+= Time.deltaTime; //�ð��� ����
       if (currentTime > attackDelayTime) //���� ���� �ð��� �����ٸ�
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
