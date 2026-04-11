using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

[InitializeOnLoad]
public static class AutoAssignPrefabHelper
{
    private static readonly Dictionary<int, List<string>> _unresolvedFields = new();
    private static readonly HashSet<int> _assignedObjects = new();
    private static readonly HashSet<int> _mismatchedObjects = new();

    static AutoAssignPrefabHelper()
    {
        PrefabStage.prefabSaving -= OnPrefabSaving;
        PrefabStage.prefabSaving += OnPrefabSaving;

        PrefabStage.prefabStageOpened -= OnPrefabStageOpened;
        PrefabStage.prefabStageOpened += OnPrefabStageOpened;

        PrefabStage.prefabStageClosing -= OnPrefabStageClosing;
        PrefabStage.prefabStageClosing += OnPrefabStageClosing;

        Selection.selectionChanged -= OnSelectionChanged;
        Selection.selectionChanged += OnSelectionChanged;

        EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyGUI;
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    // ──────────────────────────── Hierarchy GUI ────────────────────────────

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        // 경로 찾지 못함
        if (_unresolvedFields.Count > 0 && _unresolvedFields.ContainsKey(instanceID))
        {
            EditorGUI.DrawRect(selectionRect, new Color(1f, 0f, 0f, 0.2f));
            return;
        }

        // 경로 불일치 (수동 할당 또는 경로 변경됨)
        if (_mismatchedObjects.Contains(instanceID))
        {
            EditorGUI.DrawRect(selectionRect, new Color(1f, 1f, 0f, 0.2f));
            return;
        }

        // 정상 할당됨
        if (_assignedObjects.Contains(instanceID))
        {
            EditorGUI.DrawRect(selectionRect, new Color(0f, 0f, 1f, 0.2f));
        }
    }

    private static void OnSelectionChanged()
    {
        _assignedObjects.Clear();
        _mismatchedObjects.Clear();

        var stage = PrefabStageUtility.GetCurrentPrefabStage();
        if (stage == null)
            return;

        var root = stage.prefabContentsRoot.transform;

        foreach (var go in Selection.gameObjects)
        {
            foreach (var component in go.GetComponents<MonoBehaviour>())
            {
                if (component == null)
                    continue;

                CheckAssignedTargets(component, root);
            }
        }

        EditorApplication.RepaintHierarchyWindow();
    }

    private static void CheckAssignedTargets(MonoBehaviour component, Transform root)
    {
        var type = component.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var att = field.GetCustomAttribute<AutoAssignAttribute>();
            if (att == null)
                continue;

            var value = field.GetValue(component);
            if ((Object)value == null)
                continue;

            var targetTr = AutoAssignUtility.GetTransformFromValue(value);
            if (targetTr == null)
                continue;

            var fullPath = AutoAssignUtility.GetInnerPath(root, targetTr);
            int id = targetTr.gameObject.GetInstanceID();

            if (AutoAssignUtility.MatchesPathSuffix(fullPath, att.Path))
                _assignedObjects.Add(id);
            else
                _mismatchedObjects.Add(id);
        }
    }

    private static void OnPrefabStageClosing(PrefabStage stage)
    {
        _unresolvedFields.Clear();
        _assignedObjects.Clear();
        _mismatchedObjects.Clear();
    }

    private static void OnPrefabSaving(GameObject root)
    {
        _unresolvedFields.Clear();

        foreach (var component in root.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (component == null)
                continue;

            ProcessComponentOnSave(component, root.transform);
        }

        OnSelectionChanged();

        if (_unresolvedFields.Count > 0)
            EditorApplication.RepaintHierarchyWindow();
    }

    private static void OnPrefabStageOpened(PrefabStage stage)
    {
        _unresolvedFields.Clear();

        var root = stage.prefabContentsRoot;

        foreach (var component in root.GetComponentsInChildren<MonoBehaviour>(true))
        {
            if (component == null)
                continue;

            CheckFieldsOnOpen(component, root.transform);
        }

        if (_unresolvedFields.Count > 0)
            EditorApplication.RepaintHierarchyWindow();
    }
    
    private static void ProcessComponentOnSave(MonoBehaviour component, Transform root)
    {
        var type = component.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        bool dirty = false;

        foreach (var field in fields)
        {
            var att = field.GetCustomAttribute<AutoAssignAttribute>();
            if (att == null)
                continue;

            var targetPath = att.Path;
            if (string.IsNullOrEmpty(targetPath))
                continue;

            var result = AutoAssignUtility.TryAssignField(component, field, root, targetPath, out var foundTransform);

            switch (result)
            {
                case AutoAssignUtility.FieldResult.Assigned:
                    dirty = true;
                    break;

                case AutoAssignUtility.FieldResult.PathNotFound:
                    Debug.LogError(
                        $"AutoAssign.Save :: '{type.Name}.{field.Name}' | '{targetPath}'를 '{root.name}' 하위에서 찾을 수 없습니다.",
                        component);
                    TrackUnresolved(component.gameObject, $"{field.Name} ({targetPath})");
                    break;

                case AutoAssignUtility.FieldResult.ComponentNotFound:
                    Debug.LogError(
                        $"AutoAssign.Save :: '{type.Name}.{field.Name}' | '{AutoAssignUtility.GetInnerPath(root, foundTransform)}'에 '{field.FieldType.Name}' 타입의 컴포넌트가 없습니다.",
                        component);
                    TrackUnresolved(component.gameObject, $"{field.Name} ({field.FieldType.Name})");
                    break;

                case AutoAssignUtility.FieldResult.UnsupportedType:
                    Debug.LogError(
                        $"AutoAssign.Save :: '{type.Name}.{field.Name}' | '{field.FieldType.Name}' 타입은 지원되지 않습니다. GameObject, Transform 또는 Component 타입만 사용할 수 있습니다.",
                        component);
                    TrackUnresolved(component.gameObject, $"{field.Name} (Not Supported)");
                    break;
            }
        }

        if (dirty)
            EditorUtility.SetDirty(component);
    }

    private static void CheckFieldsOnOpen(MonoBehaviour component, Transform root)
    {
        var type = component.GetType();
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            var att = field.GetCustomAttribute<AutoAssignAttribute>();
            if (att == null)
                continue;

            var value = field.GetValue(component);

            // SerializeField는 비어 있어도 fake null 상태라 Object로 캐스팅해서 검사
            if ((Object)value == null)
            {
                Debug.LogError(
                    $"AutoAssign.Open :: '{type.Name}.{field.Name}' | 할당되지 않은 필드가 발견되었습니다. 경로: '{att.Path}'",
                    component);
                TrackUnresolved(component.gameObject, $"{field.Name} (\"{att.Path}\")");
                continue;
            }

            // 할당은 되어있지만 경로가 일치하는지 검사
            var assignedTransform = AutoAssignUtility.GetTransformFromValue(value);
            if (assignedTransform == null)
                continue;

            var fullPath = AutoAssignUtility.GetInnerPath(root, assignedTransform);

            if (!AutoAssignUtility.MatchesPathSuffix(fullPath, att.Path))
            {
                Debug.LogError(
                    $"AutoAssign.Open :: '{type.Name}.{field.Name}' | 할당된 오브젝트 '{fullPath}'가 지정된 경로 '{att.Path}'와 일치하지 않습니다.",
                    component);
                TrackUnresolved(component.gameObject, $"{field.Name} (\"{att.Path}\" ≠ \"{fullPath}\")");
            }
        }
    }

    private static void TrackUnresolved(GameObject obj, string description)
    {
        var id = obj.GetInstanceID();
        if (!_unresolvedFields.TryGetValue(id, out var list))
        {
            list = new List<string>();
            _unresolvedFields[id] = list;
        }
        list.Add(description);
    }
}

