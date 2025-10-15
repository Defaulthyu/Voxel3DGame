using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("���콺 ����")]
    public float mouseSensitivity = 100f;

    [Header("������ ������Ʈ")]
    // ī�޶��� ���� ȸ���� ���� 1��Ī ī�޶��� Transform�� �����մϴ�.
    public Transform firstPersonCamera;

    private float xRotation = 0f; // ī�޶��� ���� ȸ����

    void Start()
    {
        // ������ ���۵Ǹ� ���콺 Ŀ���� ����� ȭ�� �߾ӿ� �����մϴ�.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // ���콺�� �¿� ������ �Է� (X��)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // ���콺�� ���� ������ �Է� (Y��)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // --- ���� ȸ�� (ī�޶�) ---
        // ���콺�� Y �����ӿ� ���� xRotation ���� �����մϴ�.
        // ���콺�� ���� �ø��� �ü��� �Ʒ��� ���� ���� �����ϱ� ���� -= �� ����մϴ�.
        xRotation -= mouseY;

        // ī�޶��� ���� ȸ�� ������ -90��(�Ʒ�) ~ 90��(��) ���̷� �����մϴ�.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ���� ī�޶��� Transform�� ���� ȸ������ �����մϴ�.
        if (firstPersonCamera != null)
        {
            firstPersonCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // --- �¿� ȸ�� (�÷��̾�) ---
        // ���콺�� X �����ӿ� ���� �÷��̾� ��ü�� Y�� �������� ȸ����ŵ�ϴ�.
        transform.Rotate(Vector3.up * mouseX);
    }
}