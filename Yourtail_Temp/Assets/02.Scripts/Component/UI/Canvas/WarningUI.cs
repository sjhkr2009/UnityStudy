using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningUI : UIBase_Popup
{
    enum Buttons
    {
        YesButton,
        NoButton,
        blockRaycast
    }

    enum Texts
    {
        WarningText
    }

    enum Tweens
    {
        blockRaycast,
        background
    }

    Button yes;
    Button no;
    Text warningText;


    void Start() => Init();
    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<TweenBase>(typeof(Tweens));

        SetDefault();
        SetButtons();

        GameManager.Instance.ignoreOnMouse = true;

        SetPooling();
    }
    private void OnEnable()
    {
        if (!Inited)
            return;

        if (yes == null || no == null || warningText == null)
            SetDefault();

        SetButtons();
        gameObject.SetCanvasOrder();
        GameManager.Instance.ignoreOnMouse = true;
    }

    private void OnDisable()
    {
        yes.onClick.RemoveAllListeners();
        GameManager.Instance.ignoreOnMouse = false;
    }

    void SetDefault()
    {
        yes = GetButton((int)Buttons.YesButton);
        no = GetButton((int)Buttons.NoButton);
        warningText = GetText((int)Texts.WarningText);

        no.onClick.AddListener(() => { GameManager.UI.ClosePopupUI<WarningUI>(); });
        GetButton((int)Buttons.blockRaycast).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<WarningUI>(); });
    }

    void SetButtons()
    {
        Define.WarningType type = GameManager.UI.CurrentWarningType;

        switch (type)
        {
            case Define.WarningType.QuitApp:
                warningText.text = "게임을 종료하시겠습니까?";
                yes.onClick.AddListener(GameManager.Instance.QuitApp);
                break;
            default:
                warningText.text = "";
                yes.onClick.AddListener(() => { GameManager.UI.ClosePopupUI<WarningUI>(); });
                break;
        }
    }

}
