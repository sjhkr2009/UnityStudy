using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerWeapon : WeaponBase {
    public override WeaponIndex Index => WeaponIndex.SpinAround;
    public override Transform Transform { get; set; }

    public string spinnerPrefabName = "Bullet00";
    
    private List<Transform> spinners = new List<Transform>();

    public override void Initialize(WeaponController controller) {
        Transform = controller.CreateDummyTransform(this);
        AttackSpeed = 150;
        AttackRange = 1f;
        Damage = 10;
        AttackCount = 3;
        
        CreateSpinners();
    }

    protected virtual void CreateSpinners() {
        if (!Transform) return;
        
        for (int i = 0; i < AttackCount; i++) {
            var spinner = PoolManager.Get(spinnerPrefabName, Transform).transform;
            var param = new ProjectileParam() { damage = Damage };
            spinner.GetComponent<Projectile>().Initialize(param);
            spinners.Add(spinner);
            spinner.localScale = Vector3.one * base.AttackRange;

            var rotation = Vector3.forward * 360f * i / AttackCount;
            spinner.Rotate(rotation);
            spinner.Translate(spinner.up * AttackRange, Space.World); // spinner.up으로 월드 기준 위쪽 방향을 지정했으니 Space.World로 움직여줘야 함
        }
    }

    public override void OnUpdate(float deltaTime) {
        Transform.Rotate(Vector3.back * (AttackSpeed * deltaTime));
    }

    public override void OnUpgrade() {
        Abandon();
        CreateSpinners();
    }

    public override void Abandon() {
        spinners.ForEach(t => PoolManager.Abandon(t.gameObject));
        spinners.Clear();
        
        Object.Destroy(Transform.gameObject);
    }
}