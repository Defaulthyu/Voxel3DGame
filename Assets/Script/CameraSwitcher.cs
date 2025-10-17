using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCam;

    public CinemachineVirtualCamera firstPersonCam;

    public Renderer[] renderersToHide;

    // 현재 1인칭 상태인지 확인하는 변수
    private bool isFirstPerson = false;

    // 시작할 때 3인칭으로 설정
    void Start()
    {
        // 처음에는 3인칭이 활성화되도록 우선순위 설정
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        SetRenderers(true);
    }

    void Update()
    {
        // V키를 누르면 카메라 상태 전환
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson; // 상태를 반전시킴
            SwitchCameraState();
        }
    }

    void SwitchCameraState()
    {
        if (isFirstPerson)
        {
            // 1인칭 카메라를 활성화 (더 높은 우선순위)
            firstPersonCam.Priority = 11;
            thirdPersonCam.Priority = 10;
            SetRenderers(false);
        }
        else
        {
            // 3인칭 카메라를 활성화
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