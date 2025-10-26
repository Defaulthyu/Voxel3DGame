using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkeletonMageController : MonoBehaviour
{
    [Header("����")]
    public float maxHp = 80f;
    public float attackRange = 10f;
    public float attackDelay = 2f;
    public float damage = 15f;

    [Header("����")]
    public Slider hpSlider;
    public Animator animator;
    public Transform player;
    public Transform firePoint;          // �߻� ��ġ
    public GameObject magicProjectile;   // ���� ����ź ������

    [Header("���� ����")]
    public float spawnDuration = 2f;
    public float dieTime = 0.8f;

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
        animator.SetTrigger("Spawn");

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
        if (isDead || isSpawning || player == null) return;

        // �÷��̾� �ٶ󺸱� (y�ุ)
        Vector3 lookDir = player.position - transform.position;
        lookDir.y = 0;
        if (lookDir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 5f * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        if (player != null)
        {
            GameObject proj = Instantiate(magicProjectile, firePoint.position, firePoint.rotation);
            HomingProjectile hp = proj.GetComponent<HomingProjectile>();
            if (hp != null)
            {
                hp.SetTarget(player);
                hp.SetDamage(damage);
            }
        }

        yield return new WaitForSeconds(attackDelay);
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
