using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnScrolling : MonoBehaviour, IDragHandler, IPointerUpHandler
{
    ScrollRect sr;

    public void OnDrag(PointerEventData p)
    {
        //Debug.Log($"스크롤링 속도: {sr.velocity}");
    }

    public void OnPointerUp(PointerEventData p)
    {
        Debug.Log($"[스크롤 종료] 가로축 위치: {sr.horizontalNormalizedPosition}");
        
    }

    private void Start()
    {
        sr = gameObject.GetOrAddComponent<ScrollRect>();
    }

}
