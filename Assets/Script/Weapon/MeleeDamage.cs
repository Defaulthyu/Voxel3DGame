using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public float damage = 25f;

    private PlayerController playerController;

    private void Start()
    {
        // 부모나 루트에서 PlayerController 찾기
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        bool hitSomething = false;

        EnemyController enemy = other.GetComponent<EnemyController>();
        Destructible rock = other.GetComponent<Destructible>();
        EnemyWizard ew = other.GetComponent<EnemyWizard>();
        GolemBossController boss = other.GetComponent<GolemBossController>();
        BlueJam bj = other.GetComponent<BlueJam>();

        if (enemy)
        {
            enemy.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("적 공격 " + damage);
        }
        if (rock)
        {
            rock.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("문 공격 " + damage);
        }
        if (ew)
        {
            ew.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("마법사 공격 " + damage);
        }
        if (boss)
        {
            boss.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("골렘 공격 " + damage);
        }
        if (bj)
        {
            bj.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("블루젬 공격 " + damage);
        }

        // 타격이 실제로 일어났을 때만 소리 재생
        if (hitSomething && playerController && playerController.audioSource)
        {
            if (playerController.currentWeapon == PlayerController.WeaponType.Fist && playerController.fistHitSound)
                playerController.audioSource.PlayOneShot(playerController.fistHitSound);

            if (playerController.currentWeapon == PlayerController.WeaponType.Sword && playerController.swordHitSound)
                playerController.audioSource.PlayOneShot(playerController.swordHitSound);
        }
    }
}
