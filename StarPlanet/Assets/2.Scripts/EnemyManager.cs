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
                GameObject spawnedObject = poolManager.Spawn(PoolManager.ObjectPool.EnemyTP1, position, Quaternion.LookRotation(Vector3.zero - position));
                Enemy enemyTP1 = spawnedObject.GetComponent<Enemy>();
                enemyTP1.EventContactCorrect += OnContactCorrect;
                enemyTP1.EventContactWrong += OnContactWrong;
            }
            else
            {
                GameObject spawnedObject = poolManager.Spawn(PoolManager.ObjectPool.EnemyTS1, position, Quaternion.LookRotation(Vector3.zero - position));
                Enemy enemyTS1 = spawnedObject.GetComponent<Enemy>();
                enemyTS1.EventContactCorrect += OnContactCorrect;
                enemyTS1.EventContactWrong += OnContactWrong;
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
        GameManager.Instance.Star.Hp += healing;
        CallParticle(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }

    public void OnContactWrong(Enemy owner, int damage)
    {
        GameManager.Instance.Star.Hp -= damage;
        CallParticle(owner.EnemyType, false, owner.transform);
        DespawnEnemy(owner);
    }

    void CallParticle(Type myType, bool isCorrect, Transform transform)
    {
        PoolManager poolManager = GameManager.Instance.poolManager;
        SoundManager soundManager = GameManager.Instance.SoundManager;

        switch (myType)
        {
            case Type.ToPlanet1:
                poolManager.Spawn(PoolManager.ObjectPool.ParticleTP1, transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;
            case Type.ToStar1:
                poolManager.Spawn(PoolManager.ObjectPool.ParticleTS1, transform.position, Quaternion.Euler(0f, 90f, 0f));
                break;

        }

        if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
        else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
    }
}
