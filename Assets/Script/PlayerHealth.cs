using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("�÷��̾� ü�� ����")]
    public float maxHP;
    private float currentHP;
    public Slider hpSlider;

    [Header("�÷��̾� ��� ����")]
    public Animator animator;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        //hpSlider.value = 1f;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHP -= damage;
        hpSlider.value = (float)currentHP / maxHP;
        Debug.Log("�÷��̾� ������: " + damage + " / ���� HP: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾� ���");
        if(animator != null)
        {
            animator.SetTrigger("Die");
        }

        //������ ���� ����
        PlayerController controller = GetComponent<PlayerController>();
        if(controller != null) { controller.enabled = false; }

        Invoke(nameof(RestartScene), 3f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
