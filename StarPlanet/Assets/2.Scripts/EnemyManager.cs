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

    [BoxGroup("Spawn Control"), SerializeField] float spawnDelay;

    //높은 티어의 적이 스폰될 확률을 0과 1 사이의 숫자로 입력합니다. 높은 티어부터 적용되어야 하며, 하위 티어 유닛의 소환 확률은 상위 티어 유닛의 확률을 뺀 부분이 됩니다.
    //예) tier4Probability = 0f, tier3Probability = 0.05f, tier2Probability = 0.2f 인 경우, 티어3 유닛이 5% 확률, 티어2 유닛이 15% 확률, 나머지 80% 확률로 티어1 유닛이 소환됩니다.
    //단, 높은 티어의 유닛이 소환되면 쿨타임이 적용되어, 쿨타임 동안 해당 티어의 적은 생성되지 않습니다.
    [BoxGroup("Spawn Control"), SerializeField] float tier2Probability;
    private bool isTier2cooltime = false;
    [BoxGroup("Spawn Control"), SerializeField] float tier3Probability;
    private bool isTier3cooltime = false;
    [BoxGroup("Spawn Control"), SerializeField] float tier4Probability;
    private bool isTier4cooltime = false;
    [BoxGroup("Spawn Control"), SerializeField] float tier2cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier3cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier4cooltime;
    [BoxGroup("Spawn Control"), SerializeField] int tier3TSSpawnCount = 5;

    float screenView;
    FeverManager feverManager;

    void Start()
    {
        if (poolManager == null) poolManager = GameManager.Instance.PoolManager;
        if (scoreManager == null) scoreManager = GameManager.Instance.ScoreManager;
        if (soundManager == null) soundManager = GameManager.Instance.SoundManager;
        if (particleManager == null) particleManager = GameManager.Instance.ParticleManager;

        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;
        feverManager = GameManager.Instance.FeverManager;
        StartCoroutine(EnemySpawn());
    }

    public IEnumerator EnemySpawn()
    {
        while (true)
        {
            yield return null;
            if (GameManager.Instance.gameState != GameState.Playing) continue;

            float spawnDelay = Random.Range(this.spawnDelay * 0.8f, this.spawnDelay * 1.2f);
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

            if (Random.value < 0.4f) //To Planet형 유닛 생성
            {
                if(Random.value < tier4Probability && !isTier4cooltime)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP4, position, Quaternion.LookRotation(Vector3.zero - position));
                    StartCoroutine(SpawnCooldown(4));
                }
                else if(Random.value < tier3Probability && !isTier3cooltime)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP3, position, Quaternion.LookRotation(Vector3.zero - position));
                    StartCoroutine(SpawnCooldown(3));
                }
                else if(Random.value < tier2Probability && !isTier2cooltime)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP2, position, Quaternion.LookRotation(Vector3.zero - position));
                    StartCoroutine(SpawnCooldown(2));
                }
                else
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, position, Quaternion.LookRotation(Vector3.zero - position));
                }
            }
           else //To Star형 유닛 생성
            {
                if (Random.value < tier4Probability && !isTier4cooltime)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS4, position, Quaternion.LookRotation(Vector3.zero - position));
                    StartCoroutine(SpawnCooldown(4));
                }
                else if (Random.value < tier3Probability && !isTier3cooltime)
                {
                    for (int i = 0; i < tier3TSSpawnCount - 1; i++)
                    {
                        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS3, position, Quaternion.LookRotation(Vector3.zero - position));
                        AddEventToEnemy(enemy);
                        yield return new WaitForSeconds(0.5f);
                    }

                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS3, position, Quaternion.LookRotation(Vector3.zero - position));
                    spawnDelay = Mathf.Max(spawnDelay - (0.2f * tier3TSSpawnCount), 0.1f);
                    StartCoroutine(SpawnCooldown(3));
                }
                else if (Random.value < tier2Probability && !isTier2cooltime)
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS2, position, Quaternion.LookRotation(Vector3.zero - position));
                    StartCoroutine(SpawnCooldown(2));
                }
                else
                {
                    enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTS1, position, Quaternion.LookRotation(Vector3.zero - position));
                }
            }

            AddEventToEnemy(enemy);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnCooldown(int tier)
    {
        switch (tier)
        {
            case 2:
                isTier2cooltime = true;
                yield return new WaitForSeconds(tier2cooltime);
                isTier2cooltime = false;
                break;
            case 3:
                isTier3cooltime = true;
                yield return new WaitForSeconds(tier3cooltime);
                isTier3cooltime = false;
                break;
            case 4:
                isTier4cooltime = true;
                yield return new WaitForSeconds(tier4cooltime);
                isTier4cooltime = false;
                break;
            default:
                yield break;
        }
    }

    void OnDivideSpawn(Vector3 spawnPos)
    {
        particleManager.SpawnParticle(ParticleType.DestroyTPsmall, spawnPos);

        Enemy enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + Vector3.forward));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + Vector3.back));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + Vector3.right));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + Vector3.left));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + new Vector3(1, 0, 1)));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + new Vector3(-1, 0, 1)));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + new Vector3(1, 0, -1)));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(ObjectPool.EnemyTP1, spawnPos, Quaternion.LookRotation(spawnPos + new Vector3(-1, 0, -1)));
        AddEventToEnemy(enemy);
    }

    void AddEventToEnemy(Enemy enemy)
    {
        enemy.EventContactCorrect += OnContactCorrect;
        enemy.EventContactCorrect += scoreManager.GetScore;

        enemy.EventContactWrong += OnContactWrong;

        enemy.EventOnExplosion += OnExplosion;
        enemy.EventOnExplosion += scoreManager.GetScore;

        if (enemy.EnemyType == EnemyType.ToPlanet4) enemy.EventOnDivide += OnDivideSpawn;
    }

    void DeleteEventToEnemy(Enemy enemy)
    {
        enemy.EventContactCorrect -= OnContactCorrect;
        enemy.EventContactCorrect -= scoreManager.GetScore;

        enemy.EventContactWrong -= OnContactWrong;

        enemy.EventOnExplosion -= OnExplosion;
        enemy.EventOnExplosion -= scoreManager.GetScore;

        if (enemy.EnemyType == EnemyType.ToPlanet4) enemy.EventOnDivide -= OnDivideSpawn;
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
        CallFXOnDestory(owner.EnemyType, true, owner.transform);
        AddFeverGauge(owner.EnemyType);
        DespawnEnemy(owner);
    }

    public void OnContactWrong(Enemy owner, int damage)
    {
        if (owner.EnemyType == EnemyType.ToPlanet1 || owner.EnemyType == EnemyType.ToPlanet2) GameManager.Instance.PlayerHPChange(true, -damage);
        else if (owner.EnemyType == EnemyType.ToStar1 || owner.EnemyType == EnemyType.ToStar2) GameManager.Instance.PlayerHPChange(false, -damage);
        CallFXOnDestory(owner.EnemyType, false, owner.transform);
        DespawnEnemy(owner);
    }

    public void OnExplosion(Enemy owner)
    {
        CallFXOnDestory(owner.EnemyType, true, owner.transform);
        AddFeverGauge(owner.EnemyType);
        DespawnEnemy(owner);
    }
    

    void CallFXOnDestory(EnemyType myType, bool isCorrect, Transform _transform)
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
            case EnemyType.ToPlanet3:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
            case EnemyType.ToPlanet4:
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
            case EnemyType.ToStar3:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
            case EnemyType.ToStar4:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect) soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
        }
    }

    void AddFeverGauge(EnemyType type)
    {
        if(type == EnemyType.ToPlanet1 || type == EnemyType.ToStar1)
        {
            feverManager.GetFeverCount(2);
        }
        else if (type == EnemyType.ToPlanet2 || type == EnemyType.ToStar2)
        {
            feverManager.GetFeverCount(5);
        }
        else if (type == EnemyType.ToPlanet3)
        {
            feverManager.GetFeverCount(10);
        }
        else if (type == EnemyType.ToStar3)
        {
            feverManager.GetFeverCount(2);
        }
        else if (type == EnemyType.ToPlanet4 || type == EnemyType.ToStar4)
        {
            feverManager.GetFeverCount(10);
        }
    }

    public void AllEnemyEventReset()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies) if (enemy.gameObject.activeSelf) DeleteEventToEnemy(enemy);
        StopAllCoroutines();
    }
}
