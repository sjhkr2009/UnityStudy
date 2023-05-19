using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackRangeModifier {
    float ModifyAttackRange(float prevValue);
}
