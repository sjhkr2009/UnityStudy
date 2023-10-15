using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestCollision : MonoBehaviour {
    [AutoAssignComponent] public Tilemap tilemap;
    public TileBase tile;

    private void Start() {
        tilemap.SetTile(Vector3Int.zero, tile);
    }

    private void Update() {
        List<Vector3Int> blocked = new List<Vector3Int>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
            var t = tilemap.GetTile(pos);
            if (t != null) {
                blocked.Add(pos);
            }
        }
    }
}
