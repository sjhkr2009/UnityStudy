using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine.Tilemaps;

#endif

public class MapEditor {
#if UNITY_EDITOR
    // 컨트롤(command), 알트(option), 쉬프트 == %, #, &
    [MenuItem("Custom Tools/Generate Map %m")]
    private static void GenerateMapInfo() {
        GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (var map in maps) {
            var mapBound = CustomUtility.FindChild<Tilemap>(map, "Tilemap_Base", true).cellBounds;
            Tilemap collisionMap = CustomUtility.FindChild<Tilemap>(map, "Tilemap_Collision", true);
            // FIXME: 세팅했다 지웠던 타일까지 cellBound 범위로 잡히는 경우가 있음. https://forum.unity.com/threads/tilemap-compressbounds.859426/ 이것과 관련된 듯한데, 확인 필요
            // 일단 맵 상단에 1줄을 더 계산하고 있어서 임시로 yMax에서 1을 뺀다
            WriteMapInfo(collisionMap, mapBound.xMin, mapBound.xMax, mapBound.yMin, mapBound.yMax - 1, map.name);
        }
    }

    static void WriteMapInfo(Tilemap collision, int xMin, int xMax, int yMin, int yMax, string saveFileName = null) {
        if (string.IsNullOrEmpty(saveFileName))
            saveFileName = collision.transform.parent.name;

        string directory = "Assets/Resources/Map/";
        if (Directory.Exists(directory) == false)
            Directory.CreateDirectory(directory);

        string filePath = $"{directory}{saveFileName}.txt";
        using (var writer = File.CreateText(filePath)) {
            writer.WriteLine(xMin);
            writer.WriteLine(xMax);
            writer.WriteLine(yMin);
            writer.WriteLine(yMax);

            for (int y = yMax; y >= yMin; --y) {
                for (int x = xMin; x <= xMax; ++x) {
                    var tile = collision.GetTile(new Vector3Int(x, y, 0));
                    writer.Write(tile != null ? "1" : "0");
                }
                writer.WriteLine();
            }
        }
        
        AssetDatabase.ImportAsset(filePath);
    }
#endif
}

