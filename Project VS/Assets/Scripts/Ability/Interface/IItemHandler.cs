using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemHandler {
    AbilityData Data { get; }
    int Level { get; }
}
