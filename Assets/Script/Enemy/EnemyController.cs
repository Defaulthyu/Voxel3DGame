using System.Collections;
using UnityEngine;
// using UnityEngine.AI; // AI를 사용하지 않으므로 삭제

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHp = 100f;
    private float currentHp;
    public int attackDamage = 10;
    public float attackCooldown = 2f;
    private float lastAttackTime = -999f;

    [Header("Movement & Detection")]
    public float moveSpeed = 3.5f;     // AI 대신 사용할 이동 속도
    public float detectionRange = 10f;
    public float attackRange = 2f;
    private Transform player;
    // private NavMeshAgent agent; // AI 삭제
    private Rigidbody rb;           // 물리 이동을 위한 Rigidbody
    private Animator anim;

    // AI 상태 정의
    public enum EnemyState { Spawning, Idle, Chasing, Attacking, TakingDamage, Dead }
    public EnemyState currentState;

    void Start()
    {
        Debug.Log("!!!!!!!! ENEMY SCRIPT START() CALLED !!!!!!!"); // <--- 이 줄 추가
        // agent = GetComponent<NavMeshAgent>(); // AI 삭제
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // Rigidbody가 NavMeshAgent처럼 작동하도록 Kinematic으로 설정
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // "Player" 태그를 가진 오브젝트를 찾습니다.
        try
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch (System.Exception)
        {
            Debug.LogError("플레이어를 찾을 수 없습니다. 플레이어에 'Player' 태그가 있는지 확인하세요.");
            this.enabled = false;
            return;
        }

        currentHp = maxHp;
        SetState(EnemyState.Idle);
    }

    void Update()
    {
        // 죽었거나, 스폰 중이거나, 맞고 있는 중에는 다른 행동을 하지 않음
        if (currentState == EnemyState.Dead || currentState == EnemyState.Spawning || currentState == EnemyState.TakingDamage)
        {
            return;
        }

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. 공격 범위 안에 있고 쿨타임이 지났는가?
        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            SetState(EnemyState.Attacking);
        }
        // 2. 감지 범위 안에 있지만 공격 범위 밖인가?
        else if (distanceToPlayer <= detectionRange && distanceToPlayer > attackRange)
        {
            SetState(EnemyState.Chasing);
        }
        // 3. 감지 범위를 벗어났는가?
        else if (distanceToPlayer > detectionRange)
        {
            SetState(EnemyState.Idle);
        }

        // AI 대신 애니메이션 직접 제어 (SetState 함수로 이동)
        // anim.SetFloat("Speed", ...); // 삭제
    }

    // 물리 기반 이동은 FixedUpdate에서 처리하는 것이 안정적입니다.
    void FixedUpdate()
    {
        // 추적 상태일 때만 이동
        if (currentState == EnemyState.Chasing && rb != null)
        {
            // 플레이어를 향하는 방향 계산 (Y축은 무시)
            Vector3 direction = (player.position - transform.position);
            direction.y = 0;
            direction.Normalize();

            // 플레이어를 바라보게 함
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

            if (currentState == EnemyState.Chasing && rb != null)
            {
                // ... (방향 계산, 바라보기) ...

                // 'Time.fixedUnscaledDeltaTime'으로 변경 (게임 시간이 멈춰도 이 값은 멈추지 않음)
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
                // agent.isStopped = true; // AI 삭제
                anim.SetFloat("Speed", 0f); // 애니메이션 멈춤
                break;
            case EnemyState.Chasing:
                // agent.isStopped = false; // AI 삭제
                anim.SetFloat("Speed", 1f); // 걷는 애니메이션 재생 (Animator의 Speed가 0.1보다 크면 됨)
                break;
            case EnemyState.Attacking:
                // agent.isStopped = true; // AI 삭제
                anim.SetFloat("Speed", 0f); // 공격 중 멈춤
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                anim.SetTrigger("Attack");
                lastAttackTime = Time.time;
                break;
            case EnemyState.TakingDamage:
                anim.SetFloat("Speed", 0f); // 피격 시 멈춤
                break;
            case EnemyState.Dead:
                anim.SetFloat("Speed", 0f); // 죽을 때 멈춤
                break;
        }
    }

    // 소환 애니메이션 시퀀스
    private IEnumerator SpawnSequence()
    {
        anim.SetTrigger("Spawn");

        // [중요!] Spawning 상태에 갇히는 문제 해결을 위해 Realtime으로 변경
        yield return new WaitForSeconds(4.0f); // Time.timeScale 영향 안 받음

        Debug.Log("스폰 시퀀스 완료, Idle 상태로 변경 시도.");
        SetState(EnemyState.Idle);
    }

    // 외부(플레이어)에서 이 함수를 호출하여 데미지를 줌
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

    // 피격 시퀀스
    private IEnumerator HitSequence()
    {
        SetState(EnemyState.TakingDamage);
        // agent.isStopped = true; // AI 삭제
        anim.SetTrigger("Hit");

        yield return new WaitForSecondsRealtime(0.5f); // 이것도 Realtime으로 변경

        if (currentState != EnemyState.Dead)
        {
            SetState(EnemyState.Idle);
        }
    }

    private void Die()
    {
        SetState(EnemyState.Dead);
        anim.SetTrigger("Die");
        // agent.isStopped = true; // AI 삭제
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        Destroy(gameObject, 3.0f);
    }

    // (DealDamageToPlayer 함수는 변경 없음)
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