using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalShoes : ItemBase, ISpeedModifier {
    public override ItemIndex Index => ItemIndex.NormalShoes;
    public float ModifySpeed(float prevSpeed) {
        var factor = Data.GetValue(EquipmentValueType.MoveSpeed, Level);
        return prevSpeed * factor;
    }
}