using System.Collections;
using UnityEngine;

public class EndingCamera : MonoBehaviour
{
    [Header("연결할 오브젝트")]
    [SerializeField] private Transform cameraTargetPoint;

    [Tooltip("활성화시킬 UI 캔버스")]
    [SerializeField] private GameObject targetCanvas;

    [Header("카메라 이동 설정")]

    [SerializeField] private float moveSpeed = 2.0f;


    [SerializeField] private float rotateSpeed = 2.0f;

    [Header("비활성화할 스크립트 (선택 사항)")]

    [SerializeField] private MonoBehaviour existingCameraScript;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // 중복 실행 방지
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (existingCameraScript != null)
            {
                existingCameraScript.enabled = false;
            }

            if (targetCanvas != null)
            {
                targetCanvas.SetActive(true);
            }

            if (cameraTargetPoint != null)
            {
                StartCoroutine(MoveCameraSmoothly());
            }

            Destroy(other.gameObject);
            GetComponent<Collider>().enabled = false;
        }
    }
    private IEnumerator MoveCameraSmoothly()
    {
        Camera mainCamera = Camera.main; // 메인 카메라 참조

        float journey = 0f;
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        // 대략적인 이동 시간 계산
        float duration = Vector3.Distance(startPos, cameraTargetPoint.position) / moveSpeed;
        if (duration < 0.1f) duration = 0.1f; // 0으로 나누기 방지

        while (journey <= duration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration); // 0과 1 사이의 값
            float smoothedPercent = Mathf.SmoothStep(0.0f, 1.0f, percent); // Slerp/Lerp에 사용할 부드러운 값

            // 부드러운 이동 (Lerp: Linear Interpolation)
            mainCamera.transform.position = Vector3.Lerp(startPos, cameraTargetPoint.position, smoothedPercent);

            // 부드러운 회전 (Slerp: Spherical Linear Interpolation)
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, cameraTargetPoint.rotation, smoothedPercent);

            yield return null; // 다음 프레임까지 대기
        }

        // 루프 종료 후 정확한 위치와 회전값으로 고정
        mainCamera.transform.position = cameraTargetPoint.position;
        mainCamera.transform.rotation = cameraTargetPoint.rotation;
    }
}