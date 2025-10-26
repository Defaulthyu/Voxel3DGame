using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public float damage = 15f;
    public float speed = 15f;       //이동 속도
    public float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        //로컬의 forward 방향으로 이동
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Debug.Log("총알 Hit " + damage);

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

        Destroy(gameObject); // 총알은 한 번 맞으면 사라짐
    }
}
