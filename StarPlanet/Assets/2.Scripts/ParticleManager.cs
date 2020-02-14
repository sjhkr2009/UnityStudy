using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ParticleType { DestroyTPsmall, DestroyTSsmall, NormalExplosion, HexagonExplosion, Healing }

public class ParticleManager : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;

    private void Start()
    {
        poolManager = GameManager.Instance.PoolManager;
    }

    public Component SpawnAndGetParticle(ParticleType particleType, Transform _transform)
    {
        Component returnObject = null;

        switch (particleType)
        {
            case ParticleType.DestroyTPsmall:
                returnObject = poolManager.Spawn(ObjectPool.ParticleTP1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case ParticleType.DestroyTSsmall:
                returnObject = poolManager.Spawn(ObjectPool.ParticleTS1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case ParticleType.NormalExplosion:
                returnObject = poolManager.Spawn(ObjectPool.ParticleExplosion, _transform.position, Quaternion.Euler(90f, 0f, 0f));
                break;
            case ParticleType.HexagonExplosion:
                returnObject = poolManager.Spawn(ObjectPool.ParticleHexagonExp, _transform.position, Quaternion.identity);
                break;
            case ParticleType.Healing:
                returnObject = poolManager.Spawn(ObjectPool.ParticleHealing, _transform.position, Quaternion.Euler(-90f, 0f, 0f));
                break;
        }

        return returnObject;
    }
    public void SpawnParticle(ParticleType particleType, Transform _transform)
    {
        switch (particleType)
        {
            case ParticleType.DestroyTPsmall:
                poolManager.Spawn(ObjectPool.ParticleTP1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case ParticleType.DestroyTSsmall:
                poolManager.Spawn(ObjectPool.ParticleTS1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case ParticleType.NormalExplosion:
                poolManager.Spawn(ObjectPool.ParticleExplosion, _transform.position, Quaternion.Euler(90f, 0f, 0f));
                break;
            case ParticleType.HexagonExplosion:
                poolManager.Spawn(ObjectPool.ParticleHexagonExp, _transform.position, Quaternion.identity);
                break;
            case ParticleType.Healing:
                poolManager.Spawn(ObjectPool.ParticleHealing, _transform.position, Quaternion.Euler(-90f, 0f, _transform.rotation.eulerAngles.y + 180f));
                break;
        }
    }
    public void SpawnParticle(ParticleType particleType, Vector3 _transform)
    {
        switch (particleType)
        {
            case ParticleType.DestroyTPsmall:
                poolManager.Spawn(ObjectPool.ParticleTP1, _transform, Quaternion.Euler(0f, 90f, 0f));
                break;
            case ParticleType.DestroyTSsmall:
                poolManager.Spawn(ObjectPool.ParticleTS1, _transform, Quaternion.Euler(0f, 90f, 0f));
                break;
            case ParticleType.NormalExplosion:
                poolManager.Spawn(ObjectPool.ParticleExplosion, _transform, Quaternion.Euler(90f, 0f, 0f));
                break;
            case ParticleType.HexagonExplosion:
                poolManager.Spawn(ObjectPool.ParticleHexagonExp, _transform, Quaternion.identity);
                break;
            case ParticleType.Healing:
                poolManager.Spawn(ObjectPool.ParticleHealing, _transform, Quaternion.Euler(-90f, 0f, 0f));
                break;
        }
    }
}
