using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyParentTracker : MonoBehaviour {
    private void FixedUpdate() {
        transform.localPosition = Vector3.zero;
    }
}
