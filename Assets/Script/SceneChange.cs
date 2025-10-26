using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName = "Stage_2"; // �̵��� �� �̸�
    public float fadeTime = 1.5f;             // ���̵� �ð�
    public Image fadeImage;                // ���� ���� Image ����

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
