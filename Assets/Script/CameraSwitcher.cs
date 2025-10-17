using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCam;

    public CinemachineVirtualCamera firstPersonCam;

    public Renderer[] renderersToHide;

    // ���� 1��Ī �������� Ȯ���ϴ� ����
    private bool isFirstPerson = false;

    // ������ �� 3��Ī���� ����
    void Start()
    {
        // ó������ 3��Ī�� Ȱ��ȭ�ǵ��� �켱���� ����
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        SetRenderers(true);
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
            SetRenderers(false);
        }
        else
        {
            // 3��Ī ī�޶� Ȱ��ȭ
            thirdPersonCam.Priority = 11;
            firstPersonCam.Priority = 10;
            SetRenderers(true);
        }
    }

    void SetRenderers(bool isVisible)
    {
        if (renderersToHide == null) return;

        foreach (var renderer in renderersToHide)
        {
            if (renderer != null)
            {
                renderer.enabled = isVisible;
            }
        }
    }
}