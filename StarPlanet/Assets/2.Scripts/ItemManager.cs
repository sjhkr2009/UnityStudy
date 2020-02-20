using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ItemManager : MonoBehaviour
{
    public event Action<int> EventOnScoreChange = n => { };
    
    [SerializeField] PoolManager poolManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] ParticleManager particleManager;

    [TabGroup("Fixed Bomb"), SerializeField] private float delayOfFixedBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] private float delayOfHexagonBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] Transform planet;
    [TabGroup("Healkit"), SerializeField] private float delayOfHealkit;

    float screenView;
    float cameraSizeX;
    float cameraSizeY;
    List<ItemBomb> spawnedBombs = new List<ItemBomb>();
    List<ItemHeal> spawnedHealkits = new List<ItemHeal>();

    private void Start()
    {
        if (poolManager == null) poolManager = GameManager.Instance.PoolManager;
        if (soundManager == null) soundManager = GameManager.Instance.SoundManager;
        if (particleManager == null) particleManager = GameManager.Instance.ParticleManager;

        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;
        cameraSizeX = Camera.main.orthographicSize * screenView;
        cameraSizeY = Camera.main.orthographicSize;

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
                Explosion explosion = (Explosion)particleManager.SpawnAndGetParticle(ParticleType.HexagonExplosion, planet);
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

    Component SpawnItemOnMap(ObjectPool objectType)
    {
        Vector3 spawnPos = Vector3.zero;
        while (Vector3.Distance(spawnPos, Vector3.zero) < 2.5f)
        {
            spawnPos = new Vector3(UnityEngine.Random.Range(-cameraSizeX * 0.95f, cameraSizeX * 0.95f), 0f, UnityEngine.Random.Range(-cameraSizeY * 0.95f, cameraSizeY * 0.95f));
        }
        Component newObject = poolManager.Spawn(objectType, spawnPos, Quaternion.identity);
        return newObject;
    }
    Component SpawnItemOverMap(ObjectPool objectType)
    {
        Vector3 spawnPosUp = new Vector3(UnityEngine.Random.Range(-cameraSizeX, cameraSizeX), 0f, cameraSizeY + 1f);
        Vector3 spawnPosRight = new Vector3(cameraSizeX + 1f, 0f, UnityEngine.Random.Range(-cameraSizeY, cameraSizeY));
        Vector3 spawnPosDown = new Vector3(UnityEngine.Random.Range(-cameraSizeX, cameraSizeX), 0f, -cameraSizeY - 1f);
        Vector3 spawnPosLeft = new Vector3(-cameraSizeX - 1f, 0f, UnityEngine.Random.Range(-cameraSizeY, cameraSizeY));

        float getPositionRandom = UnityEngine.Random.value;
        Component newObject = null;

        if (getPositionRandom < 0.25f) newObject = poolManager.Spawn(objectType, spawnPosUp, Quaternion.LookRotation(spawnPosDown));
        else if (getPositionRandom < 0.5f) newObject = poolManager.Spawn(objectType, spawnPosRight, Quaternion.LookRotation(spawnPosLeft));
        else if (getPositionRandom < 0.75f) newObject = poolManager.Spawn(objectType, spawnPosDown, Quaternion.LookRotation(spawnPosUp));
        else newObject = poolManager.Spawn(objectType, spawnPosLeft, Quaternion.LookRotation(spawnPosRight));
        return newObject;
    }

    IEnumerator HexagonBombSpawn()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(delayOfHexagonBomb * 0.7f, delayOfHexagonBomb * 1.3f);
            yield return new WaitForSeconds(delay);

            ItemBomb newObject = (ItemBomb)SpawnItemOverMap(ObjectPool.ItemHexagonBomb);
            newObject.EventOnExplosion += CreateExplosion;
            if (!spawnedBombs.Contains(newObject)) spawnedBombs.Add(newObject);
        }
    }

    IEnumerator FixedBombSpawn()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(delayOfFixedBomb * 0.5f, delayOfFixedBomb * 1.5f);
            yield return new WaitForSeconds(delay);

            ItemBomb newObject = (ItemBomb)SpawnItemOnMap(ObjectPool.ItemFixedBomb);
            newObject.EventOnExplosion += CreateExplosion;
            if(!spawnedBombs.Contains(newObject)) spawnedBombs.Add(newObject);
        }
    }

    IEnumerator HealkitSpawn()
    {
        while (true)
        {
            float delay = UnityEngine.Random.Range(delayOfHealkit * 0.7f, delayOfHealkit * 1.3f);
            yield return new WaitForSeconds(delay);

            ItemHeal newObject = (ItemHeal)SpawnItemOverMap(ObjectPool.ItemHeal);
            newObject.EventOnHealingPlanet += HealingToPlanet;
            newObject.EventOnHealingStar += HealingToStar;
            if (!spawnedHealkits.Contains(newObject)) spawnedHealkits.Add(newObject);
        }
    }

    public void AllItemEventReset()
    {
        foreach (var item in spawnedBombs) if (item.gameObject.activeSelf) DisableExplosion(item);
        foreach (var item in spawnedHealkits) if (item.gameObject.activeSelf) DisableHealkit(item);
        StopAllCoroutines();
        spawnedBombs.Clear();
        spawnedHealkits.Clear();
    }
}
