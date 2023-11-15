using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager {
    public MyPlayerController MyPlayer { get; set; }
    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();

    public void AddPlayer(PlayerInfo info, bool isMyPlayer = false) {
        var gameObject = Director.Resource.Instantiate(isMyPlayer ? "MyPlayer" : "Player");
        gameObject.name = info.Name;
        _objects.Add(info.PlayerId, gameObject);

        var controller = gameObject.GetComponent<BaseController>();
        controller.Id = info.PlayerId;
        controller.CellPos = new Vector3Int(info.PosInfo.PosX, info.PosInfo.PosY, 0);
        
        if (isMyPlayer) {
            MyPlayer = gameObject.GetComponent<MyPlayerController>();
        }
    }

    public void RemoveMyPlayer() {
        if (MyPlayer == null) return;
        
        Remove(MyPlayer.Id);
        MyPlayer = null;
    }
    
    public void Add(int id, GameObject gameObject) {
        _objects.Add(id, gameObject);
    }

    public void Remove(int id) {
        _objects.Remove(id);
    }

    public void Clear(GameObject gameObject) {
        _objects.Clear();
    }

    public GameObject Find(Vector3Int cellPos) {
        foreach (var gameObject in _objects.Values) {
            var controller = gameObject.GetComponent<BaseController>();
            if (controller == null) continue;
            if (controller.CellPos == cellPos) return gameObject;
        }

        return null;
    }
    
    public T Find<T>(Vector3Int cellPos) where T : BaseController {
        foreach (var gameObject in _objects.Values) {
            var controller = gameObject.GetComponent<T>();
            if (controller == null) continue;
            if (controller.CellPos == cellPos) return controller;
        }

        return null;
    }

    public T FindIf<T>(Func<T, bool> condition) where T : Component {
        foreach (var gameObject in _objects.Values) {
            var component = gameObject.GetComponent<T>();
            if (!component) continue;
            if (condition.Invoke(component)) return component;
        }

        return null;
    }
}
