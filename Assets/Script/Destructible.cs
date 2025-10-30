using UnityEngine;
using UnityEngine.UI;

public class Destructible : MonoBehaviour
{
    public float maxHp = 30f;
    public Slider hp;
    private float currentHp;
    public RectTransform rectTransform;

    [Header("사운드")]
    public AudioClip breakSound;
    private AudioSource audioSource;

    void Awake()
    {
        currentHp = maxHp;

        hp.value = 1f;
        hp.value = currentHp / maxHp;
        // 사운드 세팅
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null && breakSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.Log("오디오 소스 셋업");
        }

    }

    public void TakeDamage(float damage)
    {
        if (currentHp <= 0f) return;

        int aliveEnemies = CountAliveEnemiesByTag("Enemy");
        if (aliveEnemies > 0)
        {
            return;
        }

        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }

        currentHp -= damage;
        hp.value = (float)currentHp / maxHp;

        if (currentHp <= 0f)
        {
            Break();
        }
    }

    int CountAliveEnemiesByTag(string tag)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
        int cnt = 0;
        foreach (var e in enemies)
        {
            if (e.activeInHierarchy) cnt++;
        }
        return cnt;
    }

    void Break()
    {
        // 소리 재생
        if (audioSource != null && breakSound != null)
            audioSource.PlayOneShot(breakSound);

        Debug.Log("돌 파괴");

        // 소리 끝날 때까지 약간 대기 후 삭제
        Destroy(gameObject, breakSound != null ? breakSound.length : 0f);
    }
}
