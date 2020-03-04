using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    PoolManager poolManager;
    SpawnManager spawnManager;
    ScoreManager scoreManager;
    SoundManager soundManager;
    ParticleManager particleManager;
    FeverManager feverManager;

    [BoxGroup("Spawn Control"), SerializeField] float spawnDelay;
    [BoxGroup("Spawn Control"), SerializeField] float tier1Speed;
    [BoxGroup("Spawn Control"), SerializeField] float tier2Probability;
    [SerializeField, ReadOnly] private bool isTier2cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier3Probability;
    [SerializeField, ReadOnly] private bool isTier3cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier4Probability;
    [SerializeField, ReadOnly] private bool isTier4cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier2cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier3cooltime;
    [BoxGroup("Spawn Control"), SerializeField] float tier4cooltime;
    [BoxGroup("Spawn Control"), SerializeField] int tier3TSSpawnCount;
    [BoxGroup("Spawn Control"), SerializeField] int tier3TPSpawnCount;

    List<Enemy> spawnedEnemies = new List<Enemy>();
    
    int feverTimeCount;

    void Start()
    {
        poolManager = GameManager.Instance.PoolManager;
        spawnManager = GameManager.Instance.SpawnManager;
        scoreManager = GameManager.Instance.ScoreManager;
        soundManager = GameManager.Instance.SoundManager;
        particleManager = GameManager.Instance.ParticleManager;
        feverManager = GameManager.Instance.FeverManager;

        isTier2cooltime = false;
        isTier3cooltime = false;
        isTier4cooltime = false;

        feverTimeCount = 0;
        tier1Speed = 1f;
        tier3TSSpawnCount = 4;

        StartCoroutine(nameof(EnemySpawn));
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

            Enemy enemy = null;

            if (Random.value < 0.4f) //To Planet형 유닛 생성
            {
                if(Random.value < tier4Probability && !isTier4cooltime)
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTP4);
                    StartCoroutine(SpawnCooldown(4));
                }
                else if(Random.value < tier3Probability && !isTier3cooltime)
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTP3);
                    StartCoroutine(SpawnCooldown(3));
                }
                else if(Random.value < tier2Probability && !isTier2cooltime)
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTP3);
                    StartCoroutine(SpawnCooldown(2));
                }
                else
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTP1);
                    enemy.moveSpeed = tier1Speed;
                }
            }
           else //To Star형 유닛 생성
            {
                if (Random.value < tier4Probability && !isTier4cooltime)
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTS4);
                    StartCoroutine(SpawnCooldown(4));
                }
                else if (Random.value < tier3Probability && !isTier3cooltime)
                {
                    StartCoroutine(SpawnEnemiesDirectly(ObjectPool.EnemyTS3, tier3TSSpawnCount));
                    StartCoroutine(SpawnCooldown(3));
                    yield return new WaitForSeconds(spawnDelay);
                    continue;
                }
                else if (Random.value < tier2Probability && !isTier2cooltime)
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTS2);
                    StartCoroutine(SpawnCooldown(2));
                }
                else
                {
                    enemy = (Enemy)spawnManager.SpawnOverMapToCenter(ObjectPool.EnemyTS1);
                    enemy.moveSpeed = tier1Speed;
                }
            }
            AddEventToEnemy(enemy);
            if (!spawnedEnemies.Contains(enemy)) spawnedEnemies.Add(enemy);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator SpawnEnemiesDirectly(ObjectPool type, int count)
    {
        Vector3 spawnPos = spawnManager.RandomSpawnPositionOverMap();

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = (Enemy)poolManager.Spawn(type, spawnPos, Quaternion.identity);
            AddEventToEnemy(enemy);
            if (!spawnedEnemies.Contains(enemy)) spawnedEnemies.Add(enemy);
            yield return new WaitForSeconds(0.4f);
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
    /// 적 개체 중 분열하는 형태의 적에게 적용됩니다. 입력한 타입의 적을 8방향으로 소환합니다.
    /// </summary>
    /// <param name="spawnTransform"></param>
    void OnDivideSpawn(Transform spawnTransform, ObjectPool type)
    {
        particleManager.SpawnParticle(ParticleType.DestroyTPsmall, spawnTransform); //분열 이펙트로 변경할 것

        for (int i = 0; i < tier3TPSpawnCount; i++)
        {
            Enemy enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.Euler(new Vector3(0f, (360f/(float)tier3TPSpawnCount) * (float)i, 0f)));
            AddEventToEnemy(enemy);
        }
    }
    /// <summary>
    /// 적에게 이벤트를 추가합니다. 추가된 이벤트는 DeleteEventToEnemy 함수에서 제거해야 합니다.
    /// </summary>
    /// <param name="enemy"></param>
    void AddEventToEnemy(Enemy enemy)
    {
        enemy.EventContactCorrect += OnContactCorrect;
        enemy.EventContactCorrect += scoreManager.GetScore;

        enemy.EventContactWrong += OnContactWrong;

        enemy.EventOnExplosion += OnExplosion;
        enemy.EventOnExplosion += scoreManager.GetScore;

        if (enemy.EnemyType == EnemyType.ToPlanet3) enemy.EventOnDivide += OnDivideSpawn;
        if (enemy.EnemyType == EnemyType.TP3mini) enemy.EventOnDistanceOver += DespawnEnemy;
    }
    /// <summary>
    /// 적의 이벤트를 제거합니다. Despawn 함수에 의해 발동됩니다.
    /// </summary>
    /// <param name="enemy"></param>
    void DeleteEventToEnemy(Enemy enemy)
    {
        enemy.EventContactCorrect -= OnContactCorrect;
        enemy.EventContactCorrect -= scoreManager.GetScore;

        enemy.EventContactWrong -= OnContactWrong;

        enemy.EventOnExplosion -= OnExplosion;
        enemy.EventOnExplosion -= scoreManager.GetScore;

        if (enemy.EnemyType == EnemyType.ToPlanet3) enemy.EventOnDivide -= OnDivideSpawn;
        if (enemy.EnemyType == EnemyType.TP3mini) enemy.EventOnDistanceOver -= DespawnEnemy;
    }
    /// <summary>
    /// 적의 이벤트를 초기화하고 비활성화하며, 소환된 적 목록에서 제거합니다.
    /// </summary>
    /// <param name="targetEnemy"></param>
    public void DespawnEnemy(Enemy targetEnemy)
    {
        DeleteEventToEnemy(targetEnemy);
        spawnedEnemies.Remove(targetEnemy);
        targetEnemy.gameObject.SetActive(false);
    }
    /// <summary>
    /// 대상 적을 파괴하고 효과를 재생하며, 플레이어의 체력을 회복시킵니다. 적이 올바른 대상에 접촉하여 제거되었을 때 호출됩니다.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="healing"></param>
    public void OnContactCorrect(Enemy owner, int healing)
    {
        if (owner.EnemyTarget == EnemyTarget.ToPlanet) GameManager.Instance.PlayerHPChange(false, healing);
        else if (owner.EnemyTarget == EnemyTarget.ToStar) GameManager.Instance.PlayerHPChange(true, healing);
        CallFXOnDestory(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }
    /// <summary>
    /// 대상 적을 파괴하고 효과를 재생하며, 플레이어에게 피해를 줍니다. 적이 플레이어에게 피해를 주며 자폭했을 때 호출됩니다.
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="damage"></param>
    public void OnContactWrong(Enemy owner, int damage)
    {
        if (owner.EnemyTarget == EnemyTarget.ToPlanet) GameManager.Instance.PlayerHPChange(true, -damage);
        else if (owner.EnemyTarget == EnemyTarget.ToStar) GameManager.Instance.PlayerHPChange(false, -damage);
        CallFXOnDestory(owner.EnemyType, false, owner.transform);
        DespawnEnemy(owner);
    }
    /// <summary>
    /// 대상 적을 파괴하고 효과를 재생합니다. 적이 폭발형 아이템에 의해 제거되었을 때 호출됩니다.
    /// </summary>
    /// <param name="owner"></param>
    public void OnExplosion(Enemy owner)
    {
        CallFXOnDestory(owner.EnemyType, true, owner.transform);
        DespawnEnemy(owner);
    }
    
    /// <summary>
    /// 파괴 시 파티클과 사운드 효과를 재생합니다. 피버 게이지를 채우는 효과는 올바른 대상에 의해 파괴되었을때만 발동됩니다.
    /// </summary>
    /// <param name="myType">파괴된 적의 종류</param>
    /// <param name="isCorrect">올바르게 파괴되었으면 true, 플레이어에게 피해를 주고 자폭했다면 false</param>
    /// <param name="_transform">파괴된 적의 위치</param>
    void CallFXOnDestory(EnemyType myType, bool isCorrect, Transform _transform)
    {
        switch (myType)
        {
            case EnemyType.ToPlanet1:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 3);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.ToPlanet2:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 5);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.ToPlanet3:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 17);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.ToPlanet4:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 24);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;


            case EnemyType.ToStar1:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 3);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.ToStar2:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 5);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.ToStar3:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 2);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.ToStar4:
                particleManager.SpawnParticle(ParticleType.DestroyTSsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 10);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;

            case EnemyType.TP3mini:
                particleManager.SpawnParticle(ParticleType.DestroyTPsmall, _transform);
                if (isCorrect)
                {
                    soundManager.PlayFXSound(SoundTypeFX.CorrectCol);
                    feverManager.CallParticle(_transform, 5);
                }
                else soundManager.PlayFXSound(SoundTypeFX.WrongCol);
                break;
        }
    }

    /// <summary>
    /// 소환되어 있는 모든 적의 이벤트를 해제하고 비활성하며, 소환 코루틴을 중지합니다.
    /// 씬을 종료할 때 GameManager에 의해 호출됩니다.
    /// </summary>
    public void AllEnemyEventReset()
    {
        foreach (var enemy in spawnedEnemies) if (enemy.gameObject.activeSelf) enemy.gameObject.SetActive(false);
        StopAllCoroutines();
        spawnedEnemies.Clear();
    }
    public void OnExitFeverTime()
    {
        feverTimeCount++;
        if(feverTimeCount >= 3)
        {
            feverTimeCount = 0;
            tier3TSSpawnCount++;
        }
        if (spawnDelay > 1.0f) spawnDelay = Mathf.Max(spawnDelay - 0.1f, 1.0f);
        else spawnDelay = Mathf.Lerp(spawnDelay, 0.1f, 0.1f);

        if (tier1Speed < 5f) tier1Speed += 0.2f;
    }
    public void OnSpawnControlByTime(int second)
    {
        float currentTime = (float)second;

        if (currentTime > 30f) tier2Probability = Mathf.Min(Mathf.Sqrt(currentTime - 30f) * 0.033f, 0.85f);
        if (currentTime > 50f) tier3Probability = Mathf.Min(Mathf.Sqrt(currentTime - 50f) * 0.02f, 0.7f);
        if (currentTime > 100f) tier4Probability = Mathf.Min(Mathf.Sqrt(currentTime - 100f) * 0.01f, 0.5f);

        if(spawnDelay > 1.5f) spawnDelay = Mathf.Max(spawnDelay - 0.01f, 1.5f);
    }

    public void OnSpawnControlByScore(int score)
    {
        float currentScore = (float)score;

        if (tier1Speed < 3f) tier1Speed = Mathf.Min(1f + currentScore * 0.005f, 3f);
        tier2cooltime = Mathf.Max(15f - Mathf.Sqrt(currentScore) * 0.5f, 2.2f);
        if (currentScore > 100f) tier3cooltime = Mathf.Max(20f - Mathf.Sqrt(currentScore - 100f) * 0.5f, 5f);
        if (currentScore > 200f) tier4cooltime = Mathf.Max(30f - Mathf.Sqrt(currentScore - 200f) * 0.5f, 10f);
    }
}
