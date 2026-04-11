using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class AutoAssignChecker : EditorWindow
{
    private struct PrefabCheckResult
    {
        public string PrefabName;
        public string PrefabAssetPath;
        public List<AssignResult> Fields;
    }

    private struct AssignResult
    {
        public string ObjectPath;
        public string ComponentName;
        public string FieldType;
        public string FieldName;
        public string AutoAssignPath;
        public AutoAssignUtility.FieldResult Result;
    }

    private string _folderPath = "Assets/";
    private bool _isRunning;
    private int _processedCount;
    private int _totalCount;
    private Vector2 _scrollPos;
    private readonly List<PrefabCheckResult> results = new();

    [MenuItem("Tools/AutoAssign 자동 할당")]
    public static void ShowWindow()
    {
        GetWindow<AutoAssignChecker>("AutoAssign Checker");
    }

    private void OnGUI()
    {
        DrawFolderField();

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(_isRunning);
        if (GUILayout.Button("실행", GUILayout.Height(28)))
        {
            RunAsync();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();
        DrawResultLog();
    }

    private void DrawFolderField()
    {
        EditorGUILayout.BeginHorizontal();
        _folderPath = EditorGUILayout.TextField("폴더 경로", _folderPath);

        if (GUILayout.Button("폴더 선택", GUILayout.Width(100)))
        {
            var selected = EditorUtility.OpenFolderPanel("폴더 선택", _folderPath, "");
            if (!string.IsNullOrEmpty(selected))
            {
                var dataPath = Application.dataPath;
                if (selected.StartsWith(dataPath, StringComparison.Ordinal))
                    _folderPath = "Assets" + selected.Substring(dataPath.Length);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawResultLog()
    {
        if (results.Count == 0)
            return;

        int totalFields = 0;
        int successCount = 0;
        int failCount = 0;
        foreach (var prefab in results)
        {
            foreach (var f in prefab.Fields)
            {
                totalFields++;
                if (f.Result == AutoAssignUtility.FieldResult.Assigned)
                    successCount++;
                else
                    failCount++;
            }
        }

        EditorGUILayout.LabelField($"결과 - 프리팹 {results.Count}개 / 필드 {totalFields}개 발견 | 성공 {successCount} | 실패 {failCount}", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

        foreach (var prefabResult in results)
        {
            DrawPrefabResult(prefabResult);
        }

        EditorGUILayout.EndScrollView();
    }

    private static void DrawPrefabResult(PrefabCheckResult prefabResult)
    {
        bool hasFailure = false;
        foreach (var f in prefabResult.Fields)
        {
            if (f.Result != AutoAssignUtility.FieldResult.Assigned)
            {
                hasFailure = true;
                break;
            }
        }

        var prevBg = GUI.backgroundColor;
        GUI.backgroundColor = hasFailure
            ? new Color(1f, 0.7f, 0.7f)
            : new Color(0.7f, 1f, 0.7f);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUI.backgroundColor = prevBg;

        EditorGUILayout.LabelField($"{prefabResult.PrefabName}  ({prefabResult.Fields.Count}건)", EditorStyles.boldLabel);

        foreach (var field in prefabResult.Fields)
        {
            DrawFieldResult(field);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(2);
    }

    private static void DrawFieldResult(AssignResult result)
    {
        var icon = result.Result switch
        {
            AutoAssignUtility.FieldResult.Assigned => "d_GreenCheckmark",
            _ => "console.erroricon.sml",
        };

        var label = result.Result switch
        {
            AutoAssignUtility.FieldResult.Assigned => "할당 성공",
            AutoAssignUtility.FieldResult.PathNotFound => "경로 탐색 실패",
            AutoAssignUtility.FieldResult.ComponentNotFound => "컴포넌트 없음",
            AutoAssignUtility.FieldResult.UnsupportedType => "미지원 타입",
            _ => "알 수 없음",
        };

        using (new EditorGUILayout.HorizontalScope())
        {
            var iconContent = EditorGUIUtility.IconContent(icon);
            GUILayout.Label(iconContent, GUILayout.Width(18), GUILayout.Height(18));

            var objName = string.IsNullOrEmpty(result.ObjectPath) ? "Root" : result.ObjectPath;
            EditorGUILayout.LabelField($"[{label}]  [{result.ComponentName}] {result.FieldType} {result.FieldName} -> '{result.AutoAssignPath}'  ({objName})");
        }
    }

    private async Task RunAsync()
    {
        _isRunning = true;
        results.Clear();
        Repaint();

        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { _folderPath });
        _totalCount = guids.Length;
        _processedCount = 0;

        if (_totalCount == 0)
        {
            EditorUtility.DisplayDialog("AutoAssignChecker", "해당 폴더에 프리팹이 없습니다.", "확인");
            _isRunning = false;
            Repaint();
            return;
        }

        bool cancelled = false;
        var lastWaitTime = EditorApplication.timeSinceStartup;

        try
        {
            AssetDatabase.StartAssetEditing();

            foreach (var guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);

                if (prefabRoot != null)
                {
                    bool dirty = ProcessPrefab(prefabRoot, assetPath);

                    if (dirty)
                        PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath);

                    PrefabUtility.UnloadPrefabContents(prefabRoot);
                }

                _processedCount++;

                var now = EditorApplication.timeSinceStartup;
                if (now - lastWaitTime >= 0.1)
                {
                    lastWaitTime = now;
                    cancelled = EditorUtility.DisplayCancelableProgressBar(
                        "AutoAssignChecker",
                        $"프리팹 확인 중... ({_processedCount} / {_totalCount})",
                        (float)_processedCount / _totalCount);

                    if (cancelled)
                        break;

                    await Awaitable.NextFrameAsync();
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            EditorUtility.ClearProgressBar();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            _isRunning = false;
            Repaint();
        }

        int totalFields = 0;
        int assignedCount = 0;
        foreach (var p in results)
        {
            foreach (var f in p.Fields)
            {
                totalFields++;
                if (f.Result == AutoAssignUtility.FieldResult.Assigned)
                    assignedCount++;
            }
        }

        if (cancelled)
            Debug.Log($"AutoAssignChecker.Run :: 취소됨 ({_processedCount} / {_totalCount})");
        else
            Debug.Log($"AutoAssignChecker.Run :: 완료 - 총 {totalFields}건 (성공 {assignedCount}, 실패 {totalFields - assignedCount})");
    }

    private bool ProcessPrefab(GameObject prefabRoot, string assetPath)
    {
        var root = prefabRoot.transform;
        bool dirty = false;
        var fieldResults = new List<AssignResult>();

        foreach (var component in prefabRoot.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (component == null)
                continue;

            var type = component.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var att = field.GetCustomAttribute<AutoAssignAttribute>();
                if (att == null)
                    continue;

                var value = field.GetValue(component);
                if ((Object)value != null)
                    continue;

                var targetPath = att.Path;
                if (string.IsNullOrEmpty(targetPath))
                    continue;

                var fieldResult = AutoAssignUtility.TryAssignField(component, field, root, targetPath, out _);

                fieldResults.Add(new AssignResult
                {
                    ObjectPath = AutoAssignUtility.GetInnerPath(root, component.transform),
                    ComponentName = type.Name,
                    FieldType = field.FieldType.Name,
                    FieldName = field.Name,
                    AutoAssignPath = targetPath,
                    Result = fieldResult,
                });

                if (fieldResult == AutoAssignUtility.FieldResult.Assigned)
                    dirty = true;
            }
        }

        if (fieldResults.Count > 0)
        {
            results.Add(new PrefabCheckResult
            {
                PrefabName = root.name,
                PrefabAssetPath = assetPath,
                Fields = fieldResults,
            });
        }

        if (dirty)
            EditorUtility.SetDirty(prefabRoot);

        return dirty;
    }
}
