using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public float damage = 25f;

    private PlayerController playerController;

    private void Start()
    {
        // �θ� ��Ʈ���� PlayerController ã��
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
            Debug.Log("�� ���� " + damage);
        }
        if (rock)
        {
            rock.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("�� ���� " + damage);
        }
        if (ew)
        {
            ew.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("������ ���� " + damage);
        }
        if (boss)
        {
            boss.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("�� ���� " + damage);
        }
        if (bj)
        {
            bj.TakeDamage(damage);
            hitSomething = true;
            Debug.Log("����� ���� " + damage);
        }

        // Ÿ���� ������ �Ͼ�� ���� �Ҹ� ���
        if (hitSomething && playerController && playerController.audioSource)
        {
            if (playerController.currentWeapon == PlayerController.WeaponType.Fist && playerController.fistHitSound)
                playerController.audioSource.PlayOneShot(playerController.fistHitSound);

            if (playerController.currentWeapon == PlayerController.WeaponType.Sword && playerController.swordHitSound)
                playerController.audioSource.PlayOneShot(playerController.swordHitSound);
        }
    }
}
