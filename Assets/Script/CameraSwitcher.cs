using UnityEngine;
using Cinemachine; // Cinemachine을 사용하기 위해 꼭 추가해야 합니다!

public class CameraSwitcher : MonoBehaviour
{
    // 인스펙터 창에서 설정할 3인칭 프리룩 카메라
    public CinemachineFreeLook thirdPersonCam;

    // 인스펙터 창에서 설정할 1인칭 가상 카메라
    public CinemachineVirtualCamera firstPersonCam;

    // 1인칭일 때 숨길 플레이어의 몸 렌더러
    public SkinnedMeshRenderer playerBodyRenderer;

    // 현재 1인칭 상태인지 확인하는 변수
    private bool isFirstPerson = false;

    // 시작할 때 3인칭으로 설정
    void Start()
    {
        // 처음에는 3인칭이 활성화되도록 우선순위 설정
        thirdPersonCam.Priority = 11;
        firstPersonCam.Priority = 10;
        if (playerBodyRenderer != null) playerBodyRenderer.enabled = true;
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
            // 1인칭일 때 내 몸이 보이지 않도록 렌더러를 비활성화
            if (playerBodyRenderer != null) playerBodyRenderer.enabled = false;
        }
        else
        {
            // 3인칭 카메라를 활성화
            thirdPersonCam.Priority = 11;
            firstPersonCam.Priority = 10;
            // 3인칭일 때 다시 몸이 보이도록 렌더러를 활성화
            if (playerBodyRenderer != null) playerBodyRenderer.enabled = true;
        }
    }
}