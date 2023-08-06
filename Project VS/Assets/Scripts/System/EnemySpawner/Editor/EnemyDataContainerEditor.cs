using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyDataContainer))]
public class EnemyDataContainerEditor : Editor {
    private bool showRawData;
    
    public override void OnInspectorGUI() {
        EnemyDataContainer enemyDataContainer = (EnemyDataContainer)target;
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(enemyDataContainer), typeof(MonoScript), false);
        EditorGUI.EndDisabledGroup();
        if (GUILayout.Button("초기화")) enemyDataContainer.Initialize();
        
        serializedObject.Update();

        var statDatasProperty = serializedObject.FindProperty(nameof(enemyDataContainer.statDatas));
        for (int i = 0; i < enemyDataContainer.statDatas.Count; i++) {
            DrawStatDataEditor(enemyDataContainer.statDatas[i], statDatasProperty.GetArrayElementAtIndex(i));
        }

        serializedObject.ApplyModifiedProperties();
        
        if (GUILayout.Button("Save Data")) {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
        
        EditorGUILayout.Space(10);

        showRawData = EditorGUILayout.Foldout(showRawData, "Show Raw Data");
        if (showRawData) DrawDefaultInspector();
    }
    
    private void DrawStatDataEditor(EnemyStatData statData, SerializedProperty property) {
        EditorGUILayout.Space(10);
        
        EditorGUILayout.LabelField($"EnemyStatData: {statData.enemyIndex}");
        EditorGUILayout.Space();
        statData.tier = (EnemyTier)EditorGUILayout.EnumPopup("Enemy Tier", statData.tier);
        statData.attackDamage = EditorGUILayout.FloatField("Attack Damage", statData.attackDamage);
        statData.speed = EditorGUILayout.FloatField("Speed", statData.speed);
        statData.hp = EditorGUILayout.FloatField("HP", statData.hp);
        statData.mass = EditorGUILayout.FloatField("Mass", statData.mass);

        if (statData.dropTables == null) {
            statData.dropTables = new List<DropTable>();
        }
        
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(property.FindPropertyRelative(nameof(statData.dropTables)));
        EditorGUI.indentLevel--;
        GUILayout.Label(string.Empty, GUI.skin.horizontalSlider);
    }
}