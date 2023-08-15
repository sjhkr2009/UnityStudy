using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HomingProjectile : Projectile {
    [TabGroup("Homing")] [SerializeField] protected float homingRotation = 3f;
    [TabGroup("Homing")] [SerializeField] protected float homingRotationAdd = 0.5f;
    [TabGroup("Homing")] [SerializeField] protected float homingStartDelay = 0.2f;

    protected float originSpeed;
    protected float originHomingRotation;

    protected float speed;
    GameObject target;

    protected void MoveToward() {
        transform.Translate(speed * Time.deltaTime * Vector3.forward);
    }

    public override void Initialize(ProjectileParam param) {
        base.Initialize(param);

        transform.position = param.startPoint;
        target = GetNearTarget();
        originSpeed = param.speed;
        hp = 3;
        
        if (target != null) {
            StartCoroutine(Homing(target));
        }
    }

    void Update() {
        if (GameManager.IsPause) return;
        
        if (target == null || target.activeSelf == false) {
            speed = originSpeed;
            target = GetNearTarget();
            if (target != null) {
                StartCoroutine(Homing(target));
            }
        }

        MoveToward();
    }

    private int hp = 3;
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (!IsValidTarget(other.gameObject)) return;

        var damageHandler = other.GetComponent<IDamagableEntity>();
        var attackPos = other.bounds.ClosestPoint(transform.position);
        damageHandler?.OnAttacked(this, attackPos);
        OnAttack?.Invoke(attackPos);
        ShowHitEffect();

        hp--;
        if (hp <= 0) PoolManager.Abandon(gameObject);
    }

    GameObject GetNearTarget() {
        var nearest = GameManager.Ability.Scanner.GetNearestTargetFrom(transform.position);
        
        return nearest ? nearest.gameObject : null;
    }

    protected IEnumerator Homing(GameObject target) {
        yield return new WaitForSeconds(homingStartDelay);
        homingRotation = originHomingRotation;

        while (this.gameObject.activeSelf) {
            if (!target.activeSelf) {
                break;
            }

            if (GameManager.IsPause) {
                yield return null;
            }

            Vector3 targetDir = Quaternion.LookRotation(target.transform.position - transform.position).eulerAngles;
            float targetRotation = targetDir.y;
            Vector3 currentDir = transform.rotation.eulerAngles;
            float currentRotation = currentDir.y;

            //Debug.Log($"타겟: {targetRotation} / 총알: {currentRotation}");

            float difference = targetRotation - currentRotation;
            if (difference > 180f) {
                difference -= 360f;
            }
            else if (difference < -180f) {
                difference += 360f;
            }
            //Debug.Log($"회전해야 하는 각도: {difference}");

            if (Mathf.Abs(difference) < homingRotation) {
                transform.rotation = Quaternion.Euler(targetDir);
            }
            else if (difference >= 0) {
                Quaternion getRotation =
                    Quaternion.Euler(new Vector3(currentDir.x, currentRotation + homingRotation, currentDir.z));
                transform.rotation = getRotation;
            }
            else if (difference < 0) {
                Quaternion getRotation =  Quaternion.Euler(new Vector3(currentDir.x, currentRotation - homingRotation, currentDir.z));
                transform.rotation = getRotation;
            } else {
                Debugger.Error("Error!");
            }

            homingRotation += homingRotationAdd;

            speed = (originSpeed + homingRotation / 3f);

            yield return new WaitForSeconds(0.04f);
        }
    }
}
