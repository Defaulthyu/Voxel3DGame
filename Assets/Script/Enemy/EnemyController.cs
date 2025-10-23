using System.Collections;
using UnityEngine;
// using UnityEngine.AI; // AI�� ������� �����Ƿ� ����

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp = 100f;
    private float currentHp;
    public int attackDamage = 10;
    public float attackCooldown = 2f;
    private float lastAttackTime = -999f;

    [Header("Movement & Detection")]
    public float moveSpeed = 3.5f;     // AI ��� ����� �̵� �ӵ�
    public float detectionRange = 10f;
    public float attackRange = 2f;
    private Transform player;
    // private NavMeshAgent agent; // AI ����
    private Rigidbody rb;           // ���� �̵��� ���� Rigidbody
    private Animator anim;

    // AI ���� ����
    public enum EnemyState { Spawning, Idle, Chasing, Attacking, TakingDamage, Dead }
    public EnemyState currentState;

    void Start()
    {
        Debug.Log("!!!!!!!! ENEMY SCRIPT START() CALLED !!!!!!!"); // <--- �� �� �߰�
        // agent = GetComponent<NavMeshAgent>(); // AI ����
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // Rigidbody�� NavMeshAgentó�� �۵��ϵ��� Kinematic���� ����
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // "Player" �±׸� ���� ������Ʈ�� ã���ϴ�.
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.Exception)
        {
            Debug.LogError("�÷��̾ ã�� �� �����ϴ�. �÷��̾ 'Player' �±װ� �ִ��� Ȯ���ϼ���.");
            this.enabled = false;
            return;
        }

        currentHp = maxHp;
        SetState(EnemyState.Idle);
    }

    void Update()
    {
        // �׾��ų�, ���� ���̰ų�, �°� �ִ� �߿��� �ٸ� �ൿ�� ���� ����
        if (currentState == EnemyState.Dead || currentState == EnemyState.Spawning || currentState == EnemyState.TakingDamage)
        {
            return;
        }

        // �÷��̾���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. ���� ���� �ȿ� �ְ� ��Ÿ���� �����°�?
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            SetState(EnemyState.Attacking);
        }
        // 2. ���� ���� �ȿ� ������ ���� ���� ���ΰ�?
        else if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            SetState(EnemyState.Chasing);
        }
        // 3. ���� ������ ����°�?
        else if (distanceToPlayer > detectionRange)
        {
            SetState(EnemyState.Idle);
        }

        // AI ��� �ִϸ��̼� ���� ���� (SetState �Լ��� �̵�)
        // anim.SetFloat("Speed", ...); // ����
    }

    // ���� ��� �̵��� FixedUpdate���� ó���ϴ� ���� �������Դϴ�.
    void FixedUpdate()
    {
        // ���� ������ ���� �̵�
        if (currentState == EnemyState.Chasing && rb != null)
        {
            // �÷��̾ ���ϴ� ���� ��� (Y���� ����)
            Vector3 direction = (player.position - transform.position);
            direction.y = 0;
            direction.Normalize();

            // �÷��̾ �ٶ󺸰� ��
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (currentState == EnemyState.Chasing && rb != null)
            {
                // ... (���� ���, �ٶ󺸱�) ...

                // 'Time.fixedUnscaledDeltaTime'���� ���� (���� �ð��� ���絵 �� ���� ������ ����)
                Vector3 newPos = transform.position + direction * moveSpeed * Time.fixedUnscaledDeltaTime;
                rb.MovePosition(newPos);
            }
        }
    }

    private void SetState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Spawning:
                StartCoroutine(SpawnSequence());
                break;
            case EnemyState.Idle:
                // agent.isStopped = true; // AI ����
                anim.SetFloat("Speed", 0f); // �ִϸ��̼� ����
                break;
            case EnemyState.Chasing:
                // agent.isStopped = false; // AI ����
                anim.SetFloat("Speed", 1f); // �ȴ� �ִϸ��̼� ��� (Animator�� Speed�� 0.1���� ũ�� ��)
                break;
            case EnemyState.Attacking:
                // agent.isStopped = true; // AI ����
                anim.SetFloat("Speed", 0f); // ���� �� ����
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                anim.SetTrigger("Attack");
                lastAttackTime = Time.time;
                break;
            case EnemyState.TakingDamage:
                anim.SetFloat("Speed", 0f); // �ǰ� �� ����
                break;
            case EnemyState.Dead:
                anim.SetFloat("Speed", 0f); // ���� �� ����
                break;
        }
    }

    // ��ȯ �ִϸ��̼� ������
    private IEnumerator SpawnSequence()
    {
        anim.SetTrigger("Spawn");

        // [�߿�!] Spawning ���¿� ������ ���� �ذ��� ���� Realtime���� ����
        yield return new WaitForSeconds(4.0f); // Time.timeScale ���� �� ����

        Debug.Log("���� ������ �Ϸ�, Idle ���·� ���� �õ�.");
        SetState(EnemyState.Idle);
    }

    // �ܺ�(�÷��̾�)���� �� �Լ��� ȣ���Ͽ� �������� ��
    public void TakeDamage(int damage)
    {
        if (currentState == EnemyState.Dead) return;

        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitSequence());
        }
    }

    // �ǰ� ������
    private IEnumerator HitSequence()
    {
        SetState(EnemyState.TakingDamage);
        // agent.isStopped = true; // AI ����
        anim.SetTrigger("Hit");

        yield return new WaitForSecondsRealtime(0.5f); // �̰͵� Realtime���� ����

        if (currentState != EnemyState.Dead)
        {
            SetState(EnemyState.Idle);
        }
    }

    private void Die()
    {
        SetState(EnemyState.Dead);
        anim.SetTrigger("Die");
        // agent.isStopped = true; // AI ����
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 3.0f);
    }

    // (DealDamageToPlayer �Լ��� ���� ����)
    public void DealDamageToPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }
}