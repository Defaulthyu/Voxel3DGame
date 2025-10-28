using GLTFast.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    public AudioSource openSound;
    private bool isOpened = false;

    public void Open()
    {
        if (isOpened) return;
        isOpened = true;

        if (openSound != null)
            openSound.Play();

        Destroy(gameObject, 1f);
    }
}
