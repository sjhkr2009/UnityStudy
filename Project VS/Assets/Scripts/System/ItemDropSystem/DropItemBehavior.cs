using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DropItemBehavior : MonoBehaviour, IPoolHandler {
    [SerializeField] private DropItemIndex itemIndex;

    private bool isAnimating = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (!isAnimating && other.CompareTag(Define.Tag.ItemGainer)) {
            AnimateGain();
        }
        
        if (other.CompareTag(Define.Tag.Player)) {
            Gain();
        }
    }

    public void AnimateGain() {
        if (isAnimating) return;
        
        isAnimating = true;

        if (GameManager.Player) {
            var destTr = GameManager.Player.transform;
            transform.SetParent(destTr);
            var duration = 0.3f + (transform.localPosition.magnitude * 0.05f).Clamp(0f, 1f);
            transform.DOLocalMove(Vector3.zero, duration).SetEase(Ease.InBack, 0.5f)
                .SetId(GetInstanceID())
                .OnComplete(Gain);
        } else {
            Debugger.Error("[DropItemBehavior.AnimateGain] Cannot find player!");
            Gain();
        }
    }

    void Gain() {
        PoolManager.Abandon(gameObject);
        DropItemManager.GainItem(itemIndex);
    }

    public bool CanGainByMagnetic() {
        return itemIndex switch {
            DropItemIndex.SmallSoul => true,
            DropItemIndex.MiddleSoul => true,
            DropItemIndex.BigSoul => true,
            _ => false
        };
    }

    public void OnInitialize() {
        DropItemManager.SpawnedDropItems.Add(this);
        isAnimating = false;
    }

    public void OnRelease() {
        DropItemManager.SpawnedDropItems.Remove(this);
        DOTween.Kill(GetInstanceID());
        isAnimating = false;
    }
}
