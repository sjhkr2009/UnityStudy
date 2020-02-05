﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemManager : MonoBehaviour
{
    [SerializeField] PoolManager poolManager;
    [SerializeField] SoundManager soundManager;
    [SerializeField] ParticleManager particleManager;

    [TabGroup("Fixed Bomb"), SerializeField] private float minDelayOfFixedBomb;
    [TabGroup("Fixed Bomb"), SerializeField] private float maxDelayOfFixedBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] private float minDelayOfHexagonBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] private float maxDelayOfHexagonBomb;
    [TabGroup("Hexagon Bomb"), SerializeField] Transform planet;

    float screenView;

    private void Start()
    {
        if (poolManager == null) poolManager = GameManager.Instance.PoolManager;
        if (soundManager == null) soundManager = GameManager.Instance.SoundManager;
        if (particleManager == null) particleManager = GameManager.Instance.ParticleManager;

        screenView = GameManager.Instance.screenHorizontal / GameManager.Instance.screenVertical;

        StartCoroutine(nameof(HexagonBombSpawn));
        StartCoroutine(nameof(FixedBombSpawn));
    }


    void CreateExplosion(ItemBomb item)
    {
        switch (item.explosionType)
        {
            case ExplosionType.Hexagon:
                float size = Vector3.Distance(item.transform.position, Vector3.zero);
                Explosion explosion = (Explosion)particleManager.SpawnParticle(ParticleType.HexagonExplosion, planet);
                explosion.transform.localScale = Vector3.one * size;
                soundManager.PlayFXSound(SoundTypeFX.HexagonBomb);
                break;

            case ExplosionType.Fixed:
                particleManager.SpawnParticle(ParticleType.NormalExplosion, item.transform);
                soundManager.PlayFXSound(SoundTypeFX.NormalBomb);
                break;
        }
    }

    void OnBombExplosion(ItemBomb owner)
    {
        CreateExplosion(owner);
        owner.EventOnExplosion -= CreateExplosion;
        owner.gameObject.SetActive(false);
    }

    IEnumerator HexagonBombSpawn()
    {
        while (true)
        {
            float delay = Random.Range(minDelayOfHexagonBomb, maxDelayOfHexagonBomb);
            yield return new WaitForSeconds(delay);

            float cameraSizeX = Camera.main.orthographicSize * screenView;
            float cameraSizeY = Camera.main.orthographicSize;

            Vector3 spawnPosUp = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, cameraSizeY + 1f);
            Vector3 spawnPosRight = new Vector3(cameraSizeX + 1f, 0f, Random.Range(-cameraSizeY, cameraSizeY));
            Vector3 spawnPosDown = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, -cameraSizeY - 1f);
            Vector3 spawnPosLeft = new Vector3(-cameraSizeX - 1f, 0f, Random.Range(-cameraSizeY, cameraSizeY));

            float getPositionRandom = Random.value;

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
            newObject.EventOnExplosion += OnBombExplosion;
        }
    }

    IEnumerator FixedBombSpawn()
    {
        while (true)
        {
            float delay = Random.Range(minDelayOfFixedBomb, maxDelayOfFixedBomb);
            yield return new WaitForSeconds(delay);

            float cameraSizeX = Camera.main.orthographicSize * screenView;
            float cameraSizeY = Camera.main.orthographicSize;

            Vector3 spawnPos = Vector3.zero;

            while (Vector3.Distance(spawnPos, Vector3.zero) < 2.5f)
            {
                spawnPos = new Vector3(Random.Range(-cameraSizeX, cameraSizeX), 0f, Random.Range(-cameraSizeY, cameraSizeY));
            }
            ItemBomb newObject = (ItemBomb)poolManager.Spawn(ObjectPool.ItemFixedBomb, spawnPos, Quaternion.identity);
            newObject.EventOnExplosion += OnBombExplosion;
        }
    }
}
