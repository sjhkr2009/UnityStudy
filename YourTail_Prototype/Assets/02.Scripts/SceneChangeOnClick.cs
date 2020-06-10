using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SceneChangeOnClick : SceneChange, IPointerClickHandler
{
    public float delay = 0f;

    void Start()
    {
        TweenOnClick tweenOnClick = GetComponent<TweenOnClick>();
        if (tweenOnClick != null && delay == 0f) delay = tweenOnClick.Duration;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DOVirtual.DelayedCall(delay, () => { Change(sceneName); });
    }
}
