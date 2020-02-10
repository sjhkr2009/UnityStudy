using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum WindowType { Background, GameOver, Pause, Sound }

public class PopUpAnimation : MonoBehaviour
{
    [SerializeField] WindowType windowType;
    [SerializeField] GameObject background;
    [SerializeField] Text titleText;
    private void OnEnable()
    {
        switch (windowType)
        {
            case WindowType.Background:
                transform.localScale = Vector3.one * 0.5f;
                transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
                break;
            case WindowType.GameOver:
                if (!background.activeSelf)
                {
                    transform.localScale = Vector3.one * 0.5f;
                    transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
                    background.SetActive(true);
                }
                titleText.text = "GAME OVER";
                break;
            case WindowType.Pause:
                if (!background.activeSelf)
                {
                    transform.localScale = Vector3.one * 0.5f;
                    transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
                    background.SetActive(true);
                }
                titleText.text = "OPTION";
                break;
            case WindowType.Sound:
                if (!background.activeSelf) background.SetActive(true);
                titleText.text = "SOUND";
                break;
        }
    }
}
