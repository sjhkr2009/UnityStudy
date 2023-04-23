using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerWeapon : WeaponBase {
    public string spinnerPrefabName = "Bullet00";
    public float rotateSpeed = 150;
    public float radius = 1.5f;
    public int spinnerCount = 3;
    
    private List<Transform> spinners = new List<Transform>();
    
    public override void Initialize(WeaponController controller) {
        damage = 10;
        CreateSpinners();
    }

    protected virtual void CreateSpinners() {
        for (int i = 0; i < spinnerCount; i++) {
            var spinner = PoolManager.Get(spinnerPrefabName, transform).transform;
            spinner.GetComponent<Projectile>().Initialize(damage, -1);
            spinners.Add(spinner);

            var rotation = Vector3.forward * 360f * i / spinnerCount;
            spinner.Rotate(rotation);
            spinner.Translate(spinner.up * radius, Space.World); // spinner.up으로 월드 기준 위쪽 방향을 지정했으니 Space.World로 움직여줘야 함
        }
    }

    public override void OnUpdate(float deltaTime) {
        transform.Rotate(Vector3.back * (rotateSpeed * deltaTime));
    }

    public override void OnUpgrade() {
        Abandon();
        CreateSpinners();
    }

    public override void Abandon() {
        spinners.ForEach(t => PoolManager.Abandon(t.gameObject));
        spinners.Clear();
        
        PoolManager.Abandon(gameObject);
    }
}
