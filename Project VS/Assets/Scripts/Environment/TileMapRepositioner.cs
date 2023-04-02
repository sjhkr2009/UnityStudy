using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapRepositioner : MonoBehaviour, IRepositionTarget {
    [SerializeField] float moveDistance = Define.EnvironmentSetting.TileMapSize * 2;
    
    public void Reposition(Transform pivotTransform) {
        var player = GameManager.Instance.Player;
        var playerPos = player.transform.position;
        var myPos = transform.position;
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);

        var playerDir = player.GetStatus.InputVector;
        var dirX = playerDir.x < 0 ? -1 : 1;
        var dirY = playerDir.y < 0 ? -1 : 1;

        if (diffX > diffY) {
            transform.Translate(Vector3.right * dirX * moveDistance);
        } else if (diffX < diffY) {
            transform.Translate(Vector3.up * dirY * moveDistance);
        }
    }
}
