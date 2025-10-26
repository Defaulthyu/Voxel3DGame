using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName = "Stage_2"; // 이동할 씬 이름
    public float fadeTime = 1.5f;             // 페이드 시간
    public Image fadeImage;                // 직접 만든 Image 참조

    private bool isTriggered = false;



    private void OnTriggerEnter(Collider other)
    {
        if (isTriggered) return;
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
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
