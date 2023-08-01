using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 지속적으로 공격하는 능력의 인터페이스 */
public interface IWeaponAbility {
    Transform Transform { get; set; }
    void OnEveryFrame(float deltaTime) { }
}
