using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveStrategy {
    Transform Transform { get; }
    float Speed { get; }
    void Move();
}
