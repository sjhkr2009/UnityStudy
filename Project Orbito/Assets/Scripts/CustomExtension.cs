using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomExtension {
    public static bool IsLeftFromCenter(this Transform target, Transform center) {
        Vector3 direction = center.position - target.position;
        var crossProduct = Vector3.Cross(direction, target.forward);
        return crossProduct.y > 0;
    }
}
