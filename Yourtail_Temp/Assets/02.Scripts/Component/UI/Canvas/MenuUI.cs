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
        MenuOpenButton,
        Panel
    }
    enum Icons
    {
        OpenMusicList,
        OpenCockList,
        OpenBirdList,
        Count
    }
    enum RectTransforms
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
        GameManager.UI.SetCanvasOrder(gameObject, false, 9999);

        Bind<Button>(typeof(Buttons));
        Bind<Transform>(typeof(Icons));
        Bind<RectTransform>(typeof(RectTransforms));

        pivot = GetButton((int)Buttons.MenuOpenButton).transform.position;
        backgroundSize = Get<RectTransform>((int)RectTransforms.MenuBarBackground).sizeDelta;

        SetButtons();

        GetButton((int)Buttons.Panel).gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        ResetButtons();
    }

    void SetButtons()
    {
        GetButton((int)Buttons.MenuOpenButton).onClick.AddListener(() => { MenuOnOff(isOpened); });

        GetButton((int)Buttons.Panel).onClick.AddListener(() =>
        {
            MenuOnOff(isOpened);
        });

        //UI컬렉션 추가 시 컬렉션 UI를 여는 기능을 각 버튼에 할당할 것
        GetButton((int)Buttons.OpenCockList).onClick.AddListener(() =>
        {
            GameManager.UI.OpenPopupUI<CollectionUI>();
            MenuOnOff(true);
        });
        GetButton((int)Buttons.OpenBirdList).onClick.AddListener(() =>
        {
            GameManager.UI.OpenPopupUI<BirdCollectionUI>();
            MenuOnOff(true);
        });

        DisableIcons();
    }

    void EnableIcons()
    {
        for (int i = 0; i < (int)Icons.Count; i++)
            Get<Transform>(i).gameObject.SetActive(true);
    }
    void DisableIcons()
    {
        for (int i = 0; i < (int)Icons.Count; i++)
            Get<Transform>(i).gameObject.SetActive(false);
    }
    void MenuOnOff(bool _isOpened)
    {
        Button button = GetButton((int)Buttons.MenuOpenButton);
        button.interactable = false;

        RectTransform buttonTr = Get<RectTransform>((int)RectTransforms.MenuOpenButton);
        RectTransform background = Get<RectTransform>((int)RectTransforms.MenuBarBackground);

        buttonTr.DOKill();
        background.DOKill();

        GetButton((int)Buttons.Panel).gameObject.SetActive(!_isOpened);

        if (_isOpened)
        {
            buttonTr.localScale = Vector3.one * 0.8f;
            buttonTr.DOScale(1f, Define.OpenMenuDuration).SetEase(Ease.OutBack);

            DisableIcons();
            background.DOSizeDelta(new Vector2(backgroundSize.x, 0f), Define.OpenMenuDuration);

        }
        else
        {
            buttonTr.localScale = Vector3.one * 1.1f;
            buttonTr.DOScale(1f, Define.OpenMenuDuration).SetEase(Ease.OutBack);

            background.DOSizeDelta(new Vector2(backgroundSize.x, ((int)Icons.Count + 1) * Define.MenuIconSpacing), Define.OpenMenuDuration)
                .OnComplete(EnableIcons);
        }

        isOpened = !isOpened;
        DOVirtual.DelayedCall(Define.OpenMenuDuration, () => { button.interactable = true; });
    }
}
