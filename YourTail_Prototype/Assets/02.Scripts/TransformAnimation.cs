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
     * from: 변경 전의 값
     * to: 변경 후의 값
     */

    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool isChangeScale;
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool isChangePosition;
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool isChangeRotation;

    [SerializeField, BoxGroup("옵션")] bool ignoreTimescale;
    [SerializeField, BoxGroup("옵션")] bool playOnStart;

    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] float positionDuration;
    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] float positionDelay;
    [SerializeField, ShowIf(nameof(isChangePosition)), TabGroup("Position")] bool setPositionVector;
    [SerializeField, ShowIf(nameof(isChangePosition)), HideIf(nameof(setPositionVector)), TabGroup("Position")] Transform positionFromTransform;
    [SerializeField, ShowIf(nameof(isChangePosition)), HideIf(nameof(setPositionVector)), TabGroup("Position")] Transform positionToTransform;
    [SerializeField, ShowIf(nameof(isChangePosition)), ShowIf(nameof(setPositionVector)), TabGroup("Position")] Vector3 positionFromVector;
    [SerializeField, ShowIf(nameof(isChangePosition)), ShowIf(nameof(setPositionVector)), TabGroup("Position")] Vector3 positionToVector;

    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] float scaleDuration;
    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] float scaleDelay;
    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] float scaleFrom;
    [SerializeField, ShowIf(nameof(isChangeScale)), TabGroup("Scale")] float scaleTo;
    public void ChangeScale()
    {
        transform.DOScale(scaleFrom, 0f);
        DOVirtual.DelayedCall(scaleDelay, () =>
        {
            transform.DOScale(scaleTo, scaleDuration);
        }, ignoreTimescale);
    }

    public void ChangePosition()
    {
        if (!setPositionVector)
        {
            positionFromVector = positionFromTransform.position;
            positionToVector = positionToTransform.position;
        }

        transform.DOMove(positionFromVector, 0f);
        DOVirtual.DelayedCall(positionDelay, () =>
        {
            transform.DOMove(positionToVector, positionDuration);
        }, ignoreTimescale);
    }



    void Start()
    {
        if (playOnStart)
        {
            if(isChangeScale) ChangeScale();
            if(isChangePosition) ChangePosition();
        }
    }
}
