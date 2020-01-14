using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Enemy : MonoBehaviour
{
    public enum Type { ToPlanet1, ToStar1 }
    [SerializeField] Type enemyType;

    [SerializeField] private string targetType;
    [SerializeField] private string avoidType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private int healing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetType))
        {
            Debug.Log("Success");
            GameManager.Instance.EnemyOnCollision(targetType, healing, false);
            gameObject.SetActive(false);
        }
        else if (other.CompareTag(avoidType))
        {
            Debug.Log("Fail");
            GameManager.Instance.EnemyOnCollision(avoidType, damage, true);
            gameObject.SetActive(false);
        }
    }

    void CallParticle(Type myType)
    {
        PoolManager poolManager = GameManager.Instance.poolManager;
        
        switch (myType)
        {
            case Type.ToPlanet1:
                poolManager.Spawn(PoolManager.ObjectPool.ParticleTP1, transform.position);
                break;
            case Type.ToStar1:
                poolManager.Spawn(PoolManager.ObjectPool.ParticleTS1, transform.position);
                break;

        }
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        Move();
    }
}
