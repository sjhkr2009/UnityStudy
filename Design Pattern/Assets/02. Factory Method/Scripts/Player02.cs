using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryMethod {
    public class Player02 : MonoBehaviour, IUnit {
        public float damagePerSecond = 100f;
        [ReadOnly] public bool isBattle = false;
        [SerializeField, ReadOnly] private float hp = 1000f;

        [Button("플레이어 체력 설정")]
        void SetHp(float value = 1000f) {
            hp = Mathf.Clamp(value, 0, 10000);
        }
        
        [Button("용 사냥 시작")]
        private void SetTarget() {
            if (hp <= 0f) {
                Debug.Log("체력이 낮아 전투를 할 수 없습니다.");
                return;
            }
            Debug.Log("드래곤과 전투를 시작합니다. (사실 그냥 맞기만 함)");
            isBattle = true;
            DragonSpawnManager.Instance.SetTarget(this);
        }
        
        [Button("도망가기")]
        private void ReleaseTarget() {
            isBattle = false;
            DragonSpawnManager.Instance.ReleaseTarget(this);
            Debug.Log("드래곤과 전투를 종료합니다.");
        }
        
        public GameObject GetGameObject() => gameObject;
        public Vector3 GetPosition() => transform.position;
        public float GetHp() => hp;

        public void OnHit(float damage) {
            Debug.Log($"플레이어가 {damage}의 피해를 입었습니다.");
            hp -= damage;

            if (hp < 0f) {
                hp = 0f;
                Debug.Log("<color=red>플레이어가 사망하였습니다.</color>");
                ReleaseTarget();
            }
        }
        
        private void Update() {
            if (!isBattle) return;
            if (DragonSpawnManager.Instance.Dragon == null) return;
            
            DragonSpawnManager.Instance.Dragon.OnHit(damagePerSecond * Time.deltaTime);
        }
    }
}
