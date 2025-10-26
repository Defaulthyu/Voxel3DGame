using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float damage = 15f;
    public float speed = 15f;       //�̵� �ӵ�
    public float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //������ forward �������� �̵�
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Debug.Log("�Ѿ� Hit " + damage);

        EnemyWizard ew = other.GetComponent<EnemyWizard>();
        if (ew != null)
        {
            ew.TakeDamage(damage);
        }

        Destructible rock = other.GetComponent<Destructible>();
        if (rock != null)
        {
            rock.TakeDamage(damage);
        }

        Destroy(gameObject); // �Ѿ��� �� �� ������ �����
    }
}
