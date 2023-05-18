using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGlove : ItemBase, IAttackRangeModifier {
    public override ItemIndex Index => ItemIndex.NormalGlove;
    public float ModifyAttackRange(float prevValue) {
        var factor = Data.GetMainValue(Level);
        return prevValue * factor;
    }
}
