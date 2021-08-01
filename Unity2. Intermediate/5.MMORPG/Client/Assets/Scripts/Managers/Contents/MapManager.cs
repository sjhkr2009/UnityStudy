using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager {
    private GameObject map;
    public Grid CurrentGrid { get; private set; }

    public int MinX { get; internal set; }
    public int MaxX { get; internal set; }
    public int MinY { get; internal set; }
    public int MaxY { get; internal set; }
    private bool[,] collisionData;

    private static string GetMapName(int id) => $"Map_{id:000}";
    
    public void LoadMap(int mapId) {
        DestroyMap();

        string mapName = GetMapName(mapId);
        map = Director.Resource.Instantiate($"Map/{mapName}");
        map.name = "Map";
        CurrentGrid = map.GetOrAddComponent<Grid>();

        var collisionMap = CustomUtility.FindChild(map, "Tilemap_Collision");
        if (collisionMap != null) 
            collisionMap.SetActive(false);

        TextAsset collisionText = Director.Resource.Load<TextAsset>($"Map/{mapName}");
        SetCollisionData(collisionText);
    }

    public void DestroyMap() {
        if (map == null) return;
        
        Object.Destroy(map);
        CurrentGrid = null;
    }

    private void SetCollisionData(TextAsset collitionInfoText) {
        StringReader reader = new StringReader(collitionInfoText.text);

        MinX = int.Parse(reader.ReadLine());
        MaxX = int.Parse(reader.ReadLine());
        MinY = int.Parse(reader.ReadLine());
        MaxY = int.Parse(reader.ReadLine());

        int yCount = MaxY - MinY + 1;
        int xCount = MaxX - MinX + 1;
        collisionData = new bool[yCount, xCount];

        for (int y = 0; y < yCount; y++) {
            string line = reader.ReadLine();
            if (line == null) break;
            
            for (int x = 0; x < xCount; x++) {
                collisionData[y, x] = line[x] != '0';
            }
        }
    }

    public bool CanGo(Vector3Int cellPos) {
        // 맵 범위 밖이면 false
        if (cellPos.x < MinX || cellPos.x > MaxX || cellPos.y < MinY || cellPos.y > MaxY) return false;
        
        // 맵 배열 내에서의 인덱스를 구한다. 맵 왼쪽 위를 원점으로 간주한다. 
        int x = cellPos.x - MinX;
        int y = MaxY - cellPos.y;
        
        bool existCollision = collisionData[y, x];
        return !existCollision;
    }
}
