using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Scanner : MonoBehaviour {
    [SerializeField] private float scanRange;
    [SerializeField] private LayerMask targetLayer;
    
    private RaycastHit2D[] targets;
    private Transform _nearestTarget;
    public Transform NearestTarget => _nearestTarget;
    public Transform RandomTarget => targets?.PickRandom().transform;

    private void FixedUpdate() {
        if (targetLayer <= 0) {
            Debugger.Error($"[EnemyScanner.FixedUpdate] Cannot Find Enemy TargetLayer: {Define.Layer.Enemy}");
            return;
        }
        
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, scanRange, targetLayer);
        _nearestTarget = GetNearest();
    }

    Transform GetNearest() {
        Transform result = null;
        float diff = float.MaxValue;

        foreach (var target in targets) {
            var myPos = transform.position;
            var targetPos = target.transform.position;
            var curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff) {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
