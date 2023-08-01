using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBulletCreator {
    float BulletSpeed { get; }
    float Penetration { get; }
}
    