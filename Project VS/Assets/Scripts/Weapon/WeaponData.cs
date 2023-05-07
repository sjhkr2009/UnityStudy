using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class WeaponData {
    [BoxGroup("Main")] public WeaponType weaponType;
    [BoxGroup("Main")] public WeaponIndex weaponIndex;
    
    [BoxGroup("View")] public string itemName;
    [BoxGroup("View")] public string itemDesc;
    [BoxGroup("View")] public Sprite itemIcon;
    
    [BoxGroup("Spec")] public float baseDamage;
    [BoxGroup("Spec")] public int baseCount;
    [BoxGroup("Spec")] public List<float> damages;
    [BoxGroup("Spec")] public List<int> counts;
}
