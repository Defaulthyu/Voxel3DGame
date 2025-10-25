using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    private EnemySpawner[] enemySpawners;
    public bool spawnOnce = true;
    private bool hasSpawned = false;

    private void Start()
    {
        // �ڽ� �Ǵ� �ֺ� EnemySpawner �ڵ� Ž��
        enemySpawners = GetComponentsInChildren<EnemySpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (spawnOnce && hasSpawned) return;

        foreach (EnemySpawner spawner in enemySpawners)
        {
            spawner.SpawnEnemy();
        }

        hasSpawned = true;
        Debug.Log($"[TriggerZoneSpawner] {enemySpawners.Length} enemies spawned!");
    }
}
