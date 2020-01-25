using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;

    void Start()
    {
        poolManager = GetComponent<PoolManager>();
    }

    public IEnumerator EnemySpawn()
    {
        while (true)
        {
            float cameraSize = Camera.main.orthographicSize;

            float minX = -cameraSize * 9f / 16f - 1f;
            float maxX = cameraSize * 9f / 16f + 1f;

            float posX = Random.Range(minX, maxX);
            float posY = cameraSize + 1f;

            if (Random.value < 0.5f)
            {
                posY = -cameraSize - 1f;
            }

            Vector3 position = new Vector3(posX, 0f, posY);

            if (Random.value < 0.5f)
            {
                Enemy enemyTP1 = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, position, Quaternion.LookRotation(Vector3.zero - position));
                enemyTP1.EventContactCorrect += OnContactCorrect;
                enemyTP1.EventContactWrong += OnContactWrong;
                enemyTP1.EventOnExplosion += OnExplosion;
            }
            else
            {
                Enemy enemyTS1 = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS1, position, Quaternion.LookRotation(Vector3.zero - position));
                enemyTS1.EventContactCorrect += OnContactCorrect;
                enemyTS1.EventContactWrong += OnContactWrong;
                enemyTS1.EventOnExplosion += OnExplosion;
            }

            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void DespawnEnemy(Enemy targetEnemy)
    {
        targetEnemy.EventContactCorrect -= OnContactCorrect;
        targetEnemy.EventContactWrong -= OnContactWrong;
        targetEnemy.gameObject.SetActive(false);
    }

    public void OnContactCorrect(Enemy owner, int healing)
    {
        if (owner.EnemyType == EnemyType.ToPlanet1 || owner.EnemyType == EnemyType.ToPlanet2) GameManager.Instance.PlayerHPChange(false, healing);
        else if (owner.EnemyType == EnemyType.ToStar1 || owner.EnemyType == EnemyType.ToStar2) GameManager.Instance.PlayerHPChange(true, healing);
        CallParticle(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }

    public void OnContactWrong(Enemy owner, int damage)
    {
        if (owner.EnemyType == EnemyType.ToPlanet1 || owner.EnemyType == EnemyType.ToPlanet2) GameManager.Instance.PlayerHPChange(true, -damage);
        else if (owner.EnemyType == EnemyType.ToStar1 || owner.EnemyType == EnemyType.ToStar2) GameManager.Instance.PlayerHPChange(false, -damage);
        CallParticle(owner.EnemyType, false, owner.transform);
        DespawnEnemy(owner);
    }

    public void OnExplosion(Enemy owner)
    {
        CallParticle(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }
    

    void CallParticle(EnemyType myType, bool isCorrect, Transform _transform)
    {
        SoundManager soundManager = GameManager.Instance.SoundManager;

        switch (myType)
        {
            case EnemyType.ToPlanet1:
                poolManager.Spawn(ObjectPool.ParticleTP1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case EnemyType.ToPlanet2:
                poolManager.Spawn(ObjectPool.ParticleTP1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case EnemyType.ToStar1:
                poolManager.Spawn(ObjectPool.ParticleTS1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case EnemyType.ToStar2:
                poolManager.Spawn(ObjectPool.ParticleTS1, _transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;

        }

        if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
        else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
    }
}
