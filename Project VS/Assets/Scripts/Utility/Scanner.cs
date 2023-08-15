using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Scanner : MonoBehaviour {
    [SerializeField] private float scanRange;
    [SerializeField] private LayerMask targetLayer;

    private RaycastHit2D[] targets;
    private Transform _nearestTarget;
    public Transform NearestTarget => _nearestTarget;
    public Transform RandomTarget => targets?.PickRandom().transform;

    public Transform GetNearestTargetFrom(Vector2 pos) {
        if (targets == null || targets.Length == 0) return null;
        return targets.OrderByDescending(t => Vector2.Distance(t.transform.position, pos)).First().transform;
    }

    private void FixedUpdate() {
        if (targetLayer <= 0) {
            Debugger.Error($"[EnemyScanner.FixedUpdate] Cannot Find Enemy TargetLayer: {Define.Layer.Enemy}");
            return;
        }

        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, scanRange, targetLayer)
            .Where(g => GameManager.Ability.IsInCameraView(g.transform.position))
            .ToArray();
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
