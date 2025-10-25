using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float maxHp = 30f;
    private float currentHp;

    [Header("사운드")]
    public AudioClip breakSound;
    private AudioSource audioSource;

    void Awake()
    {
        currentHp = maxHp;

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

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            Break();
        }
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
