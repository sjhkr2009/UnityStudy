using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class WindowPopup : MonoBehaviour
{
    [SerializeField, ReadOnly] RectTransform rectTransform;
    [SerializeField] float popupTime = 0.2f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if(gameObject.activeSelf) gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        rectTransform.DOScale(1f, popupTime).SetEase(Ease.OutBack);
    }
}
