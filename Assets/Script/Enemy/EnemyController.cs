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

    private float currentHp;
    private bool isSpawning = true;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();  // �ڵ� ����

        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;

        animator.SetTrigger("Spawn"); // ���� �ִϸ��̼� ���
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // ���� �ִϸ��̼��� ���� ������ ��ٸ� (���̿� ���� ����)
        yield return new WaitForSeconds(2f);
        isSpawning = false;
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
            // �÷��̾� �������� �̵�
            Vector3 dir = (player.position - transform.position).normalized;
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            transform.position += dir * moveSpeed * Time.deltaTime;

            animator.SetBool("isWalking", true);
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ���

        //  ���� ��� �߰� Ÿ�ֿ̹� ���� ������ ���� (ex. 0.5�� ��)
        yield return new WaitForSeconds(0.5f);

        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= attackRange + 0.2f) // ��¦ ���� �Ÿ�
            {
                PlayerHealth ph = player.GetComponent<PlayerHealth>();
                if (ph != null)
                    ph.TakeDamage(damage);
            }
        }

        // ������ ���� �ִϸ��̼� �ð� ��ٸ�
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
        Destroy(gameObject, 0.7f);
    }
}
