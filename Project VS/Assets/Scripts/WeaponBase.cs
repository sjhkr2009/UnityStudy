using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour {
    // TODO: 각 WeaponBase를 구분할 수 있는 identifier 추가 필요
    // TODO: 데미지 외에 범위/속도/연사력 등 공통 변수 WeaponBase에 추가
    public int level;
    public float damage;
    public abstract void Initialize(WeaponController controller);
    public abstract void OnUpdate(float deltaTime);
    public abstract void OnUpgrade();
    public abstract void Abandon();
}
