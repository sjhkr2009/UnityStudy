using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerWeapon : AbilityBase, IWeaponAbility {
    public override AbilityIndex Index => AbilityIndex.WeaponSpinAround;
    public Transform Transform { get; set; }

    public string spinnerPrefabName = "Bullet00";
    
    private List<Transform> spinners = new List<Transform>();
    
    public float Damage { get; set; }
    public float ObjectSpeed { get; set; }
    public float AttackRange { get; set; }
    public int AttackCount { get; set; }

    public override void Initialize(AbilityController controller) {
        base.Initialize(controller);
        
        Transform = controller.CreateDummyTransform(this);
        SetDataByLevel();
    }

    protected virtual void CreateSpinners() {
        if (!Transform) return;

        ClearAllSpinners();
        
        for (int i = 0; i < AttackCount; i++) {
            var spinner = PoolManager.Get(spinnerPrefabName, Transform).transform;
            var param = new ProjectileParam() { damage = Damage  * GameManager.Player.Status.AttackPower };
            spinner.GetComponent<Projectile>().Initialize(param);
            spinners.Add(spinner);
            spinner.localScale = Vector3.one * AttackRange;

            var rotation = Vector3.forward * 360f * i / AttackCount;
            spinner.Rotate(rotation);
            spinner.Translate(spinner.up * AttackRange, Space.World); // spinner.up으로 월드 기준 위쪽 방향을 지정했으니 Space.World로 움직여줘야 함
        }
    }

    public void OnEveryFrame(float deltaTime) {
        Transform.Rotate(Vector3.back * (ObjectSpeed * deltaTime));
    }

    public override void Upgrade() {
        base.Upgrade();

        SetDataByLevel();
    }

    private void SetDataByLevel() {
        ObjectSpeed = Data.GetValue(AbilityValueType.ObjectSpeed, Level);
        AttackRange = Data.GetValue(AbilityValueType.AttackRange, Level);
        Damage = Data.GetValue(AbilityValueType.Damage, Level);
        AttackCount = Data.GetIntValue(AbilityValueType.AttackCount, Level);
        
        CreateSpinners();
    }

    void ClearAllSpinners() {
        spinners.ForEach(t => PoolManager.Abandon(t.gameObject));
        spinners.Clear();
    }

    public override void Abandon() {
        ClearAllSpinners();
        Object.Destroy(Transform.gameObject);
    }
}
