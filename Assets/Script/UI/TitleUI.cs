using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage_1");
    }

    public void Reset()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("GameStart");
    }
}
