using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIBase_Scene
{
    enum Buttons
    {
        OpenBirdList,
        OpenCockList,
        OpenMusicList,
        MenuOpenButton
    }
    enum Icons
    {
        OpenMusicList,
        OpenCockList,
        OpenBirdList,
        Count
    }
    enum Background
    {
        MenuBarBackground,
        MenuOpenButton
    }

    bool isOpened = false;
    Vector3 pivot;
    Vector2 backgroundSize;
    
    void Start() => Init();
    public override void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject, false, int.MaxValue);

        Bind<Button>(typeof(Buttons));
        Bind<Transform>(typeof(Icons));
        Bind<RectTransform>(typeof(Background));

        pivot = GetButton((int)Buttons.MenuOpenButton).transform.position;
        backgroundSize = Get<RectTransform>((int)Background.MenuBarBackground).sizeDelta;
        GetButton((int)Buttons.MenuOpenButton).onClick.AddListener(() => { MenuOnOff(isOpened); });

        for (int i = 0; i < (int)Icons.Count; i++)
        {
            Get<Transform>(i).gameObject.SetActive(false);
        }

        //UI컬렉션 추가 시 컬렉션 UI를 여는 기능을 각 버튼에 할당할 것
        GetButton((int)Buttons.OpenCockList).onClick.AddListener(() =>
        {
            GameManager.UI.OpenPopupUI<CollectionUI>();
            MenuOnOff(true);
        });

        Get<RectTransform>((int)Background.MenuBarBackground).sizeDelta = Vector2.zero;
    }
    private void OnDestroy()
    {
        ResetButtons();
    }

    void EnableIcon(int index)
    {
        Transform tr = Get<Transform>(index);

        DOVirtual.DelayedCall(Define.OpenMenuDuration * 0.4f, () =>
        {
            if (!tr.gameObject.activeSelf) tr.gameObject.SetActive(true);

            tr.position = pivot;
            tr.DOMove(pivot + new Vector3(0, Define.MenuIconSpacing * (index + 1), 0), Define.OpenMenuDuration * 0.8f);
        });
    }
    void DisableIcon(int index)
    {
        Transform tr = Get<Transform>(index);

        tr.DOKill();
        tr.DOMove(pivot, Define.OpenMenuDuration);
        DOVirtual.DelayedCall(Define.OpenMenuDuration, () => { tr.gameObject.SetActive(false); });
    }
    void MenuOnOff(bool _isOpened)
    {
        Button button = GetButton((int)Buttons.MenuOpenButton);
        button.interactable = false;

        RectTransform buttonTr = Get<RectTransform>((int)Background.MenuOpenButton);
        RectTransform background = Get<RectTransform>((int)Background.MenuBarBackground);

        buttonTr.DOKill();
        background.DOKill();

        if (_isOpened)
        {
            buttonTr.localScale *= 0.8f;
            buttonTr.DOScale(1f, Define.OpenMenuDuration).SetEase(Ease.OutBack);

            background.DOSizeDelta(backgroundSize, Define.OpenMenuDuration * 0.8f).SetEase(Ease.Linear).OnComplete(() =>
            {
                background.DOSizeDelta(Vector2.zero, Define.OpenMenuDuration * 0.4f);
            });

            for (int i = 0; i < (int)Icons.Count; i++)
                DisableIcon(i);

        }
        else
        {
            buttonTr.localScale *= 1.1f;
            buttonTr.DOScale(1f, Define.OpenMenuDuration).SetEase(Ease.OutBack);

            background.DOSizeDelta(backgroundSize, Define.OpenMenuDuration * 0.4f).OnComplete(() =>
            {
                background.DOSizeDelta(new Vector2(backgroundSize.x, (((int)Icons.Count) * Define.MenuIconSpacing * 2) + backgroundSize.y), Define.OpenMenuDuration * 0.8f);
            });

            for (int i = 0; i < (int)Icons.Count; i++)
                EnableIcon(i);
        }
        isOpened = !isOpened;
        DOVirtual.DelayedCall(Define.OpenMenuDuration * 1.3f, () => { button.interactable = true; });
    }
}
