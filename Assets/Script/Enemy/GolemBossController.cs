using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GolemBossController : MonoBehaviour
{
    [Header("스탯")]
    public float maxHp = 1000f;
    public float moveSpeed = 3f;
    public float attackRange = 3f;
    public float jumpCooldown = 8f;
    public float slamDamage = 40f;
    public float attackDamage = 20f;

    public float attackCooldown = 3f;
    public float spawnTime = 2f;
    public Slider hpSlider;

    [Header("참조")]
    public Animator animator;
    public Transform player;
    public AudioSource attackSound;
    public AudioSource jumpSound;
    public AudioSource slamSound;

    private float currentHp;
    private bool isSpawning = true;
    private bool isJumping = false;
    private bool canAttack = true;
    private float lastJumpTime = -999f;

    void Start()
    {
        currentHp = maxHp;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }

    // 트리거에서 호출되는 스폰 연출 시작
    public void PlaySpawnSequence()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        animator.SetTrigger("Spawn");        // 스폰 애니메이션 실행
        FindObjectOfType<CameraShake>()?.StartCoroutine(
            FindObjectOfType<CameraShake>().Shake(2.0f, 0.8f)
        );
        yield return new WaitForSeconds(spawnTime);  // 스폰 애니메이션 길이 (약 3초)
        isSpawning = false;                    // 스폰 끝 → 전투 시작
    }

    void Update()
    {
        if (isSpawning || isJumping) return;
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist < attackRange && canAttack)
        {
            StartCoroutine(Attack());
        }
        else if (Time.time - lastJumpTime > jumpCooldown && dist > 6f)
        {
            StartCoroutine(JumpAttack());
        }
        else
        {
            // 이동
            animator.SetBool("isMoving", true);
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.LookAt(player);
        }
    }

    IEnumerator Attack()
    {
        canAttack = false;
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Attack");


        yield return new WaitForSeconds(1f); // 타격 타이밍
        if (Vector3.Distance(transform.position, player.position) < attackRange + 0.5f)
        {

            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
            attackSound.Play();
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator JumpAttack()
    {
        isJumping = true;
        lastJumpTime = Time.time;

        // 점프 준비
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f); // 준비 동작

        // 도약 사운드
        jumpSound.Play();

        // 점프 이동
        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position;
        Vector3 dir = (targetPos - startPos).normalized;
        targetPos += dir * 1f; // 살짝 더 멀리

        float flightTime = 1.0f;
        float elapsed = 0f;
        float jumpHeight = 12f;

        while (elapsed < flightTime)
        {
            float t = elapsed / flightTime;
            transform.position = Vector3.Lerp(startPos, targetPos, t)
                                + Vector3.up * Mathf.Sin(t * Mathf.PI) * jumpHeight;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 착지 순간 공격
        animator.SetTrigger("Slam");
        slamSound.Play();

        // 카메라 흔들림
        FindObjectOfType<CameraShake>()?.StartCoroutine(
            FindObjectOfType<CameraShake>().Shake(1.0f, 0.6f)
        );

        // 범위 데미지
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerHealth>()?.TakeDamage(slamDamage);
        }

        //착지 후 stopTime 만큼 멈추기
        float stopTime = 5f;
        float timer = 0f;
        animator.SetTrigger("Slam");
        while (timer < stopTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        isJumping = false;
    }

    public void TakeDamage(float dmg)
    {
        if (isSpawning) return; // 스폰 중엔 무적
        currentHp -= dmg;
        hpSlider.value = currentHp;
        if (currentHp <= 0)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject, 2f);
        }
    }
}
