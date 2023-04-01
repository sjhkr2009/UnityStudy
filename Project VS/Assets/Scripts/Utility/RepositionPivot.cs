using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class RepositionPivot : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D collider) {
        var target = collider.GetComponent<IRepositionTarget>();
        target?.Reposition(transform);
    }
}
