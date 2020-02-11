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

    [TabGroup("Fixed Bomb"), SerializeField] private float minDelayOfFixedBomb;
    [TabGroup("Fixed Bomb"), SerializeField] private float maxDelayOfFixedBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] private float minDelayOfHexagonBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] private float maxDelayOfHexagonBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] Transform planet;
    [TabGroup("Healkit"), SerializeField] private float minDelayOfHealkit;
    [TabGroup("Healkit"), SerializeField] private float maxDelayOfHealkit;

    float screenView;

    private void Start()
    {
        if (poolManager == null) poolManager = GameManager.Instance.PoolManager;
        if (soundManager == null) soundManager = GameManager.Instance.SoundManager;
        if (particleManager == null) particleManager = GameManager.Instance.ParticleManager;

        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;

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
        item.EventOnExplosion -= CreateExplosion;
        item.gameObject.SetActive(false);
    }

    IEnumerator HexagonBombSpawn()
    {
        while (true)
        {

            float delay = UnityEngine.Random.Range(minDelayOfHexagonBomb, maxDelayOfHexagonBomb);
            yield return new WaitForSeconds(delay);

            float cameraSizeX = Camera.main.orthographicSize * screenView;
            float cameraSizeY = Camera.main.orthographicSize;

            Vector3 spawnPosUp = new Vector3(UnityEngine.Random.Range(-cameraSizeX, cameraSizeX), 0f, cameraSizeY + 1f);
            Vector3 spawnPosRight = new Vector3(cameraSizeX + 1f, 0f, UnityEngine.Random.Range(-cameraSizeY, cameraSizeY));
            Vector3 spawnPosDown = new Vector3(UnityEngine.Random.Range(-cameraSizeX, cameraSizeX), 0f, -cameraSizeY - 1f);
            Vector3 spawnPosLeft = new Vector3(-cameraSizeX - 1f, 0f, UnityEngine.Random.Range(-cameraSizeY, cameraSizeY));

            float getPositionRandom = UnityEngine.Random.value;

            ItemBomb newObject = null;

            if (getPositionRandom < 0.25f)
            {
                newObject = (ItemBomb)poolManager.Spawn(ObjectPool.ItemHexagonBomb, spawnPosUp, Quaternion.LookRotation(spawnPosDown));
            }
            else if (getPositionRandom < 0.5f)
            {
                newObject = (ItemBomb)poolManager.Spawn(ObjectPool.ItemHexagonBomb, spawnPosRight, Quaternion.LookRotation(spawnPosLeft));
            }
            else if (getPositionRandom < 0.75f)
            {
                newObject = (ItemBomb)poolManager.Spawn(ObjectPool.ItemHexagonBomb, spawnPosDown, Quaternion.LookRotation(spawnPosUp));
            }
            else
            {
                newObject = (ItemBomb)poolManager.Spawn(ObjectPool.ItemHexagonBomb, spawnPosLeft, Quaternion.LookRotation(spawnPosRight));
            }
            newObject.EventOnExplosion += CreateExplosion;
        }
    }

    IEnumerator FixedBombSpawn()
    {
        while (true)
        {
            
            float delay = UnityEngine.Random.Range(minDelayOfFixedBomb, maxDelayOfFixedBomb);
            yield return new WaitForSeconds(delay);

            float cameraSizeX = Camera.main.orthographicSize * screenView;
            float cameraSizeY = Camera.main.orthographicSize;

            Vector3 spawnPos = Vector3.zero;

            while (Vector3.Distance(spawnPos, Vector3.zero) < 2.5f)
            {
                spawnPos = new Vector3(UnityEngine.Random.Range(-cameraSizeX * 0.95f, cameraSizeX * 0.95f), 0f, UnityEngine.Random.Range(-cameraSizeY * 0.95f, cameraSizeY * 0.95f));
            }
            ItemBomb newObject = (ItemBomb)poolManager.Spawn(ObjectPool.ItemFixedBomb, spawnPos, Quaternion.identity);
            newObject.EventOnExplosion += CreateExplosion;
        }
    }

    IEnumerator HealkitSpawn()
    {
        while (true)
        {
            
            float delay = UnityEngine.Random.Range(minDelayOfHealkit, maxDelayOfHealkit);
            yield return new WaitForSeconds(delay);

            float cameraSizeX = Camera.main.orthographicSize * screenView;
            float cameraSizeY = Camera.main.orthographicSize;

            Vector3 spawnPosUp = new Vector3(UnityEngine.Random.Range(-cameraSizeX, cameraSizeX), 0f, cameraSizeY + 1f);
            Vector3 spawnPosRight = new Vector3(cameraSizeX + 1f, 0f, UnityEngine.Random.Range(-cameraSizeY, cameraSizeY));
            Vector3 spawnPosDown = new Vector3(UnityEngine.Random.Range(-cameraSizeX, cameraSizeX), 0f, -cameraSizeY - 1f);
            Vector3 spawnPosLeft = new Vector3(-cameraSizeX - 1f, 0f, UnityEngine.Random.Range(-cameraSizeY, cameraSizeY));

            float getPositionRandom = UnityEngine.Random.value;

            ItemHeal newObject = null;

            if (getPositionRandom < 0.25f)
            {
                newObject = (ItemHeal)poolManager.Spawn(ObjectPool.ItemHeal, spawnPosUp, Quaternion.LookRotation(spawnPosDown));
            }
            else if (getPositionRandom < 0.5f)
            {
                newObject = (ItemHeal)poolManager.Spawn(ObjectPool.ItemHeal, spawnPosRight, Quaternion.LookRotation(spawnPosLeft));
            }
            else if (getPositionRandom < 0.75f)
            {
                newObject = (ItemHeal)poolManager.Spawn(ObjectPool.ItemHeal, spawnPosDown, Quaternion.LookRotation(spawnPosUp));
            }
            else
            {
                newObject = (ItemHeal)poolManager.Spawn(ObjectPool.ItemHeal, spawnPosLeft, Quaternion.LookRotation(spawnPosRight));
            }
            newObject.EventOnHealingPlanet += HealingToPlanet;
            newObject.EventOnHealingStar += HealingToStar;
        }
    }

    public void AllItemEventReset()
    {
        ItemBomb[] itemBombs = FindObjectsOfType<ItemBomb>();
        ItemHeal[] itemHeals = FindObjectsOfType<ItemHeal>();
        foreach (var item in itemBombs) if (item.gameObject.activeSelf) DisableExplosion(item);
        foreach (var item in itemHeals) if (item.gameObject.activeSelf) DisableHealkit(item);
        StopAllCoroutines();
    }
}
