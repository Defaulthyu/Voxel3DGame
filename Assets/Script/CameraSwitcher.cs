using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCam;
    public CinemachineVirtualCamera firstPersonCam;
    public Renderer[] renderersToHide;

    public static bool isFirstPerson = false;
    public MouseLook mouseLook;

    private bool isDead = false;

    void Start()
    {
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        SetRenderers(true);

        if (mouseLook != null)
            mouseLook.enabled = false; // 기본은 3인칭
        else
            Debug.LogWarning("MouseLook이 연결되지 않았습니다!");
    }

    void Update()
    {
        if(isDead == false)
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                isFirstPerson = !isFirstPerson;
                SwitchCameraState();
            }
        }

    }

    public void Die()
    {
        isDead = true;
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        SetRenderers(true);
        mouseLook.enabled = false;
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
