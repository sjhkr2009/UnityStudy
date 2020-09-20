using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenBase : MonoBehaviour
{
    #region 인스펙터 변수
    [DetailedInfoBox("이미지를 일정 시간에 걸쳐 부드럽게 변경합니다.", "이미지를 일정 시간에 걸쳐 부드럽게 변경합니다. 오브젝트가 움직이거나 크기가 달라지는 연출을 위해 사용하며, 주로 UI에 사용됩니다. 대상 오브젝트에 컴포넌트로 부착합니다." +
        "\n- 크기, 위치, 투명도를 변경할 수 있습니다." +
        "\n- Ignore Timescale : 유니티의 Time.timescale을 무시할지 여부를 결정합니다. 체크하면 게임 내 시간이 멈춰 있어도 애니메이션이 정상적으로 출력됩니다." +
        "\n- Duration : 변경할 시간" +
        "\n- Delay : 지연시간" +
        "\n- From / To : From일 경우 지정된 값에서 현재 값으로 변화합니다. To일 경우 현재 값에서 지정된 값으로 변화합니다." +
        "\n- Target : 변화시킬 값을 지정합니다." +
        "\n- Move Mode : 어떻게 변화시킬지 결정합니다. SetEase 사진을 참고하세요.")]
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool changeScale;
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool changePosition;
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool changeFade;
    [SerializeField, BoxGroup("애니메이션 종류 선택")] bool changeRotation;

    [SerializeField, BoxGroup("설정")] bool ignoreTimescale = true;
    [SerializeField, BoxGroup("설정")] float duration = 0.5f;
    [SerializeField, BoxGroup("설정")] float delay = 0f;
    public float Delay => delay;
    public float Duration => duration;

    [SerializeField] bool isFrom = false;

    [SerializeField, ShowIf(nameof(changeScale)), BoxGroup("설정"), PropertyOrder(1)] Vector3 scaleTarget;
    [SerializeField, ShowIf(nameof(changeScale)), BoxGroup("설정"), PropertyOrder(1)] Ease scaleMoveMode = Ease.InQuad;

    [SerializeField, ShowIf(nameof(changePosition)), BoxGroup("설정"), PropertyOrder(2)] Vector3 positionTarget;
    [SerializeField, ShowIf(nameof(changePosition)), BoxGroup("설정"), PropertyOrder(2)] Ease positionMoveMode = Ease.InQuad;

    [SerializeField, ShowIf(nameof(changeFade)), BoxGroup("설정"), MinValue(0f), MaxValue(1f), PropertyOrder(3)] float alphaTarget;
    [SerializeField, ShowIf(nameof(changeFade)), BoxGroup("설정"), PropertyOrder(3)] Ease alphaMoveMode = Ease.InQuad;
    [SerializeField, ShowIf(nameof(changeFade)), BoxGroup("설정"), PropertyOrder(3)] bool isUI = true;
    [SerializeField, ShowIf(nameof(changeFade)), ShowIf(nameof(isUI)), BoxGroup("설정"), PropertyOrder(3)] protected Graphic image;
    [SerializeField, ShowIf(nameof(changeFade)), HideIf(nameof(isUI)), BoxGroup("설정"), PropertyOrder(3)] protected SpriteRenderer sprite;

    [SerializeField, ShowIf(nameof(changeRotation)), BoxGroup("설정"), MinValue(0f), MaxValue(1f), PropertyOrder(4)] Vector3 rotationTarget;
    [SerializeField, ShowIf(nameof(changeRotation)), BoxGroup("설정"), PropertyOrder(4)] Ease rotationMoveMode = Ease.InQuad;
    [SerializeField, ShowIf(nameof(changeRotation)), BoxGroup("설정"), PropertyOrder(4)] RotateMode rotationRotateMode = RotateMode.Fast;
    #endregion

    [SerializeField, ReadOnly] Vector3 originScale;
    [SerializeField, ReadOnly] Vector3 originPos;
    [SerializeField, ReadOnly] Vector3 originRot;
    [SerializeField, ReadOnly] float originAlpha;

    private void Awake()
    {
        if (isUI && image == null) image = GetComponent<Graphic>();
        if (!isUI && sprite == null) sprite = GetComponent<SpriteRenderer>();
        SetOrigin();
    }

    protected void ChangeRotation()
    {
        Vector3 target = transform.rotation.eulerAngles;
        if (isFrom) transform.rotation = Quaternion.Euler(rotationTarget);
        else target = rotationTarget;

        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DORotate(rotationTarget, duration, rotationRotateMode).SetEase(rotationMoveMode);
        }, ignoreTimescale);
    }

    protected void ChangeScale()
    {
        Vector3 target = transform.localScale;
        if (isFrom) transform.localScale = scaleTarget;
        else target = scaleTarget;

        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOScale(target, duration).SetEase(scaleMoveMode);
        }, ignoreTimescale);
    }

    protected void ChangePosition()
    {
        Vector3 target = transform.position;
        if (isFrom) transform.position = positionTarget;
        else target = positionTarget;

        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOMove(target, duration).SetEase(positionMoveMode);
        }, ignoreTimescale);
    }

    protected void ChangeFade()
    {
        if (image == null && sprite == null) return;
        float target = isUI ? image.color.a : sprite.color.a;

        if (isFrom)
        {
            if (isUI) image.DOFade(alphaTarget, 0f);
            else sprite.DOFade(alphaTarget, 0f);
        }
        else target = alphaTarget;

        DOVirtual.DelayedCall(delay, () =>
        {
            if(isUI) image.DOFade(target, duration).SetEase(alphaMoveMode);
            else sprite.DOFade(target, duration).SetEase(alphaMoveMode);
        }, ignoreTimescale);
    }

    protected void SetOrigin()
    {
        if (changeScale) originScale = transform.localScale;
        if (changePosition) originPos = transform.position;
        if (changeFade && (sprite != null || image != null)) originAlpha = isUI ? image.color.a : sprite.color.a;
        if (changeRotation) originRot = transform.rotation.eulerAngles;
    }

    public void DoOrigin()
    {
        if (changeScale)
            transform.localScale = originScale;
        if (changePosition)
            transform.position = originPos;
        if (changeFade && (sprite != null || image != null))
        {
            if (isUI)
                image.color = new Color(image.color.r, image.color.g, image.color.b, originAlpha);
            else
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, originAlpha);
        }
        if (changeRotation)
            transform.rotation = Quaternion.Euler(originRot);
    }

    public void DoChange()
    {
        if (changePosition) ChangePosition();
        if (changeScale) ChangeScale();
        if (changeFade) ChangeFade();
        if (changeRotation) ChangeRotation();
    }

    [Button("현재 값을 타겟으로 설정")]
    void SetTarget()
    {
        SetOrigin();
        if (changeScale) scaleTarget = originScale;
        if (changePosition) positionTarget = originPos;
        if (changeFade && (sprite != null || image != null)) alphaTarget = originAlpha;
        if (changeRotation) rotationTarget = originRot;
    }

    protected virtual void OnDisable()
    {
        transform.DOKill();
    }
	private void OnDestroy()
	{
        transform.DOKill();
	}
}
