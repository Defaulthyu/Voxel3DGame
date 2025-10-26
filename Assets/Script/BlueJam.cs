using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlueJam : MonoBehaviour
{
    public float maxHp = 10f;
    private float currentHp;
    public RectTransform rectTransform;

    [Header("����")]
    public AudioClip breakSound;
    private AudioSource audioSource;

    public string sceneName = "Stage_2"; // �̵��� �� �̸�
    public float fadeTime = 1.5f;             // ���̵� �ð�
    public Image fadeImage;                // ���� ���� Image ����

    private bool isTriggered = false;
    public Transform bj;

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

        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            bj.gameObject.SetActive(false);
            // �Ҹ� ���
            if (audioSource != null && breakSound != null)
                audioSource.PlayOneShot(breakSound);
            StartCoroutine(FadeOutAndLoad());
        }
    }

    IEnumerator FadeOutAndLoad()
    {

        // ���̵� �ƿ�
        float t = 0;
        Color c = fadeImage.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeTime);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}

