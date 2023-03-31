using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class RepositionByPlayer : MonoBehaviour {
    [SerializeField] float moveDistance = Define.EnvironmentSetting.TileMapSize * 2;
    
    private void OnTriggerExit2D(Collider2D collider) {
        if (!collider.CompareTag(Define.Tag.Area)) return;
        
        Reposition(collider);
    }

    void Reposition(Collider2D collider) {
        if (!GameManager.Instance || !GameManager.Instance.Player) {
            Assert.IsTrue(false, "[RepositionByPlayer.OnTriggerExit2D] GameManager.Instance?.Player is null!");
            return;
        }

        var player = GameManager.Instance.Player;
        var playerPos = player.transform.position;
        var myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        var playerDir = player.ClonedStatus.inputVector;
        var dirX = playerDir.x < 0 ? -1 : 1;
        var dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag) {
            case Define.Tag.Ground:
                if (diffX > diffY) {
                    transform.Translate(Vector3.right * dirX * moveDistance);
                } else if (diffX < diffY) {
                    transform.Translate(Vector3.up * dirY * moveDistance);
                }
                break;
            case Define.Tag.Enemy:
                if (!collider.enabled) return;
                transform.Translate(playerDir * moveDistance * 0.5f + new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)));
                break;
        }
    }
}
