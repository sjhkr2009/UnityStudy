using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Explosion : MonoBehaviour
{
    [SerializeField] SphereCollider explosionCollider;
    [SerializeField] private float attackDuration;
    [SerializeField] private float particleDuration;
    [SerializeField] bool isHexagon;


    private void OnEnable()
    {
        if(GameManager.Instance.gameState != GameState.Playing)
        {
            gameObject.SetActive(false);
            return;
        }

        if (isHexagon) Invoke(nameof(AttackTimeStart), Time.deltaTime);
        else AttackTimeStart();

        Invoke(nameof(AttackTimeOver), attackDuration);
        Invoke(nameof(DurationOut), particleDuration);
    }

    void AttackTimeStart() { explosionCollider.enabled = true; }
    void AttackTimeOver() { explosionCollider.enabled = false; }
    void DurationOut() { gameObject.SetActive(false); }

    private void Update()
    {
        if (isHexagon) transform.Rotate(Vector3.up, 30f * Time.deltaTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
