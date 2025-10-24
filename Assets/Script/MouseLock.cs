using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("���콺 ����")]
    public float mouseSensitivity = 100f;

    [Header("������ ������Ʈ")]
    public Transform firstPersonCamera; // ī�޶� Transform
    public Transform playerBody;        // �÷��̾� ���� (ȸ�� ����)

    private float xRotation = 0f; // ���� ȸ�� ��

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ī�޶� ���� ȸ��
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        firstPersonCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // �÷��̾� �¿� ȸ��
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
