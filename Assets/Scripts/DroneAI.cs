using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    
    float currentTime; //��� �ð�
    
    Transform tower; //Ÿ�� ��ġ
    NavMeshAgent agent; //�� ã�⸦ ������ ������̼� �޽� ������Ʈ
    public float attackRange = 3; //���� ����
    public float attackDelayTime = 2; //���� ���� �ð�

    [Header("�� ����")]
    private int hp;
    public int hpMax;
    public float moveSpeed; //�̵� �ӵ�
    public float idleDelayTime; //��� ������ ���� �ð�
    public Slider hpSlider;
    public Text level,hpText;

    [Header("����ȿ��")]
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;

    private void Start()
    {
        hpMax *= GameManager.Instance.GameLevel;
        moveSpeed *= GameManager.Instance.GameLevel*1.15f;
        hp = hpMax;
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
        hpSlider.value= (float)hp/ (float)hpMax;
        level.text = $"LV. {GameManager.Instance.GameLevel}";
        hpText.text = $"HP {hp}/{hpMax}";
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
        if (Vector3.Distance(transform.position,tower.position) < attackRange)
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
        hp-=GameManager.Instance.playerDamage; 
        if (hp > 0)
        {
            state = DroneState.Damage; //���¸� �������� ��ȯ
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
        else
        {
            UIManager.Instance.UpdateKillText(++GameManager.Instance.kill);
            UIManager.Instance.UpdateDroneCnt(--UIManager.Instance.DroneCnt);
            explosion.position = transform.position; //���� ȿ�� ��ġ ����
            expEffect.Play();
            expAudio.Play();
            Destroy(gameObject);
        }
    }
}
