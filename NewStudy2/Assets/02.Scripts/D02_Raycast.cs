using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D02_Raycast : MonoBehaviour
{
    [SerializeField] bool raycastAll = false;

    /// <summary>
    /// 캐릭터의 앞쪽으로 Ray를 쏴서 처음 맞힌 물체를 감지하는 함수
    /// </summary>
    void Raycast()
    {
        // Vector3.forward를 사용하면 월드 기준 z축 방향이므로, transform.TransformDirection을 통해 캐릭터 기준 앞쪽 좌표를 구한다.
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, forward * 10, Color.red);
        // 씬 화면에서 Ray를 보기 위한 디버깅. (시작점, 방향과 거리, 옵션으로 색상)을 넣어주면 된다.

        RaycastHit hit;
        // 레이캐스트 정보를 가져오려면 RaycastHit 형식의 변수에 저장하면 된다.
        
        // 레이캐스트도 디버그처럼 (시작점, 방향)을 넣으면 된다. 거리는 입력하지 않을 시 무한이다.
        // 옵션으로 충돌한 물체 정보를
        if (Physics.Raycast(transform.position + Vector3.up, forward, out hit, 10)) 
        {
            Debug.Log($"레이캐스트가 {hit.collider.gameObject.name} 을 감지했습니다.");
        }
    }

    /// <summary>
    /// 캐릭터의 앞쪽으로 Ray를 쏴서 맞힌 물체를 모두 감지하는 함수
    /// </summary>
    void RaycastAll()
    {
        Vector3 playerForward = transform.TransformDirection(Vector3.forward);
        Debug.DrawRay(transform.position + Vector3.up, playerForward * 10, Color.red);

        RaycastHit[] hit = Physics.RaycastAll(transform.position + Vector3.up, playerForward, 10);

        if(hit.Length > 0)
        {
            foreach (var item in hit)
            {
                Debug.Log($"레이캐스트가 {item.collider.gameObject.name} 을 감지했습니다.");
            }
        }

    }

    // 3인칭 시점의 3D RPG 게임에서는, 벽이나 물체에 캐릭터가 가려 안 보이는 경우 카메라가 더 근접해서 캐릭터를 시야에 들어오도록 하는 데 자주 이용된다.
    // 플레이어에서 카메라 방향으로 Ray를 쏴서 중간에 물체가 감지되면 카메라를 해당 물체 앞으로 가져오는 방식이다.

    private void Update()
    {
        if (!raycastAll) Raycast();
        else RaycastAll();
    }
}
