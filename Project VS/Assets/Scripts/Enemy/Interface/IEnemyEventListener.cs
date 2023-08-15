using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyEventListener {
    void OnCreate();
    void OnUpdate(float deltaTime);
    void OnHit(AbilityBase hitAbility);
    void OnDead();
}
