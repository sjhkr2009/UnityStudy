using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ItemBase : MonoBehaviour
{
    [BoxGroup("Basic"), SerializeField] bool isMoving;
    [BoxGroup("Basic"), ShowIf(nameof(isMoving)), SerializeField] private float moveSpeed;

    protected virtual void OnEnable()
    {
        transform.localScale = Vector3.one;
        if (GameManager.Instance.gameState != GameState.Playing)
        {
            gameObject.SetActive(false);
            return;
        }
        StartCoroutine(nameof(RangeCheck));
    }

    protected void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;
        if (isMoving) Move();   
    }
    protected void TimeOver() { if (gameObject.activeSelf) StartCoroutine(nameof(FadeOutDisable)); }

    IEnumerator FadeOutDisable()
    {
        transform.DOScale(Vector3.zero, 1f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    IEnumerator RangeCheck()
    {
        while (true)
        {
            if (Mathf.Abs(transform.position.y) > Camera.main.orthographicSize + 10f || Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * 9 / 16f + 10f) TimeOver();
            yield return new WaitForSeconds(1f);
        }
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
}