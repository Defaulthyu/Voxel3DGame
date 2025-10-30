using System.Collections;
using UnityEngine;

public class EndingCamera : MonoBehaviour
{
    [Header("������ ������Ʈ")]
    [SerializeField] private Transform cameraTargetPoint;

    [Tooltip("Ȱ��ȭ��ų UI ĵ����")]
    [SerializeField] private GameObject targetCanvas;

    [Header("ī�޶� �̵� ����")]

    [SerializeField] private float moveSpeed = 2.0f;


    [SerializeField] private float rotateSpeed = 2.0f;

    [Header("��Ȱ��ȭ�� ��ũ��Ʈ (���� ����)")]

    [SerializeField] private MonoBehaviour existingCameraScript;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; // �ߺ� ���� ����
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
        Camera mainCamera = Camera.main; // ���� ī�޶� ����

        float journey = 0f;
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        // �뷫���� �̵� �ð� ���
        float duration = Vector3.Distance(startPos, cameraTargetPoint.position) / moveSpeed;
        if (duration < 0.1f) duration = 0.1f; // 0���� ������ ����

        while (journey <= duration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / duration); // 0�� 1 ������ ��
            float smoothedPercent = Mathf.SmoothStep(0.0f, 1.0f, percent); // Slerp/Lerp�� ����� �ε巯�� ��

            // �ε巯�� �̵� (Lerp: Linear Interpolation)
            mainCamera.transform.position = Vector3.Lerp(startPos, cameraTargetPoint.position, smoothedPercent);

            // �ε巯�� ȸ�� (Slerp: Spherical Linear Interpolation)
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, cameraTargetPoint.rotation, smoothedPercent);

            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ���� �� ��Ȯ�� ��ġ�� ȸ�������� ����
        mainCamera.transform.position = cameraTargetPoint.position;
        mainCamera.transform.rotation = cameraTargetPoint.rotation;
    }
}