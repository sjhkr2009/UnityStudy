using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class PlayerMover : MonoBehaviour {
    [SerializeField] float maxAngulerSpeed; //초당 최대 회전각
    [SerializeField] private Transform center;
    
    // TODO: center 유무에 따른 IMoveStrategy 교체
    private IMoveStrategy moveStrategy;

    private void Awake() {
        moveStrategy = new OrbitMoveStrategy(transform, center, maxAngulerSpeed, transform.IsLeftFromCenter(center));
    }
    
    private void FixedUpdate() {
        moveStrategy.Move();
    }
}