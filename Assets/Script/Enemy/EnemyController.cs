using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("����")]
    public float maxHp = 100f;
    public float moveSpeed = 2f;
    public float attackRange = 2f;
    public float attackDelay = 1.5f;
    public float damage = 10f;

    [Header("����")]
    public Slider hpSlider;
    public Animator animator;
    public Transform player;

    [Header("���� ����")]
    public float spawnDuration = 2f; // ���� �ִϸ��̼� ����(��)

    public float dieTime = 0.7f;
    private float currentHp;
    private bool isSpawning = true;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;

        // ���� ����
        isSpawning = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Spawn");

        // ���� �ð���ŭ ��ٸ� �� �ڵ� ����
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnDuration);
        isSpawning = false;
        Debug.Log($"{name} ���� �Ϸ�!");
    }

    void Update()
    {
        if (isDead || isSpawning) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            if (!isAttacking)
                StartCoroutine(AttackRoutine());
        }
        else
        {
            Vector3 dir = (player.position - transform.position);
            dir.y = 0; // ���� �̵���
            dir.Normalize();

            // �ε巴�� ȸ��
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);

            // ȸ�� �� ������ ����
            transform.position += transform.forward * moveSpeed * Time.deltaTime;

            animator.SetBool("isWalking", true);
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange + 0.2f)
            {
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(attackDelay - 0.5f);
        isAttacking = false;
    }

    public void TakeDamage(float dmg)
    {
        if (isDead) return;

        currentHp -= dmg;
        hpSlider.value = currentHp;

        if (currentHp <= 0)
        {
            Die();
        }
        else
            animator.SetTrigger("Hit");
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, dieTime);
    }
}
