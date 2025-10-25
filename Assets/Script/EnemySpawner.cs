using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 스폰할 적 프리팹
    public bool hasSpawned = false;

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || hasSpawned) return;

        Instantiate(enemyPrefab, transform.position, transform.rotation);
        hasSpawned = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
