using UnityEngine;
using System.Collections;

public class BossTrigger : MonoBehaviour
{
    public GameObject bossPrefab;
    public Transform spawnPoint;
    public CameraShake cameraShake;
    public AudioSource caveRumbleSound;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(SpawnBossSequence());
        }
    }

    IEnumerator SpawnBossSequence()
    {
        caveRumbleSound.Play();
        yield return cameraShake.Shake(2.5f, 1.2f);

        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, spawnPoint.rotation);
        GolemBossController bossController = boss.GetComponent<GolemBossController>();
        bossController.PlaySpawnSequence();
    }
}
