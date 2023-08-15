using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkillButton : MonoBehaviour {
    [SerializeField] private GameObject cooldownRoot;
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text cooldownSecondText;

    public void ResetCallback(Action callback) {
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke());
    }

    public void SetCooldown(bool canUse, float remainCoolSecond, float originCooldown) {
        cooldownRoot.SetActive(!canUse);

        if (canUse) return;
        
        var remainCoolRatio = (originCooldown == 0f) ? 0f : remainCoolSecond / originCooldown;
        fillImage.fillAmount = remainCoolRatio;
        cooldownSecondText.text = $"{Mathf.CeilToInt(remainCoolSecond)}";
    }
}
