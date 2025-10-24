using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;
    public Renderer[] renderersToHide;

    public static bool isFirstPerson = false;
    public MouseLook mouseLook;

    void Start()
    {
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        SetRenderers(true);

        if (mouseLook != null)
            mouseLook.enabled = false; // �⺻�� 3��Ī
        else
            Debug.LogWarning("MouseLook�� ������� �ʾҽ��ϴ�!");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;
            SwitchCameraState();
        }
    }

    void SwitchCameraState()
    {
        if (isFirstPerson)
        {
            firstPersonCam.Priority = 11;
            thirdPersonCam.Priority = 10;
            SetRenderers(false);
            mouseLook.enabled = true;
        }
        else
        {
            thirdPersonCam.Priority = 11;
            firstPersonCam.Priority = 10;
            SetRenderers(true);
            mouseLook.enabled = false;
        }
    }

    void SetRenderers(bool isVisible)
    {
        if (renderersToHide == null) return;
        foreach (var renderer in renderersToHide)
        {
            if (renderer != null)
                renderer.enabled = isVisible;
        }
    }
}
