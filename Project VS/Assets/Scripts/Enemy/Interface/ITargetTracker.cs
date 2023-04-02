using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetTracker {
    Rigidbody2D Target { get; }
    void SetTarget(Rigidbody2D target);
}
