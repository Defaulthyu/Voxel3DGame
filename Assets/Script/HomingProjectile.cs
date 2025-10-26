using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float rotateSpeed = 5f;
    public float lifetime = 6f;
    private Transform target;
    private float damage;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void Update()
    {
        if (target == null)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        // 방향 유도
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);

        // 전진
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
