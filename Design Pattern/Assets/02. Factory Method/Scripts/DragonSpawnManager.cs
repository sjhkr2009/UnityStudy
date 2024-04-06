using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryMethod {
    public class DragonSpawnManager : MonoBehaviour {
        private static DragonSpawnManager _instance;
        public static DragonSpawnManager Instance => _instance;
        
        [SerializeField] private DragonType dragonType = DragonType.Infernal;
        [SerializeField] private DragonHandler dragonHandler;

        public DragonImplBase Dragon {
            get => dragonHandler.dragon;
            private set => dragonHandler.dragon = value;
        }

        private void Awake() {
            _instance = this;
        }

        private void Start() {
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = gameObject;
#endif
        }

        [Button("드래곤 소환")]
        void SpawnDragon() {
            if (!Application.isPlaying || !_instance) {
                Debug.LogError("플레이 중에만 사용할 수 있습니다.");
                return;
            }

            if (!dragonHandler) {
                dragonHandler = GetComponent<DragonHandler>();
            }
            
            Dragon = DragonFactory.Create<DragonImplBase>((int)dragonType);
            
            Debug.Log($"{dragonType} 드래곤이 소환되었습니다! (공격력: {Dragon.AttackDamage} / 공격속도: {Dragon.AttackSpeed})");
            
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = GameObject.Find("Player") ?? gameObject;
#endif
        }

        public void SetTarget(IUnit target) {
            if (Dragon == null) {
                Debug.Log("<color=yellow>먼저 드래곤을 소환해주세요.</color>");
                return;
            }
            dragonHandler.SetTarget(target);
        }

        public void ReleaseTarget(IUnit target) {
            if (dragonHandler.CurrentTarget == target) dragonHandler.ReleaseTarget();
        }
    }
}
