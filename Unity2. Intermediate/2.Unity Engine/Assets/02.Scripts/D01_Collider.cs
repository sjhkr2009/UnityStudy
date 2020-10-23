using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D01_Collider : MonoBehaviour
{
    // 콜라이더 감지 조건
    // 1. 두 물체의 충돌 범위가 접촉해야 한다.
    // 2. 둘 다 IsTrigger가 비활성화된 Collider가 있어야 한다.
    // 3. 둘 중 하나는 IsKinematic이 비활성화된 Rigidbody가 있어야 한다.
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"콜라이더 {collision.gameObject.name} 과 충돌함");
    }


    // 트리거 발동 조건
    // 1. 두 물체의 충돌 범위가 접촉해야 한다.
    // 2. 둘 다 Collider가 있어야 하며, 적어도 하나는 IsTrigger가 활성화되어 있어야 한다.
    // 3. (IsKinematic의 여부에 관계없이) 둘 중 하나는 Rigidbody가 있어야 한다.
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"트리거 {other.gameObject.name} 에 접촉함");
    }
}