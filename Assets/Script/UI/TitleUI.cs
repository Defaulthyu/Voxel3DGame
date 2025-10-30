using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene("Stage_1");
    }

    public void Reset()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene("GameStart");
    }

    public void Mainmenu()
    {
        SceneManager.LoadScene("Title");
    }
}
