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
    List<Enemy> spawnedEnemies = new List<Enemy>();

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
            if (!spawnedEnemies.Contains(enemy)) spawnedEnemies.Add(enemy);

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
    /// 적 개체 중 분열하는 형태의 적에게 적용됩니다. 입력한 타입의 적을 8방향으로 소환합니다. 기본값은 Enemy To Planet 1 입니다.
    /// </summary>
    /// <param name="spawnTransform"></param>
    void OnDivideSpawn(Transform spawnTransform, ObjectPool type = ObjectPool.EnemyTP1)
    {
        particleManager.SpawnParticle(ParticleType.DestroyTPsmall, spawnTransform); //분열 이펙트로 변경할 것

        Enemy enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + Vector3.forward));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + Vector3.back));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + Vector3.right));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + Vector3.left));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + new Vector3(1, 0, 1)));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + new Vector3(-1, 0, 1)));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + new Vector3(1, 0, -1)));
        AddEventToEnemy(enemy);

        enemy = (Enemy)poolManager.Spawn(type, spawnTransform.position, Quaternion.LookRotation(spawnTransform.position + new Vector3(-1, 0, -1)));
        AddEventToEnemy(enemy);
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

        if (enemy.EnemyType == EnemyType.ToPlanet4) enemy.EventOnDivide += OnDivideSpawn;
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

        if (enemy.EnemyType == EnemyType.ToPlanet4) enemy.EventOnDivide -= OnDivideSpawn;
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
        }
    }

    /// <summary>
    /// 소환되어 있는 모든 적의 이벤트를 해제하고 비활성하며, 소환 코루틴을 중지합니다.
    /// 씬을 종료할 때 GameManager에 의해 호출됩니다.
    /// </summary>
    public void AllEnemyEventReset()
    {
        foreach (var enemy in spawnedEnemies) if (enemy.gameObject.activeSelf) DespawnEnemy(enemy);
        StopAllCoroutines();
        spawnedEnemies.Clear();
    }
}
