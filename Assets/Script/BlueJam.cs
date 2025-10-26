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

    [Header("사운드")]
    public AudioClip breakSound;
    private AudioSource audioSource;

    public string sceneName = "Stage_2"; // 이동할 씬 이름
    public float fadeTime = 1.5f;             // 페이드 시간
    public Image fadeImage;                // 직접 만든 Image 참조

    private bool isTriggered = false;
    public Transform bj;

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

        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }

        currentHp -= damage;

        if (currentHp <= 0f)
        {
            bj.gameObject.SetActive(false);
            // 소리 재생
            if (audioSource != null && breakSound != null)
                audioSource.PlayOneShot(breakSound);
            StartCoroutine(FadeOutAndLoad());
        }
    }

    IEnumerator FadeOutAndLoad()
    {

        // 페이드 아웃
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

