using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemManager : MonoBehaviour
{
    PoolManager poolManager;
    SoundManager soundManager;

    private void Start()
    {
        poolManager = GameManager.Instance.PoolManager;
        soundManager = GameManager.Instance.SoundManager;

    }


    void CreateExplosion(ItemBomb item)
    {
        Vector3 position = item.transform.position;

        switch (item.explosionType)
        {
            case ExplosionType.Hexagon:
                float size = Vector3.Distance(position, Vector3.zero);
                Explosion explosion = (Explosion)poolManager.Spawn(ObjectPool.ParticleHexagonExp, Vector3.zero, Quaternion.identity);
                explosion.transform.localScale = Vector3.one * size;
                soundManager.PlayFXSound(SoundTypeFX.HexagonBomb);
                break;
            case ExplosionType.Fixed:
                poolManager.Spawn(ObjectPool.ParticleExplosion, position, Quaternion.identity);
                soundManager.PlayFXSound(SoundTypeFX.NormalBomb);
                break;
        }
    }

    //아이템 소환 로직 추가
}
