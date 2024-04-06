using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategy {
    public enum WeaponType {
        Calibrum,
        Severum,
        Gravitum,
        Infernum,
        Crescendum
    }
    
    public interface IWeaponStrategy {
        WeaponType WeaponType { get; }
        void ApplyPassive(IPlayerContext player);
        void Reset(IPlayerContext player);
        void OnAttack(Transform origin, Transform target);
        void OnSkill(Transform origin, Vector3 targetPoint);
    }
}
