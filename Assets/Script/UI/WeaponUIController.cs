using UnityEngine;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
    public Image weaponIcon;
    private PlayerController player;

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
