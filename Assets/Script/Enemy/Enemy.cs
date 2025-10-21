using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Idle, Trace, Attack, run }
    public EnemyState state = EnemyState.Idle;

    public float moveSpeed = 2f;
    public float traceRange = 15f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;

    //public GameObject projectilePrefab;     //투사체 프리펩
    //public Transform firePoint;

    private Transform player;
    private float lastAttackTime;
    public int maxHP = 10;
    private int currentHP;
    //public Slider hpSlider;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
        currentHP = maxHP;
        //hpSlider.value = 1f;
    }

    private void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                break;

            case EnemyState.Trace:
                if (currentHP <= (maxHP / 5)) { state = EnemyState.run; break; }

                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                break;

            case EnemyState.Attack:
                if (currentHP <= (maxHP / 5)) { state = EnemyState.run; break; }

                if (dist > attackRange)
                    state = EnemyState.Trace;
                else
                    AttackPlayer();
                break;

            case EnemyState.run:
                if (dist > traceRange)
                {
                    state = EnemyState.Idle;
                }
                else
                    RunforPlayer();
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        //hpSlider.value = (float)currentHP / maxHP;
        if (currentHP > 0 && currentHP <= (maxHP / 5))
        {
            state = EnemyState.run;
        }
        else if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void TracePlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dir = (targetPos - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(targetPos);
    }

    void RunforPlayer()
    {
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dir = (transform.position - targetPos).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void AttackPlayer()
    {
        // 공격 시 제자리에서 플레이어만 바라보도록 수정
        Vector3 targetPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(targetPos);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            //ShootProjectile();
        }
    }

    //void ShootProjectile()
    //{
    //    if (projectilePrefab != null && firePoint != null)
    //    {
    //        transform.LookAt(player.position);
    //        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    //        EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
    //        if (ep != null)
    //        {
    //            Vector3 dir = (player.position - firePoint.position).normalized;
    //            ep.SetDirection(dir);
    //        }
    //    }
    //}
}