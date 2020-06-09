using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using DG.Tweening;

public class TweenOnPointerEnter : TweenBase, IPointerEnterHandler,IPointerExitHandler
{
    #region 쓰잘데기 없는 부분
    [SerializeField, Space(8f)] bool onExitRestore = true;
    [HideIf(nameof(onExitRestore)), SerializeField, ReadOnly] string warning = "그거 함부로 끄면 맴매맞슴미다";
    #endregion
    public void OnPointerEnter(PointerEventData eventData)
    {
        DoChange();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        image.DOKill();
        DoOrigin();
    }
    void Start()
    {
        SetOrigin();
    }
}
