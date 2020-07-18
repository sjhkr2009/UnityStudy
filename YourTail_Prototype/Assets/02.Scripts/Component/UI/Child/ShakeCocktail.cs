using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ShakeCocktail : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField, ReadOnly] bool isEnd = false;
    Vector2 originPos;
    Vector2 startPos;
    Vector2 prevPos;

    public GameObject parentCanvas;
    public Slider processUI;
    public Image processColor;
    float process;

    private void Start()
    {
        originPos = transform.position;
        StartCoroutine(nameof(Making));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEnd) return;

        transform.DOKill();
        startPos = eventData.position;

        if(originPos.x < startPos.x)
        {
            transform.DORotate(new Vector3(0f, 0f, 20f), 0.75f);
        }
        else
        {
            transform.DORotate(new Vector3(0f, 0f, -20f), 0.75f);
        }
        transform.position = eventData.position;
        prevPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isEnd) return;

        transform.position = eventData.position;
        process += Mathf.Clamp(((Vector2)transform.position - prevPos).magnitude, 0f, 30f * Time.deltaTime);
        prevPos = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEnd) return;

        transform.DOMove(originPos, 1.5f);
        transform.DORotate(Vector3.zero, 0.66f);
    }

    IEnumerator Making()
    {
        process = 0f;
        UpdateProcessUI(0);
        while (process <= 100f)
        {
            yield return null;

            process += (10f * Time.deltaTime);
            UpdateProcessUI(process);
        }
        transform.DOKill();
        isEnd = true;
        GameManager.UI.ClosePopupUI<CocktailMaking>();
    }

    void UpdateProcessUI(float percent)
    {
        processUI.value = percent / 100f;
        Color color = Color.HSVToRGB((percent + 25f)/256f, 0.85f, 0.95f);
        processColor.color = color;
    }
}
