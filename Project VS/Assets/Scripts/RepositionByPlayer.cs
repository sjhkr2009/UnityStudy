using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RepositionByPlayer : MonoBehaviour {
    [SerializeField] float moveDistance = Define.EnvironmentSetting.TileMapSize * 2;
    
    private void OnTriggerExit2D(Collider2D collider) {
        if (!collider.CompareTag(Define.Tag.Area)) return;
        if (!GameManager.Instance || !GameManager.Instance.Player) {
            Assert.IsTrue(false, "[RepositionByPlayer.OnTriggerExit2D] GameManager.Instance?.Player is null!");
            return;
        }

        var playerPos = GameManager.Instance.Player.transform.position;
        var myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        var playerDir = GameManager.Instance.Player.CurrentDirection;
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
        }
    }
}
