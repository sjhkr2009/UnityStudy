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
            var mapBound = CustomUtility.FindChild<Tilemap>(map, "Tilemap_Base", true).cellBounds;
            Tilemap collisionMap = CustomUtility.FindChild<Tilemap>(map, "Tilemap_Collision", true);
            WriteMapInfo(collisionMap, mapBound.xMin, mapBound.xMax, mapBound.yMin, mapBound.yMax, map.name);
        }
    }

    static void WriteMapInfo(Tilemap collision, int xMin, int xMax, int yMin, int yMax, string saveFileName = null) {
        if (string.IsNullOrEmpty(saveFileName))
            saveFileName = collision.transform.parent.name;

        string directory = "Assets/Resources/Map/";
        if (Directory.Exists(directory) == false)
            Directory.CreateDirectory(directory);
        
        using (var writer = File.CreateText($"{directory}{saveFileName}.txt")) {
            writer.WriteLine(xMin);
            writer.WriteLine(xMax);
            writer.WriteLine(yMin);
            writer.WriteLine(yMax);

            for (int y = yMax; y >= yMin; --y) {
                for (int x = xMin; x < xMax; ++x) {
                    var tile = collision.GetTile(new Vector3Int(x, y, 0));
                    if(tile != null)
                        writer.Write("1");
                    else
                        writer.Write("0");
                }
                writer.WriteLine();
            }
        }
    }

    static void WriteMapInfo(Tilemap map, string saveFileName = null) {
        var bound = map.cellBounds;
        WriteMapInfo(map, bound.xMin, bound.xMax, bound.yMin, bound.yMax);
    }
    
#endif
}

