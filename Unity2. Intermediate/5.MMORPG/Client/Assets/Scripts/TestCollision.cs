using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestCollision : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tile;
    
    List<Vector3Int> blocks = new List<Vector3Int>();
    private void Start()
    {
        // 타일맵의 경계 내 모든 좌표값을 탐색한다.
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            //타일을 가져와서, 빈 칸이 아니면(타일이 있으면) 해당 위치를 blocked에 저장한다.
            TileBase tileBase = tilemap.GetTile(pos);
            if(tileBase != null) blocks.Add(pos);
            //반대로 SetTile로 동적으로 타일맵에 타일을 배치할 수도 있다.
            //tilemap.SetTile(pos, tile);
        }

        foreach (var block in blocks)
        {
            Debug.Log(block);
        }
    }
}
