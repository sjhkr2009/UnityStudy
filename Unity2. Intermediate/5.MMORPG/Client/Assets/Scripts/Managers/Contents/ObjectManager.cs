using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf.Protocol;
using UnityEngine;

public class ObjectManager {
    public MyPlayerController MyPlayer { get; set; }
    private Dictionary<int, GameObject> _objects = new Dictionary<int, GameObject>();
    
    public void Add(ObjectInfo info, bool isMyPlayer = false) {
        var objectType = info.ObjectId.GetObjectType();

        switch (objectType) {
            case GameObjectType.Player:
                AddPlayer(info, isMyPlayer);
                break;
            case GameObjectType.Monster:
                break;
            case GameObjectType.Projectile:
                AddProjectile(info);
                break;
        }
    }

    private void AddPlayer(ObjectInfo info, bool isMyPlayer) {
        var gameObject = Director.Resource.Instantiate(isMyPlayer ? "MyPlayer" : "Player");
        gameObject.name = info.Name;
        _objects.Add(info.ObjectId, gameObject);

        var controller = gameObject.GetComponent<BaseController>();
        controller.Id = info.ObjectId;
        controller.PositionInfo = info.PosInfo;
        controller.SyncPositionInfo();
        
        if (isMyPlayer) {
            MyPlayer = gameObject.GetComponent<MyPlayerController>();
        }
    }

    private void AddProjectile(ObjectInfo info) {
        // TODO: 다양한 투사체에 대응하도록 구분할 것. 일단 전부 화살로 간주함.
        
        var gameObject = Director.Resource.Instantiate("Arrow");
        gameObject.name = "Arrow";
        _objects.Add(info.ObjectId, gameObject);

        var controller = gameObject.GetComponent<BaseController>();
        controller.SetDirection(info.PosInfo.MoveDir);
        controller.CellPos = info.PosInfo.ToVector3Int();
        controller.SyncPositionInfo();
    }

    public void RemoveMyPlayer() {
        if (MyPlayer == null) return;
        
        Remove(MyPlayer.Id);
        MyPlayer = null;
    }

    public void Remove(int id) {
        var gameObject = Find(id);
        if (gameObject) {
            Director.Resource.Destroy(gameObject);
        }
        
        _objects.Remove(id);
    }

    public void Clear(GameObject gameObject) {
        _objects.Values.ForEach(obj => Director.Resource.Destroy(obj));
        _objects.Clear();
    }

    public GameObject Find(int id) {
        return _objects.TryGetValue(id, out var gameObject) ? gameObject : null;
    }
    
    public GameObject Find(Vector3Int cellPos) => Find<BaseController>(cellPos)?.gameObject;
    public T Find<T>(Vector3Int cellPos) where T : BaseController => FindIf<T>(c => c.CellPos == cellPos);
    public T Find<T>(int id) where T : Component => Find(id)?.GetComponent<T>();

    public T FindIf<T>(Func<T, bool> condition) where T : Component {
        foreach (var gameObject in _objects.Values) {
            var component = gameObject.GetComponent<T>();
            if (!component) continue;
            if (condition.Invoke(component)) return component;
        }

        return null;
    }
}
