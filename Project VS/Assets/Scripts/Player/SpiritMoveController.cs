using System;
using UnityEngine;

public class SpiritMoveController : MonoBehaviour {
    [SerializeField] private Transform scaleRoot;
    private void FixedUpdate() {
        if (GameManager.Player == null) return;
        if (GameManager.IsPause) return;

        var player = GameManager.Player;
        var dest = player.View?.SpiritDestination;
        if (dest == null) return;

        if (Vector2.Distance(transform.position, dest.position) < 0.05f) return;
        transform.position = Vector2.Lerp(transform.position, dest.position, 0.066f);
        scaleRoot.localScale = new Vector3(transform.position.x < dest.position.x ? -1f : 1f, 1f, 1f);
    }
}
