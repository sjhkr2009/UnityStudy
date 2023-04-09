using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapRepositioner : MonoBehaviour, IRepositionTarget {
    [SerializeField] float moveDistance = Define.EnvironmentSetting.TileMapSize * 2;
    
    public void Reposition(Transform pivotTransform) {
        var otherPos = pivotTransform.position;
        var myPos = transform.position;
        
        // X, Y축 중에 차이가 큰 방향으로 이동한다
        float diffX = Mathf.Abs(otherPos.x - myPos.x);
        float diffY = Mathf.Abs(otherPos.y - myPos.y);
        
        // 대상 위치의 반대방향으로 이동한다
        var moveDir = otherPos - myPos;
        var dirX = moveDir.x < 0 ? -1 : 1;
        var dirY = moveDir.y < 0 ? -1 : 1;

        if (diffX > diffY) {
            transform.Translate(Vector3.right * dirX * moveDistance);
        } else if (diffX < diffY) {
            transform.Translate(Vector3.up * dirY * moveDistance);
        }
    }
}
