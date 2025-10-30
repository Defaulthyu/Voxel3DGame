using UnityEngine;
using UnityEngine.UI;

public class Destructible : MonoBehaviour
{
    public float maxHp = 30f;
    public Slider hp;
    private float currentHp;
    public RectTransform rectTransform;

    [Header("����")]
    public AudioClip breakSound;
    private AudioSource audioSource;

    void Awake()
    {
        currentHp = maxHp;

        hp.value = 1f;
        hp.value = currentHp / maxHp;
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
        // �Ҹ� ���
        if (audioSource != null && breakSound != null)
            audioSource.PlayOneShot(breakSound);

        Debug.Log("�� �ı�");

        // �Ҹ� ���� ������ �ణ ��� �� ����
        Destroy(gameObject, breakSound != null ? breakSound.length : 0f);
    }
}
