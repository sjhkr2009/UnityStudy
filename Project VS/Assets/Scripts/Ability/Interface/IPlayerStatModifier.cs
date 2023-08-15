using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerStatModifier {
    void ApplyModify(PlayerStatData playerStatusData);
}
