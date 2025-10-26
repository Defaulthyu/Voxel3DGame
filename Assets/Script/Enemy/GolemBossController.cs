using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GolemBossController : MonoBehaviour
{
    [Header("����")]
    public float maxHp = 1000f;
    public float moveSpeed = 3f;
    public float attackRange = 3f;
    public float jumpCooldown = 8f;
    public float slamDamage = 40f;
    public float attackDamage = 20f;

    public float attackCooldown = 3f;
    public float spawnTime = 2f;
    public Slider hpSlider;

    [Header("����")]
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

    // Ʈ���ſ��� ȣ��Ǵ� ���� ���� ����
    public void PlaySpawnSequence()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        animator.SetTrigger("Spawn");        // ���� �ִϸ��̼� ����
        FindObjectOfType<CameraShake>()?.StartCoroutine(
            FindObjectOfType<CameraShake>().Shake(2.0f, 0.8f)
        );
        yield return new WaitForSeconds(spawnTime);  // ���� �ִϸ��̼� ���� (�� 3��)
        isSpawning = false;                    // ���� �� �� ���� ����
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
            // �̵�
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


        yield return new WaitForSeconds(1f); // Ÿ�� Ÿ�̹�
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

        // ���� �غ�
        animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f); // �غ� ����

        // ���� ����
        jumpSound.Play();

        // ���� �̵�
        Vector3 startPos = transform.position;
        Vector3 targetPos = player.position;
        Vector3 dir = (targetPos - startPos).normalized;
        targetPos += dir * 1f; // ��¦ �� �ָ�

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

        // ���� ���� ����
        animator.SetTrigger("Slam");
        slamSound.Play();

        // ī�޶� ��鸲
        FindObjectOfType<CameraShake>()?.StartCoroutine(
            FindObjectOfType<CameraShake>().Shake(1.0f, 0.6f)
        );

        // ���� ������
        Collider[] hitPlayers = Physics.OverlapSphere(transform.position, 5f);
        foreach (var hit in hitPlayers)
        {
            if (hit.CompareTag("Player"))
                hit.GetComponent<PlayerHealth>()?.TakeDamage(slamDamage);
        }

        //���� �� stopTime ��ŭ ���߱�
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
        if (isSpawning) return; // ���� �߿� ����
        currentHp -= dmg;
        hpSlider.value = currentHp;
        if (currentHp <= 0)
        {
            animator.SetTrigger("Die");
            Destroy(gameObject, 2f);
        }
    }
}
