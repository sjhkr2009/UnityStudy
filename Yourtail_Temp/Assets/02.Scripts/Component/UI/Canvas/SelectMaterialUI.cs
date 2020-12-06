using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectMaterialUI : UIBase_Scene
{
    public enum Mode
    {
        SelectBase,
        SelectSub
    }
    enum MaterialIcons
    {
        Tequilla,
        Vodka,
        Whisky,
        Gin,
        Rum,
        Brandy,
        //----------------//
        OrangeJuice,
        LimeJuice,
        LemonJuice,
        GrenadineSyrup,
        OrangeLiqueur,
        SodaWater,
        Amaretto,
        GingerBeer,
        CoffeeLiqueur,
        Vermouth,
        Mint,
        Sugar,
        Campari,
        CherryLiqueur,
        Cola,
        Count
    }
	enum SubSelectedIcons
	{
		SelectedSub1,
		SelectedSub2,
		SelectedSub3,
        Count
	}
	enum BaseSelectedIcons
	{
		SelectedBase1,
        Count
	}
	enum RectTransforms
    {
        MaterialMoveArea,
        Windows,
        PageIcon1,
        PageIcon2,
        PageIcon3
    }
    enum Buttons
    {
        ModeChangeButton,
        BackButton,
        DoButton,
        ResetButton,
        OrderButton,
        WindowNext,
        WindowPrev
    }
    enum WindowObjects
    {
        Window1,
        Window2,
        Window3
    }
    enum Images
	{
        NextImage,
        PrevImage,
        Background
    }

    Mode windowMode = Mode.SelectBase;
    int _currentWindow;
    int CurrentWindow
    {
        get => _currentWindow;
        set
        {
            _currentWindow = value;
            SetDotIcon(value);
            switch (value)
            {
                case 0:
                    GetButton((int)Buttons.WindowPrev).interactable = false;
                    break;
                case 1:
                    GetButton((int)Buttons.WindowPrev).interactable = true;
                    GetButton((int)Buttons.WindowNext).interactable = true;
                    break;
                case 2:
                    GetButton((int)Buttons.WindowNext).interactable = false;
                    break;
                default:
                    Debug.Log("[SelectMaterialUI] 재료 선택창 버튼 할당에 오류가 있습니다. 개발자에게 얘기해 주세요.");
                    break;
            }
        }
    }

    public Mode WindowMode
    {
        get => windowMode;
        set => OnChangeSelectMode(value);
    }

    RectTransform moveArea;
    //List<GameObject> windows = new List<GameObject>();
    List<RectTransform> dotIcons = new List<RectTransform>();

    void Start() => Init();
    public override void Init()
    {
        Init(2);

        Bind<MaterialIcon>(typeof(MaterialIcons));
        Bind<GameObject>(typeof(WindowObjects));
        Bind<Button>(typeof(Buttons));
        Bind<RectTransform>(typeof(RectTransforms));
        Bind<BaseSelectedIcon>(typeof(BaseSelectedIcons));
        Bind<SubSelectedIcon>(typeof(SubSelectedIcons));
        Bind<Image>(typeof(Images));

        moveArea = Get<RectTransform>((int)RectTransforms.MaterialMoveArea);
        moveArea.anchoredPosition = new Vector2(0, 0);

        InitImages();
        InitButtons();
        InitWindows();

        Inited = true;
        GameManager.UI.CloseSceneUI<SelectMaterialUI>();
    }
    private void OnEnable()
    {
        if (!Inited)
            return;

        UIReset();
    }
	private void OnDisable()
	{
        moveArea.DOKill();
    }
	void InitImages()
    {
        for (int i = 0; i < (int)MaterialIcons.Count; i++)
        {
            if(i < Define.SpiritCount)
                Get<MaterialIcon>(i).MyMaterial = GameManager.Game.BaseMaterialList[i];
            else
                Get<MaterialIcon>(i).MyMaterial = GameManager.Game.SubMaterialList[i - Define.SpiritCount];
        }
    }
    void InitButtons()
    {
        GetButton((int)Buttons.ModeChangeButton).onClick.AddListener(() => { WindowMode = Mode.SelectSub; });

        GetButton((int)Buttons.DoButton).onClick.AddListener(() => { GameManager.UI.OpenPopupUI<CheckBeforeShake>(); });
        GetButton((int)Buttons.BackButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.Idle; });
        GetButton((int)Buttons.ResetButton).onClick.AddListener(GameManager.Game.ResetSelected);
        GetButton((int)Buttons.OrderButton).onClick.AddListener(() => { GameManager.UI.OpenPopupUI<OrderInfoWindow>(); });

        GetButton((int)Buttons.WindowNext).onClick.AddListener(() => { MoveWindow(true); });
        GetButton((int)Buttons.WindowPrev).onClick.AddListener(() => { MoveWindow(false); });

        SetNextBtnSprite(true);
    }

    public void MoveWindow(bool toNext)
    {
        if (WindowMode == Mode.SelectBase)
            return;
        if (CurrentWindow <= 0 && !toNext)
            return;
        if (CurrentWindow >= 2 && toNext)
            return;

        RectTransform subRect = Get<RectTransform>((int)RectTransforms.Windows);
        subRect.DOKill();

        if (toNext)
            subRect.DOAnchorPosX(-Define.UIRefSize.x * (++CurrentWindow), 0.2f);
        else
            subRect.DOAnchorPosX(-Define.UIRefSize.x * (--CurrentWindow), 0.2f);
    }

    void OnChangeSelectMode(Mode mode)
    {
        windowMode = mode;
        Image bg = GetImage((int)Images.Background);

        moveArea.DOKill();
        bg.DOKill();

        GetButton((int)Buttons.ModeChangeButton).onClick.RemoveAllListeners();

        switch (mode)
        {
            case Mode.SelectBase:
                moveArea.DOAnchorPos(new Vector2(0, 0), 0.3f);
                GetButton((int)Buttons.ModeChangeButton).onClick.AddListener(() => { WindowMode = Mode.SelectSub; });
                bg.DOColor(Color.white, 0.3f);
                SetNextBtnSprite(true);
                break;

            case Mode.SelectSub:
                moveArea.DOAnchorPos(new Vector2(0, Define.UIRefResolution.y), 0.3f);
                GetButton((int)Buttons.ModeChangeButton).onClick.AddListener(() => { WindowMode = Mode.SelectBase; });
                bg.DOColor(Color.yellow, 0.3f);
                SetNextBtnSprite(false);
                break;

            default:
                Debug.Log("[SelectMaterialUI] 유효하지 않은 모드입니다.");
                break;
        }
    }

    void InitWindows()
    {
        for (int i = (int)RectTransforms.PageIcon1; i <= (int)RectTransforms.PageIcon3; i++)
        {
            dotIcons.Add(Get<RectTransform>(i));
        }
        dotIcons[0].localScale = Vector3.one * Define.SelectedDotIconScale;
    }

    void SetDotIcon(int index)
    {
        for (int i = 0; i < dotIcons.Count; i++)
        {
            dotIcons[i].DOKill();

            if(i == index)
            {
                dotIcons[i].DOScale(Define.SelectedDotIconScale, 0.3f);
            }
            else if(dotIcons[i].localScale.x != 1f)
            {
                dotIcons[i].DOScale(1f, 0.3f);
            }
        }
    }

    void SetNextBtnSprite(bool isNext)
	{
        GetImage((int)Images.NextImage).gameObject.SetActive(isNext);
        GetImage((int)Images.PrevImage).gameObject.SetActive(!isNext);
    }

    void UIReset()
	{
        CurrentWindow = 0;
        moveArea.anchoredPosition = Vector2.zero;
        Get<RectTransform>((int)RectTransforms.Windows).anchoredPosition = Vector2.zero;

        GetImage((int)Images.Background).color = Color.white;

        GetButton((int)Buttons.ModeChangeButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.ModeChangeButton).onClick.AddListener(() => { WindowMode = Mode.SelectSub; });

        SetNextBtnSprite(true);
        windowMode = Mode.SelectBase;

        for (int i = 0; i < (int)BaseSelectedIcons.Count; i++)
            Get<BaseSelectedIcon>(i).ResetIcon();

        for (int i = 0; i < (int)SubSelectedIcons.Count; i++)
            Get<SubSelectedIcon>(i).ResetIcon();
    }
}
