using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(C04_Example))]
public class C04_SceneGUI : Editor {
    private C04_Example targetRef;
    
    private void OnEnable() {
        //SceneView.duringSceneGui += OnSceneGUI;
        
        targetRef = (C04_Example)target;
    }

    private void OnDisable() {
        //SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI() {
        var myPos = targetRef.transform.position;
        Handles.Label(myPos, "My Object");

        if (targetRef.otherObjects == null) return;
        float maxDist = 0.1f;
        
        foreach (var otherObj in targetRef.otherObjects) {
            if (!otherObj) continue;

            var targetPos = otherObj.transform.position;
            Handles.DrawLine(myPos, targetPos);
            
            Handles.color = Color.green;
            var dist = Vector3.Distance(myPos, targetPos);
            maxDist = Math.Max(dist, maxDist);
            Handles.Label(targetPos, $"Other - {dist}");
            
            Handles.DrawWireCube(targetPos, Vector3.one * Math.Clamp(dist * 0.2f, 0.5f, 3f));
            Handles.color = Color.white;
        }
        
        Handles.CircleHandleCap(0, myPos, Quaternion.identity, maxDist, EventType.Repaint);

        Handles.BeginGUI();

        if (GUILayout.Button("Connect All Objects")) {
            var objs = FindObjectsOfType<GameObject>().ToList();
            objs.Remove(targetRef.gameObject);
            targetRef.otherObjects = objs.ToArray();
        }

        if (GUILayout.Button("Remove null Objects")) {
            var newArray = new List<GameObject>();
            foreach (var otherObject in targetRef.otherObjects) {
                if (otherObject) newArray.Add(otherObject);
            }

            targetRef.otherObjects = newArray.ToArray();
        }
        
        Handles.EndGUI();
    }
}
