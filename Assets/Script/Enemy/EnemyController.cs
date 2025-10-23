using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float maxHP = 100f;
    private float currentHP;

    public Slider hpSlider;        // HP ǥ�ÿ� UI
    public float detectRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        hpSlider.maxValue = maxHP;
        hpSlider.value = currentHP;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        // ���� �ִϸ��̼�
        animator.SetTrigger("Spawn");
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // �÷��̾� Ž��
        if (distance <= detectRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);

            // ���� ���� ���̸� ����
            if (distance <= attackRange && !isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        agent.isStopped = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f); // ���� Ÿ�̹� (�ִϸ��̼� ���缭 ����)

        // �÷��̾� ü�� ���
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(attackCooldown);
        agent.isStopped = false;
        isAttacking = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        hpSlider.value = currentHP;
        animator.SetTrigger("Hit");

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        agent.isStopped = true;
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(2f); // �״� �ִϸ��̼� ���̸�ŭ
        Destroy(gameObject);
    }
}
