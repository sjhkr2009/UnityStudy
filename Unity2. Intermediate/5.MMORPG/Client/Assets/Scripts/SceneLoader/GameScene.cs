using System.Collections;
using System.Collections.Generic;
using Define;
using UnityEngine;

public class GameScene : BaseScene {
    protected override void Init() {
        base.Init();
        SceneType = Scene.Game;
        
        Director.Map.LoadMap(1);

        GameObject player = Director.Resource.Instantiate("Player");
        Director.Object.Add(player);
        
        // temp: 랜덤 위치에 몬스터 생성
        for (int i = 0; i < 5; i++) {
            GameObject monster = Director.Resource.Instantiate("Monster");
            monster.name = $"Monster {i}";
            
            Vector3Int pos = new Vector3Int() {
                x = Random.Range(-20, 20),
                y = Random.Range(-10, 10)
            };

            var controller = monster.GetComponent<MonsterController>();
            controller.SetPositionInstant(pos);
            
            Director.Object.Add(monster);
        }
    }

    public override void Clear() {
        
    }
}
