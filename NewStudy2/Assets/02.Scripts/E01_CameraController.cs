using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class E01_CameraController : MonoBehaviour
{
    // 카메라를 플레이어의 자식 오브젝트로 넣으면, 플레이어가 회전할 때마다 카메라도 회전하게 된다.
    // 이는 1인칭 게임이 아닌 이상 부적합하며, 쿼터뷰나 탑뷰 등 각 시점마다 카메라 동작을 제어하는 방식이 다르다.

    [SerializeField] E02_Define.CameraMode _cameraMode = E02_Define.CameraMode.QuarterView;
    [SerializeField] Vector3 _delta;
    [SerializeField] GameObject _player;

    void MoveToPlayer()
    {
        // 플레이어를 따라 이동만 하게 한다. delta는 플레이어로부터 카메라가 떨어져 있는 거리이다.
        transform.position = _player.transform.position + _delta;
    }

    void LookAtPlayer()
    {
        // 카메라의 각도가 항상 플레이어를 바라보게 한다.
        transform.LookAt(_player.transform.position + (Vector3.up * 1.2f));
    }

    void LateUpdate()
    {
        // 플레이어 이동이 끝난 후에 카메라를 변경해야 하므로, LateUpdate에서 실행한다.
        // Update에서 실행하면 플레이어와 카메라의 Update문 중 어느쪽이 먼저 실행되는가는 랜덤이므로, 카메라가 덜덜 떨리는 현상이 발생한다.
        MoveToPlayer();
        LookAtPlayer();
    }

    public void SetQuarterView(Vector3 delta)
    {
        _cameraMode = E02_Define.CameraMode.QuarterView;
        _delta = delta;
    }
}
