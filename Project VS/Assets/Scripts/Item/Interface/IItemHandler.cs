using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemHandler {
    ItemData Data { get; }
    int Level { get; }
}
