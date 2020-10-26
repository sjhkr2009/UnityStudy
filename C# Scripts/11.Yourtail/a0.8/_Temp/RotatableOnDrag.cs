using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class RotatableOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    float prevRot;
    float currentRot;
    float velocity;
    [SerializeField] float inertiaPoint = 10f;
    [SerializeField] float maxInertia = 360f;

    public float targetRotateLevel = float.MaxValue;
    float rotateLevel = 0f;

    public bool isRotateEnd = false;

    private void OnEnable()
    {
        isRotateEnd = false;
        rotateLevel = 0f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isRotateEnd) return;

        transform.DOKill();
        prevRot = transform.rotation.eulerAngles.z;
        velocity = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isRotateEnd) return;

        Vector3 dir = (Vector3)eventData.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
        currentRot = transform.rotation.eulerAngles.z;

        velocity = RotationChange(prevRot, currentRot);
        prevRot = currentRot;

        rotateLevel += Mathf.Abs(velocity);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isRotateEnd) return;

        float inertia = Mathf.Min(velocity * inertiaPoint, maxInertia);

        transform.DORotate(Vector3.forward * inertia, 1f, RotateMode.LocalAxisAdd).
                OnComplete(() =>
                {
                    rotateLevel += Mathf.Abs(inertia);
                    RotateEndCheck();
                }).
                OnKill(() =>
                {
                    rotateLevel += Mathf.Abs(RotationChange(prevRot, transform.rotation.eulerAngles.z));
                    RotateEndCheck();
                });

    }

    float RotationChange(float prev, float current)
    {
        float result = current - prev;

        if (result > 180f) result -= 360f;
        else if (result < -180f) result += 360f;

        return result;
    }

    //Option

    public event Action OnTargetRotate = () => { };

    void RotateEndCheck()
    {
        if (rotateLevel >= targetRotateLevel)
        {
            isRotateEnd = true;
            rotateLevel = 0f;

            transform.DORotate(Vector3.zero, 0.25f).
                OnComplete(() =>
                {
                    OnTargetRotate();
                });
        }
    }
}
