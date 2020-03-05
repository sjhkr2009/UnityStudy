using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ItemManager : MonoBehaviour
{
    public event Action<int> EventOnScoreChange = n => { };

    PoolManager poolManager;
    SpawnManager spawnManager;
    SoundManager soundManager;
    ParticleManager particleManager;

    [TabGroup("Fixed Bomb"), SerializeField] private float delayOfFixedBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] private float delayOfHexagonBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] Transform planet;
    [TabGroup("Healkit"), SerializeField] private float delayOfHealkit;

    List<ItemBomb> spawnedBombs = new List<ItemBomb>();
    List<ItemHeal> spawnedHealkits = new List<ItemHeal>();

    private void Start()
    {
        poolManager = GameManager.Instance.PoolManager;
        spawnManager = GameManager.Instance.SpawnManager;
        soundManager = GameManager.Instance.SoundManager;
        particleManager = GameManager.Instance.ParticleManager;

        if (spawnedBombs.Count != 0) spawnedBombs.Clear();
        if (spawnedHealkits.Count != 0) spawnedHealkits.Clear();

        StartCoroutine(nameof(HexagonBombSpawn));
        StartCoroutine(nameof(FixedBombSpawn));
        StartCoroutine(nameof(HealkitSpawn));
    }

    void HealingToStar(ItemHeal item, int healValue)
    {
        GameManager.Instance.PlayerHPChange(true, healValue);
        particleManager.SpawnParticle(ParticleType.Healing, item.transform);
        soundManager.PlayFXSound(SoundTypeFX.Healing);

        DisableHealkit(item);
    }

    void HealingToPlanet(ItemHeal item, int healValue)
    {
        GameManager.Instance.PlayerHPChange(false, healValue);
        particleManager.SpawnParticle(ParticleType.Healing, item.transform);
        soundManager.PlayFXSound(SoundTypeFX.Healing);

        DisableHealkit(item);
    }

    void DisableHealkit(ItemHeal item)
    {
        spawnedHealkits.Remove(item);
        item.EventOnHealingPlanet -= HealingToPlanet;
        item.EventOnHealingStar -= HealingToStar;
        item.gameObject.SetActive(false);
    }

    void CreateExplosion(ItemBomb owner)
    {
        switch (owner.explosionType)
        {
            case ExplosionType.Hexagon:
                float size = Vector3.Distance(owner.transform.position, Vector3.zero);
                Explosion explosion = (Explosion)poolManager.Spawn(ObjectPool.ParticleHexagonExp, planet.position, Quaternion.identity);
                explosion.transform.localScale = Vector3.one * size;
                soundManager.PlayFXSound(SoundTypeFX.HexagonBomb);
                Camera.main.transform.DOShakePosition(0.5f);
                break;

            case ExplosionType.Fixed:
                particleManager.SpawnParticle(ParticleType.NormalExplosion, owner.transform);
                soundManager.PlayFXSound(SoundTypeFX.NormalBomb);
                break;
        }
        DisableExplosion(owner);
    }

    void DisableExplosion(ItemBomb item)
    {
        spawnedBombs.Remove(item);
        item.EventOnExplosion -= CreateExplosion;
        item.gameObject.SetActive(false);
    }

    IEnumerator HexagonBombSpawn()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(delayOfHexagonBomb * 0.6f, delayOfHexagonBomb * 1.4f);
            yield return new WaitForSeconds(delay);

            ItemBomb newObject = (ItemBomb)spawnManager.SpawnOverMapToRandom(ObjectPool.ItemHexagonBomb);
            newObject.EventOnExplosion += CreateExplosion;
            if (!spawnedBombs.Contains(newObject)) spawnedBombs.Add(newObject);
        }
    }

    IEnumerator FixedBombSpawn()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(delayOfFixedBomb * 0.8f, delayOfFixedBomb * 1.2f);
            yield return new WaitForSeconds(delay);

            ItemBomb newObject = (ItemBomb)spawnManager.SpawnOnMap(ObjectPool.ItemFixedBomb);
            newObject.EventOnExplosion += CreateExplosion;
            if(!spawnedBombs.Contains(newObject)) spawnedBombs.Add(newObject);
        }
    }

    IEnumerator HealkitSpawn()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(delayOfHealkit * 0.8f, delayOfHealkit * 1.2f);
            yield return new WaitForSeconds(delay);

            ItemHeal newObject = (ItemHeal)spawnManager.SpawnOverMapToRandom(ObjectPool.ItemHeal);
            newObject.EventOnHealingPlanet += HealingToPlanet;
            newObject.EventOnHealingStar += HealingToStar;
            if (!spawnedHealkits.Contains(newObject)) spawnedHealkits.Add(newObject);
        }
    }

    public void BonusHealkitSpawnChance()
    {
        float starHp = GameManager.Instance.StarHpRate();
        float planetHp = GameManager.Instance.PlanetHpRate();
        float hpRate = Mathf.Min(starHp, planetHp);

        if(UnityEngine.Random.value > Mathf.Min(0.35f + hpRate, 0.95f))
        {
            for (int i = 0; i < 10; i++) BonusHealkitSpawn();
        }
    }

    public void BonusHealkitSpawn()
    {
        ItemHeal newObject = (ItemHeal)spawnManager.SpawnOverMapToRandom(ObjectPool.ItemHeal);
        newObject.EventOnHealingPlanet += HealingToPlanet;
        newObject.EventOnHealingStar += HealingToStar;
        if (!spawnedHealkits.Contains(newObject)) spawnedHealkits.Add(newObject);
    }

    public void AllItemEventReset()
    {
        foreach (var item in spawnedBombs) if (item.gameObject.activeSelf) item.EventOnExplosion -= CreateExplosion;
        foreach (var item in spawnedHealkits) if (item.gameObject.activeSelf)
            {
                item.EventOnHealingPlanet -= HealingToPlanet;
                item.EventOnHealingStar -= HealingToStar;
            }
        StopAllCoroutines();
        spawnedBombs.Clear();
        spawnedHealkits.Clear();
    }

    public void OnSpawnControlByScore(int score)
    {
        float currentScore = (float)score;

        delayOfFixedBomb = Mathf.Max(40f - (Mathf.Sqrt(currentScore) * 1f), 10f);
        delayOfHexagonBomb = Mathf.Max(90f - (Mathf.Sqrt(currentScore) * 2f), 20f);
        delayOfHealkit = Mathf.Max(35f - (Mathf.Sqrt(currentScore) * 0.5f), 15f);
    }
}
