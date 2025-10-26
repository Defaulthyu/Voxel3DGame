using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public float damage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        // EnemyHealth ������Ʈ�� ���� ������Ʈ�� �ǰ� ó��
        EnemyController enemy = other.GetComponent<EnemyController>();
        Destructible rock = other.GetComponent<Destructible>();
        EnemyWizard ew = other.GetComponent<EnemyWizard>();
        if (enemy != null)
        {
            if(enemy)
            {
                enemy.TakeDamage(damage);
                Debug.Log("�� ���� " + damage);
            }
        }
        if(rock != null)
        {
            if(rock)
            {
                rock.TakeDamage(damage);
                Debug.Log("�� ���� " + damage);
            }
        }
        if (ew != null)
        {
            if (ew)
            {
                ew.TakeDamage(damage);
                Debug.Log("������ ���� " + damage);
            }
        }
    }
}
