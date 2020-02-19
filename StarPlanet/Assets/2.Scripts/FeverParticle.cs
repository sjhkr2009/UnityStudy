using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class FeverParticle : MonoBehaviour
{
    [SerializeField] Transform targetPositionOnScreen;
    [SerializeField, ReadOnly] Vector3 targetPos;

    private void Awake()
    {
        targetPos = Camera.main.ScreenToWorldPoint(targetPositionOnScreen.position);
    }

    private void OnEnable()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
        {
            gameObject.SetActive(false);
            return;
        }
        if(targetPos == null || targetPos == Vector3.zero) targetPos = Camera.main.ScreenToWorldPoint(targetPositionOnScreen.position);

        float duration = Random.Range(0.8f, 1.5f);
        transform.localScale = Vector3.one;
        transform.DOScale(0.3f, duration).SetEase(Ease.InCirc);
        transform.DOMove(targetPos, duration * 1.1f).SetEase(Ease.InCirc)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                DOVirtual.DelayedCall(duration, () => { GameManager.Instance.FeverManager.GetFeverCount(1); });
            });
    }
}
