using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {
    private List<GameObject> _objects = new List<GameObject>();

    public void Add(GameObject gameObject) {
        _objects.Add(gameObject);
    }

    public void Remove(GameObject gameObject) {
        _objects.Remove(gameObject);
    }

    public void Clear(GameObject gameObject) {
        _objects.Clear();
    }

    public GameObject Find(Vector3Int cellPos) {
        foreach (var gameObject in _objects) {
            var controller = gameObject.GetComponent<BaseController>();
            if (controller == null) continue;
            if (controller.CellPos == cellPos) return gameObject;
        }

        return null;
    }
    
    public T Find<T>(Vector3Int cellPos) where T : BaseController {
        foreach (var gameObject in _objects) {
            var controller = gameObject.GetComponent<T>();
            if (controller == null) continue;
            if (controller.CellPos == cellPos) return controller;
        }

        return null;
    }

    public T FindIf<T>(Func<T, bool> condition) where T : Component {
        foreach (var gameObject in _objects) {
            var component = gameObject.GetComponent<T>();
            if (!component) continue;
            if (condition.Invoke(component)) return component;
        }

        return null;
    }
}
