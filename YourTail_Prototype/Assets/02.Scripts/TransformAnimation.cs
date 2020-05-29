using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class TransformAnimation : MonoBehaviour
{
    //Transform의 값을 일정 시간에 걸쳐 변경합니다. 오브젝트가 움직이거나 크기가 달라지는 연출을 위해 사용합니다. 대상 오브젝트에 컴포넌트로 부착합니다.
    //주로 타이틀 화면에서, 버튼 등의 UI에 적용됩니다.

    /* 
     * duration : 몇 초에 걸쳐서 변경할 것인지
     * delay: 명령을 받은 후 몇 초간 지연 후에 변경할 것인지
     * from: 변경 전의 값 (벡터값을 직접 입력하거나, 해당 위치의 Transform 또는 크기 비율을 float 형태로 입력할 수 있음)
     * to: 변경 후의 값
     * curve: 변경 과정에서의 움직임. Script 폴더의 TransformAnimation_SetEase 참고.
     */

    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool isChangeScale;
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool isChangePosition;

    [SerializeField, BoxGroup("옵션")] bool ignoreTimescale;
    [SerializeField, BoxGroup("옵션")] bool playOnStart = true;
    [SerializeField, BoxGroup("옵션"), ShowIf(nameof(playOnStart))] bool startOnFrom;

    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] float positionDuration;
    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] float positionDelay;
    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] bool setPositionInVector;
    [SerializeField, ShowIf(nameof(isChangePosition)), HideIf(nameof(setPositionInVector)), TabGroup("Position")] Transform positionFromTransform;
    [SerializeField, ShowIf(nameof(isChangePosition)), HideIf(nameof(setPositionInVector)), TabGroup("Position")] Transform positionToTransform;
    [SerializeField, ShowIf(nameof(isChangePosition)), ShowIf(nameof(setPositionInVector)), TabGroup("Position")] Vector3 positionFromVector;
    [SerializeField, ShowIf(nameof(isChangePosition)), ShowIf(nameof(setPositionInVector)), TabGroup("Position")] Vector3 positionToVector;
    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] Ease positionCurve;

    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] float scaleDuration;
    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] float scaleDelay;
    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] bool setScaleInVector;
    [SerializeField, ShowIf(nameof(isChangeScale)), HideIf(nameof(setScaleInVector)), TabGroup("Scale")] float scaleFromFloat;
    [SerializeField, ShowIf(nameof(isChangeScale)), HideIf(nameof(setScaleInVector)), TabGroup("Scale")] float scaleToFloat;
    [SerializeField, ShowIf(nameof(isChangeScale)), ShowIf(nameof(setScaleInVector)), TabGroup("Scale")] Vector3 scaleFromVector;
    [SerializeField, ShowIf(nameof(isChangeScale)), ShowIf(nameof(setScaleInVector)), TabGroup("Scale")] Vector3 scaleToVector;
    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] Ease scaleCurve;

    public void ChangeScale(Vector3 from, Vector3 to, float duration, Ease curve, float delay = 0f, bool ignoreTime = true)
    {
        transform.DOScale(from, 0f);
        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOScale(to, duration).SetEase(curve);
        }, ignoreTime);
    }

    public void ChangePosition(Vector3 from, Vector3 to, float duration, Ease curve, float delay = 0f, bool ignoreTime = true)
    {
        transform.position = from;
        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOMove(to, duration).SetEase(curve);
        }, ignoreTime);
    }


    public void ChangeScaleTo()
    {
        if (setScaleInVector) ChangeScale(transform.localScale, scaleToVector, scaleDuration, scaleCurve, scaleDelay, ignoreTimescale);
        else ChangeScale(transform.localScale, transform.localScale * scaleToFloat, scaleDuration, scaleCurve, scaleDelay, ignoreTimescale);
    }

    public void ChangeScaleFrom()
    {
        if (setScaleInVector) ChangeScale(scaleFromVector, transform.localScale, scaleDuration, scaleCurve, scaleDelay, ignoreTimescale);
        else ChangeScale(transform.localScale * scaleFromFloat, transform.localScale, scaleDuration, scaleCurve, scaleDelay, ignoreTimescale);
    }

    public void ChangePositionTo()
    {
        //ChangePosition(
        
        if (!setPositionInVector) positionToVector = positionToTransform.position;

        DOVirtual.DelayedCall(positionDelay, () =>
        {
            transform.DOMove(positionToVector, positionDuration).SetEase(positionCurve);
        }, ignoreTimescale);
    }

    public void ChangePositionFrom()
    {
        Vector3 toPosition = transform.position;
        if (!setPositionInVector) positionFromVector = positionFromTransform.position;

        transform.position = positionFromVector;
        DOVirtual.DelayedCall(positionDelay, () =>
        {
            transform.DOMove(toPosition, positionDuration).SetEase(positionCurve);
        }, ignoreTimescale);
    }

    void Start()
    {
        if (playOnStart)
        {
            if (!startOnFrom)
            {
                if (isChangeScale) ChangeScaleTo();
                if (isChangePosition) ChangePositionTo();
            }
            else
            {
                if (isChangeScale) ChangeScaleFrom();
                if (isChangePosition) ChangePositionFrom();
            }
        }
    }
}
