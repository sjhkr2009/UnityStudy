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
            if (controller.cellPos == cellPos) return gameObject;
        }

        return null;
    }
}
