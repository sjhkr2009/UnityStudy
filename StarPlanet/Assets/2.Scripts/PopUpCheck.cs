using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum WindowType { Background, GameOver, Pause, Sound }

public class PopUpCheck : MonoBehaviour
{
    public WindowType windowType;
    [SerializeField] GameObject background;
    [SerializeField] Text titleText;
    private void OnEnable()
    {
        switch (windowType)
        {
            case WindowType.Background:
                break;
            case WindowType.GameOver:
                if (!background.activeSelf) background.SetActive(true);
                titleText.text = "GAME OVER";
                break;
            case WindowType.Pause:
                if (!background.activeSelf) background.SetActive(true);
                titleText.text = "OPTION";
                break;
            case WindowType.Sound:
                if (!background.activeSelf) background.SetActive(true);
                titleText.text = "SOUND";
                break;
        }
    }
}
