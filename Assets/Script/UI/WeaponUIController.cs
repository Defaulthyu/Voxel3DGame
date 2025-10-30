using UnityEngine;
using UnityEngine.UI;

public class WeaponUIController : MonoBehaviour
{
    public Image weaponIcon;
    private PlayerController player;

    [Header("���� ����")]
    public Color normalColor = Color.white;
    public Color noAmmoColor = Color.red;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.OnWeaponIconChanged += HandleWeaponIconChanged;

            // ���� �� ���� ����� �ʱ�ȭ
            var current = player.weaponDataList.Find(w => w.type == player.currentWeapon);
            if (current != null && current.icon != null)
                weaponIcon.sprite = current.icon;
        }
    }

    void Update()
    {
        if (player == null || weaponIcon == null) return;

        // ���� ���Ⱑ ���� ���� ���� ����
        if (player.currentWeapon == PlayerController.WeaponType.Gun)
        {
            if (player.currentAmmo <= 0)
                weaponIcon.color = noAmmoColor;  // �Ѿ� ���� �� ������
            else
                weaponIcon.color = normalColor;  // ���� �Ͼ��
        }
        else
        {
            weaponIcon.color = normalColor; // �ٸ� ������ ���� ���� ��
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
