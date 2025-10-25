using UnityEngine;

public class Destructible : MonoBehaviour
{
    public float maxHp = 30f;
    private float currentHp;

    [Header("����")]
    public AudioClip breakSound;
    private AudioSource audioSource;

    void Awake()
    {
        currentHp = maxHp;

        // ���� ����
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null && breakSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.Log("����� �ҽ� �¾�");
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
        // �Ҹ� ���
        if (audioSource != null && breakSound != null)
            audioSource.PlayOneShot(breakSound);

        Debug.Log("�� �ı�");

        // �Ҹ� ���� ������ �ణ ��� �� ����
        Destroy(gameObject, breakSound != null ? breakSound.length : 0f);
    }
}
