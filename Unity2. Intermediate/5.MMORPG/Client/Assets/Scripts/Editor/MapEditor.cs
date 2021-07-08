using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.Tilemaps;

#endif

public class MapEditor
{
#if UNITY_EDITOR
    // 단축키는 MenuItem의 마지막에 띄어쓰기 후 붙일 수 있으며, 컨트롤(command)/알트(option)/쉬프트가 각각 %, #, & 로 표현된다.
    [MenuItem("Custom Tools/Hello World %#&h")]
    private static void HelloWorld() {
        if (EditorUtility.DisplayDialog("Hello World", "Create?", "OK", "No")) {
            new GameObject("Hello World");
        }
    }
    
    [MenuItem("Custom Tools/Generate Map %m")]
    private static void GenerateMapInfo() {
        GameObject[] maps = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (var map in maps) {
            Tilemap tilemap = CustomUtility.FindChild<Tilemap>(map, "Tilemap_Collision", true);
            WriteMapInfo(tilemap);
        }
    }

    static void WriteMapInfo(Tilemap tilemap) {
        using (var writer = File.CreateText("Assets/Resources/Map/mapinfo.txt")) {
            writer.WriteLine(tilemap.cellBounds.xMin);
            writer.WriteLine(tilemap.cellBounds.xMax);
            writer.WriteLine(tilemap.cellBounds.yMin);
            writer.WriteLine(tilemap.cellBounds.yMax);

            for (int y = tilemap.cellBounds.yMax; y >= tilemap.cellBounds.yMin; --y) {
                for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; ++x) {
                    var tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                    if(tile != null)
                        writer.Write("1");
                    else
                        writer.Write("0");
                }
                writer.WriteLine();
            }
        }
    }
    
#endif
}

