using UnityEngine;
using Cinemachine; // Cinemachine�� ����ϱ� ���� �� �߰��ؾ� �մϴ�!

public class CameraSwitcher : MonoBehaviour
{
    // �ν����� â���� ������ 3��Ī ������ ī�޶�
    public CinemachineFreeLook thirdPersonCam;

    // �ν����� â���� ������ 1��Ī ���� ī�޶�
    public CinemachineVirtualCamera firstPersonCam;

    // 1��Ī�� �� ���� �÷��̾��� �� ������
    public SkinnedMeshRenderer playerBodyRenderer;

    // ���� 1��Ī �������� Ȯ���ϴ� ����
    private bool isFirstPerson = false;

    // ������ �� 3��Ī���� ����
    void Start()
    {
        // ó������ 3��Ī�� Ȱ��ȭ�ǵ��� �켱���� ����
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        if (playerBodyRenderer != null) playerBodyRenderer.enabled = true;
    }

    void Update()
    {
        // VŰ�� ������ ī�޶� ���� ��ȯ
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson; // ���¸� ������Ŵ
            SwitchCameraState();
        }
    }

    void SwitchCameraState()
    {
        if (isFirstPerson)
        {
            // 1��Ī ī�޶� Ȱ��ȭ (�� ���� �켱����)
            firstPersonCam.Priority = 11;
            thirdPersonCam.Priority = 10;
            // 1��Ī�� �� �� ���� ������ �ʵ��� �������� ��Ȱ��ȭ
            if (playerBodyRenderer != null) playerBodyRenderer.enabled = false;
        }
        else
        {
            // 3��Ī ī�޶� Ȱ��ȭ
            thirdPersonCam.Priority = 11;
            firstPersonCam.Priority = 10;
            // 3��Ī�� �� �ٽ� ���� ���̵��� �������� Ȱ��ȭ
            if (playerBodyRenderer != null) playerBodyRenderer.enabled = true;
        }
    }
}