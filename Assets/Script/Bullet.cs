using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float damage = 15f;

    private void OnCollisionEnter(Collision collision)
    {
        EnemyController enemy = collision.collider.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject); // �Ѿ��� �� �� ������ �����
    }
}
