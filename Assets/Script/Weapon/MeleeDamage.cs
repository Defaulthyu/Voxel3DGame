using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    public float damage = 25f;

    private void OnTriggerEnter(Collider other)
    {
        // EnemyHealth 컴포넌트를 가진 오브젝트만 피격 처리
        EnemyController enemy = other.GetComponent<EnemyController>();
        Destructible rock = other.GetComponent<Destructible>();
        EnemyWizard ew = other.GetComponent<EnemyWizard>();
        if (enemy != null)
        {
            if(enemy)
            {
                enemy.TakeDamage(damage);
                Debug.Log("적 공격 " + damage);
            }
        }
        if(rock != null)
        {
            if(rock)
            {
                rock.TakeDamage(damage);
                Debug.Log("문 공격 " + damage);
            }
        }
        if (ew != null)
        {
            if (ew)
            {
                ew.TakeDamage(damage);
                Debug.Log("마법사 공격 " + damage);
            }
        }
    }
}
