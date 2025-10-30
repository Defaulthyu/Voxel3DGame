using UnityEngine;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
    public Image weaponIcon;
    private PlayerController player;

    [Header("색상 설정")]
    public Color normalColor = Color.white;
    public Color noAmmoColor = Color.red;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnWeaponIconChanged += HandleWeaponIconChanged;

            // 시작 시 현재 무기로 초기화
            var current = player.weaponDataList.Find(w => w.type == player.currentWeapon);
            if (current != null && current.icon != null)
                weaponIcon.sprite = current.icon;
        }
    }

    void Update()
    {
        if (player == null || weaponIcon == null) return;

        // 현재 무기가 총일 때만 색상 변경
        if (player.currentWeapon == PlayerController.WeaponType.Gun)
        {
            if (player.currentAmmo <= 0)
                weaponIcon.color = noAmmoColor;  // 총알 없을 때 빨갛게
            else
                weaponIcon.color = normalColor;  // 평상시 하얀색
        }
        else
        {
            weaponIcon.color = normalColor; // 다른 무기일 때는 원래 색
        }
    }

    void HandleWeaponIconChanged(Sprite newIcon)
    {
        if (newIcon != null)
            weaponIcon.sprite = newIcon;
    }

    void OnDestroy()
    {
        if (player != null)
            player.OnWeaponIconChanged -= HandleWeaponIconChanged;
    }
}
