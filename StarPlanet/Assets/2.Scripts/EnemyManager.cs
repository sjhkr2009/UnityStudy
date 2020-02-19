using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] PoolManager poolManager;
    [BoxGroup("Components"), SerializeField] ScoreManager scoreManager;
    [BoxGroup("Components"), SerializeField] SoundManager soundManager;
    [BoxGroup("Components"), SerializeField] ParticleManager particleManager;

    [BoxGroup("Spawn Control"), SerializeField] float spawnDelay;
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

    /// <summary>
    /// 일정 주기로 적을 생성합니다.
    /// [생성 주기: spawnDelay]
    /// 설정된 딜레이의 80%~120% 범위에서 임의로 결정됩니다.
    /// 여러 개체를 일정 간격으로 생성하는 Enemy To Star3 타입에도 딜레이는 변화하지 않으나, 해당 개체의 소환 동작이 끝나기 전까지는 새로운 적이 생성되지 않습니다.
    /// 
    /// [생성 지점: position]
    /// 카메라의 범위를 기준으로 화면 밖 상/하단 중 하나, 화면의 폭보다 약간 넓은 범위 중 임의의 지점.
    /// 카메라 범위에 대응하며 변경되지만, 종횡비가 변할 경우 GameManager의 screenHorizontal, screenVertical을 조정해주세요. 단, 화면의 폭이 높이보다 좁아야 합니다.
    /// 
    /// [생성자 타입: Probability]
    /// 높은 티어의 적이 스폰될 확률을 0과 1 사이의 숫자로 입력합니다. 높은 티어부터 적용되어야 하며, 하위 티어 유닛의 소환 확률은 상위 티어 유닛의 확률을 뺀 부분이 됩니다.
    /// 예) tier4Probability = 0f, tier3Probability = 0.05f, tier2Probability = 0.2f 인 경우, 티어3 유닛이 5% 확률, 티어2 유닛이 15% 확률, 나머지 80% 확률로 티어1 유닛이 소환됩니다.
    /// 단, 높은 티어의 유닛이 소환되면 쿨타임이 적용되어, 쿨타임 동안 해당 티어의 적은 생성되지 않으며 하위 티어의 적이 대신 생성됩니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator EnemySpawn()
    {
        while (true)
        {
            yield return null;
            if (GameManager.Instance.gameState != GameState.Playing) continue;

            float spawnDelay = Random.Range(this.spawnDelay * 0.8f, this.spawnDelay * 1.2f);
            float cameraSize = Camera.main.orthographicSize;

            float minX = -cameraSize * screenView - 2f;
            float maxX = cameraSize * screenView + 2f;

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

    /// <summary>
    /// 높은 티어의 유닛이 생성될 때 발동되어 쿨타임을 적용시킵니다.
    /// </summary>
    /// <param name="tier">해당 적의 티어</param>
    /// <returns></returns>
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

    /// <summary>
    /// 적 개체 중 일정 거리만큼 접근한 후 분열하는 적에 적용됩니다. 티어1의 To Planet형 적을 8방향으로 소환합니다.
    /// </summary>
    /// <param name="spawnPos"></param>
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
        AddFeverGauge(owner);
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
        AddFeverGauge(owner);
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

    /// <summary>
    /// 적이 파괴된 위치에 피버 파티클을 생성합니다. 생성된 파티클은 자동으로 게이지 바로 날아가며, 피버 게이지를 1 채웁니다.
    /// </summary>
    /// <param name="owner"></param>
    void AddFeverGauge(Enemy owner)
    {
        if(owner.EnemyType == EnemyType.ToPlanet1 || owner.EnemyType == EnemyType.ToStar1)
        {
            feverManager.CallParticle(owner.transform, 3);
        }
        else if (owner.EnemyType == EnemyType.ToPlanet2 || owner.EnemyType == EnemyType.ToStar2)
        {
            feverManager.CallParticle(owner.transform, 5);
        }
        else if (owner.EnemyType == EnemyType.ToPlanet3)
        {
            feverManager.CallParticle(owner.transform, 10);
        }
        else if (owner.EnemyType == EnemyType.ToStar3)
        {
            feverManager.CallParticle(owner.transform, 2);
        }
        else if (owner.EnemyType == EnemyType.ToPlanet4)
        {
            feverManager.CallParticle(owner.transform, 24);
        }
        else if (owner.EnemyType == EnemyType.ToStar4)
        {
            feverManager.CallParticle(owner.transform, 10);
        }
    }

    public void AllEnemyEventReset()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies) if (enemy.gameObject.activeSelf) DeleteEventToEnemy(enemy);
        StopAllCoroutines();
    }
}
