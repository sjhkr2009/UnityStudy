using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] PoolManager poolManager;
    [BoxGroup("Components"), SerializeField] ScoreManager scoreManager;
    [BoxGroup("Components"), SerializeField] SoundManager soundManager;
    [BoxGroup("Components"), SerializeField] ParticleManager particleManager;

    [BoxGroup("Spawn Control"), SerializeField] float minSpawnDelay;
    [BoxGroup("Spawn Control"), SerializeField] float maxSpawnDelay;

    //높은 티어의 적이 스폰될 확률을 0과 1 사이의 숫자로 입력합니다. 높은 티어부터 적용되어야 하며, 하위 티어 유닛의 소환 확률은 상위 티어 유닛의 확률을 뺀 부분이 됩니다.
    //예) tier4Probability = 0f, tier3Probability = 0.05f, tier2Probability = 0.2f 인 경우, 티어3 유닛이 5% 확률, 티어2 유닛이 15% 확률, 나머지 80% 확률로 티어1 유닛이 소환됩니다.
    [BoxGroup("Spawn Control"), SerializeField] float tier2Probability;
    [BoxGroup("Spawn Control"), SerializeField] float tier3Probability;
    [BoxGroup("Spawn Control"), SerializeField] float tier4Probability;

    float screenView;

    void Start()
    {
        if (poolManager == null) poolManager = GameManager.Instance.PoolManager;
        if (scoreManager == null) scoreManager = GameManager.Instance.ScoreManager;
        if (soundManager == null) soundManager = GameManager.Instance.SoundManager;
        if (particleManager == null) particleManager = GameManager.Instance.ParticleManager;

        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;
        StartCoroutine(EnemySpawn());
    }

    public IEnumerator EnemySpawn()
    {
        while (true)
        {
            yield return null;
            if (GameManager.Instance.gameState != GameState.Playing) continue;

            float cameraSize = Camera.main.orthographicSize;

            float minX = -cameraSize * screenView - 1f;
            float maxX = cameraSize * screenView + 1f;

            float posX = Random.Range(minX, maxX);
            float posY = cameraSize + 1f;

            if (Random.value < 0.5f)
            {
                posY = -cameraSize - 1f;
            }

            Vector3 position = new Vector3(posX, 0f, posY);
            Enemy enemy = null;

            if (Random.value < 0.5f) //To Planet형 유닛 생성
            {
                if(Random.value < tier4Probability)
                {

                }
                else if(Random.value < tier3Probability)
                {

                }
                else if(Random.value < tier2Probability)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP2, position, Quaternion.LookRotation(Vector3.zero - position));
                }
                else
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, position, Quaternion.LookRotation(Vector3.zero - position));
                }
            }
           else //To Star형 유닛 생성
            {
                if (Random.value < tier4Probability)
                {

                }
                else if (Random.value < tier3Probability)
                {

                }
                else if (Random.value < tier2Probability)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS2, position, Quaternion.LookRotation(Vector3.zero - position));
                }
                else
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS1, position, Quaternion.LookRotation(Vector3.zero - position));
                }
            }

            AddEventToEnemy(enemy);

            float spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void AddEventToEnemy(Enemy enemy)
    {
        enemy.EventContactCorrect += OnContactCorrect;
        enemy.EventContactCorrect += scoreManager.GetScore;

        enemy.EventContactWrong += OnContactWrong;

        enemy.EventOnExplosion += OnExplosion;
        enemy.EventOnExplosion += scoreManager.GetScore;
    }

    void DeleteEventToEnemy(Enemy enemy)
    {
        enemy.EventContactCorrect -= OnContactCorrect;
        enemy.EventContactCorrect -= scoreManager.GetScore;

        enemy.EventContactWrong -= OnContactWrong;

        enemy.EventOnExplosion -= OnExplosion;
        enemy.EventOnExplosion -= scoreManager.GetScore;
    }

    public void DespawnEnemy(Enemy targetEnemy)
    {
        DeleteEventToEnemy(targetEnemy);
        targetEnemy.gameObject.SetActive(false);
    }

    public void OnContactCorrect(Enemy owner, int healing)
    {
        if (owner.EnemyType == EnemyType.ToPlanet1 || owner.EnemyType == EnemyType.ToPlanet2) GameManager.Instance.PlayerHPChange(false, healing);
        else if (owner.EnemyType == EnemyType.ToStar1 || owner.EnemyType == EnemyType.ToStar2) GameManager.Instance.PlayerHPChange(true, healing);
        CallFX(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }

    public void OnContactWrong(Enemy owner, int damage)
    {
        if (owner.EnemyType == EnemyType.ToPlanet1 || owner.EnemyType == EnemyType.ToPlanet2) GameManager.Instance.PlayerHPChange(true, -damage);
        else if (owner.EnemyType == EnemyType.ToStar1 || owner.EnemyType == EnemyType.ToStar2) GameManager.Instance.PlayerHPChange(false, -damage);
        CallFX(owner.EnemyType, false, owner.transform);
        DespawnEnemy(owner);
    }

    public void OnExplosion(Enemy owner)
    {
        CallFX(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }
    

    void CallFX(EnemyType myType, bool isCorrect, Transform _transform)
    {
        switch (myType)
        {
            case EnemyType.ToPlanet1:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
            case EnemyType.ToPlanet2:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
            case EnemyType.ToStar1:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
            case EnemyType.ToStar2:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

        }
    }

    public void AllEnemyEventReset()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies) if (enemy.gameObject.activeSelf) DeleteEventToEnemy(enemy);
        StopAllCoroutines();
    }
}
