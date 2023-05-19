using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackPowerModifier {
    float ModifyAttackPower(float prevValue);
}
