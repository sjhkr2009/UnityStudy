using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Explosion : MonoBehaviour
{
    [SerializeField] SphereCollider explosionCollider;
    [SerializeField] private float attackDuration;
    [SerializeField] private float particleDuration;


    private void OnEnable()
    {
        explosionCollider.enabled = true;
        Invoke(nameof(AttackTimeOver), attackDuration);
        Invoke(nameof(DurationOut), particleDuration);
    }

    void AttackTimeOver()
    {
        explosionCollider.enabled = false;
    }

    void DurationOut()
    {
        gameObject.SetActive(false);
    }
}
