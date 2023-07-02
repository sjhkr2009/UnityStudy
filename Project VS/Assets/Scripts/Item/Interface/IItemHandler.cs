using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemHandler {
    EquipmentData Data { get; }
    int Level { get; }
}
