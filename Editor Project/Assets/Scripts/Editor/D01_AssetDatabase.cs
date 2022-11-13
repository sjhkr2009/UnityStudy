using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class D01_AssetDatabase : EditorWindow {
    [MenuItem("Custom/04-1 Asset Database", false, 4000)]
    static void OpenWindow() {
        GetWindow<D01_AssetDatabase>();
    }

    private void OnGUI() {
        if (GUILayout.Button("모든 EventManager 찾기")) {
            // AssetDatabase.FindAssets로 프로젝트 내 모든 파일의 GUID를 찾을 수 있다.
            // 필터는 "t: 타입명", "l: 라벨명" 을 지원하며, 타입명은 대소문자 무관. 라벨은 인스펙터 하단의 AssetBundle 라벨을 의미.
            // "t:scriptableobject t:EventManaGer" 등으로 입력 시 두 타입 중 하나라도 해당하면 찾아온다. 두번째 파라미터로 찾을 폴더들을 지정할 수 있다.
            var results = AssetDatabase.FindAssets("t:EvEntmAnaGer", new []{"Assets/Custom/Resources"});
            if (results.Length > 0) {
                foreach (var result in results) {
                    // GUID <-> Path 상호 변환 가능
                    var path = AssetDatabase.GUIDToAssetPath(result);
                    var guid = AssetDatabase.GUIDFromAssetPath(path);
                    Debug.Log($"GUID: {result} / Path: {path} / Rev: {guid}");
                }
                
                // AssetDatabase.LoadAssetAtPath로 특정 폴더내의 에셋을 가져올 수 있다.
                var first = AssetDatabase.LoadAssetAtPath<EventManager>(AssetDatabase.GUIDToAssetPath(results[0]));
                Debug.Log("Unlock Level: " + first.unlockLevel);
            }
        }

        if (GUILayout.Button("EventManager 추가 생성")) {
            var results = AssetDatabase.FindAssets("t:EvEntmAnaGer", new []{"Assets/Custom/Resources"});
            var filePath = $"Assets/Custom/Resources/EventManager{results.Length + 1}.asset";
            var newAsset = ScriptableObject.CreateInstance<EventManager>();
            
            // AssetDatabase.CreateAsset으로 해당 경로에 에셋을 생성할 수 있다. path에는 확장자까지 입력되어야 함에 유의.
            AssetDatabase.CreateAsset(newAsset, filePath);
            
            // 참고로 File.WriteAllText 등 다른 방식으로 파일을 쓰면 메타파일이 자동 생성되지 않는데, AssetDatabase.Refresh로 프로젝트를 갱신할 수 있다.
            // AssetDatabase.Refresh는 현재 프로젝트가 에디터 윈도우와 차이가 있다면 에셋을 리로드한다.
            // 프로젝트가 커서 이 동작이 무겁다면, AssetDatabase.ImportAsset(path) 로 특정 에셋만 불러올 수 있다. 
        }
    }
}
