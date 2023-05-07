using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(WeaponDataContainer), menuName = "Custom/Create Weapon Data")]
public class WeaponDataContainer : ScriptableObject {
    [SerializeField] private List<WeaponData> data = new List<WeaponData>();

    public IReadOnlyList<WeaponData> Data => data;
}
