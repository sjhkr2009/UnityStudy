using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum ExplosionType { Hexagon, Fixed }

public class ItemBomb : MonoBehaviour
{
    public event Action<ItemBomb> EventOnExplosion = (I) => { };
    
    [BoxGroup("Basic")] public ExplosionType explosionType;
    [BoxGroup("Basic"), SerializeField] private float durationTime;
    [SerializeField] bool isHeaxgon;
    [ShowIf(nameof(isHeaxgon)), SerializeField] private float moveSpeed;

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        if (GameManager.Instance.gameState != GameState.Playing) return;

        if (explosionType == ExplosionType.Fixed) Invoke(nameof(TimeOver), durationTime);
        StartCoroutine(nameof(RangeCheck));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Star") || other.CompareTag("Fever") || other.CompareTag("Planet"))
        {
            EventOnExplosion(this);
        }
    }
    private void Move()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void Update()
    {
        if (explosionType == ExplosionType.Hexagon) Move();

        if (Mathf.Abs(transform.position.y) > Camera.main.orthographicSize + 10f || Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * 9/16f + 10f) TimeOver();
    }

    private void TimeOver() { StartCoroutine(nameof(FadeOutDisable)); }

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

    private void OnDisable()
    {
        StopCoroutine(nameof(RangeCheck));
        CancelInvoke();
    }
}
