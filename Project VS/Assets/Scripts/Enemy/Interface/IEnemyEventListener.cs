using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyEventListener {
    void OnCreate();
    void Update();
    void OnHit();
    void OnDead();
}
