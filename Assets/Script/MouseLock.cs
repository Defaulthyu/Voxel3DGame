using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("마우스 감도")]
    public float mouseSensitivity = 100f;

    [Header("연결할 오브젝트")]
    // 카메라의 상하 회전을 위해 1인칭 카메라의 Transform을 연결합니다.
    public Transform firstPersonCamera;

    private float xRotation = 0f; // 카메라의 상하 회전값

    void Start()
    {
        // 게임이 시작되면 마우스 커서를 숨기고 화면 중앙에 고정합니다.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 마우스의 좌우 움직임 입력 (X축)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        // 마우스의 상하 움직임 입력 (Y축)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // --- 상하 회전 (카메라) ---
        // 마우스의 Y 움직임에 따라 xRotation 값을 조절합니다.
        // 마우스를 위로 올리면 시선이 아래로 가는 것을 방지하기 위해 -= 를 사용합니다.
        xRotation -= mouseY;

        // 카메라의 상하 회전 각도를 -90도(아래) ~ 90도(위) 사이로 제한합니다.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // 실제 카메라의 Transform에 상하 회전값을 적용합니다.
        if (firstPersonCamera != null)
        {
            firstPersonCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        // --- 좌우 회전 (플레이어) ---
        // 마우스의 X 움직임에 따라 플레이어 전체를 Y축 기준으로 회전시킵니다.
        transform.Rotate(Vector3.up * mouseX);
    }
}