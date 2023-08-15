using UnityEngine;

public class PlayerDirectionTracker : MonoBehaviour {
    private void Update() {
        if (!GameManager.Player) return;

        var showDirection = GameManager.Player.Status.ShowDirection;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, showDirection);
    }
}
