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

    [SerializeField, BoxGroup("설정")] bool ignoreTimescale = true;
    [SerializeField, BoxGroup("설정")] float duration = 0.5f;
    [SerializeField, BoxGroup("설정")] float delay = 0f;
    public float Delay => delay;
    public float Duration => duration;

    bool isFrom = false;
    [Button("현재 모드 : To", ButtonSizes.Medium), HideIf(nameof(isFrom)), BoxGroup("설정")] void SetFrom() { isFrom = true; }
    [Button("현재 모드 : From", ButtonSizes.Medium), ShowIf(nameof(isFrom)), BoxGroup("설정")] void SetTo() { isFrom = false; }

    [SerializeField, ShowIf(nameof(changeScale)), BoxGroup("설정"), PropertyOrder(1)] Vector3 scaleTarget;
    [SerializeField, ShowIf(nameof(changeScale)), BoxGroup("설정"), PropertyOrder(1)] Ease scaleMoveMode = Ease.InQuad;

    [SerializeField, ShowIf(nameof(changePosition)), BoxGroup("설정"), PropertyOrder(2)] Vector3 positionTarget;
    [SerializeField, ShowIf(nameof(changePosition)), BoxGroup("설정"), PropertyOrder(2)] Ease positionMoveMode = Ease.InQuad;

    [SerializeField, ShowIf(nameof(changeFade)), BoxGroup("설정"), MinValue(0f), MaxValue(1f), PropertyOrder(3)] float alphaTarget;
    [SerializeField, ShowIf(nameof(changeFade)), BoxGroup("설정"), PropertyOrder(3)] Ease alphaMoveMode = Ease.InQuad;
    #endregion

    Vector3 originScale;
    Vector3 originPos;
    float originAlpha;
    protected Graphic image;

    private void Awake()
    {
        image = GetComponent<Graphic>();
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
        if (image == null) return;
        float target = image.color.a;

        if (isFrom) image.DOFade(alphaTarget, 0f);
        else target = alphaTarget;

        DOVirtual.DelayedCall(delay, () =>
        {
            image.DOFade(target, duration).SetEase(alphaMoveMode);
        }, ignoreTimescale);
    }

    protected void SetOrigin()
    {
        if (changeScale) originScale = transform.localScale;
        if (changePosition) originPos = transform.position;
        if (image != null && changeFade) originAlpha = image.color.a;
    }

    public void DoOrigin()
    {
        if (changeScale) transform.localScale = originScale;
        if (changePosition) transform.position = originPos;
        if (image != null && changeFade) image.DOFade(originAlpha, 0f);
    }

    public void DoChange()
    {
        if (changePosition) ChangePosition();
        if (changeScale) ChangeScale();
        if (changeFade) ChangeFade();
    }
}
