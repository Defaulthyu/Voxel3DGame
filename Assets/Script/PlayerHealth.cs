using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("플레이어 체력 설정")]
    public float maxHP;
    private float currentHP;
    public Slider hpSlider;

    [Header("플레이어 사망 설정")]
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
        Debug.Log("플레이어 데미지: " + damage + " / 남은 HP: " + currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망");
        if(animator != null)
        {
            animator.SetTrigger("Die");
        }

        //움직임 공격 차단
        PlayerController controller = GetComponent<PlayerController>();
        if(controller != null) { controller.enabled = false; }

        Invoke(nameof(RestartScene), 3f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
