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
        MaterialMoveArea
    }
    enum Buttons
    {
        PrevButton,
        NextButton,
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
        ArrowImage,
        ShakerImage
    }

    Mode windowMode = Mode.SelectBase;
    int _currentWindow;
    int CurrentWindow
    {
        get => _currentWindow;
        set
        {
            _currentWindow = value;
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
        set
        {
            windowMode = value;
            moveArea.DOKill();
            GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
            switch (value)
            {
                case Mode.SelectBase:
                    moveArea.DOAnchorPos(new Vector2(screenWidth, 0), 0.3f);
                    GetButton((int)Buttons.PrevButton).interactable = false;
                    GetButton((int)Buttons.NextButton).onClick.AddListener(() => { WindowMode = Mode.SelectSub; });
                    SetNextBtnSprite(true);
                    break;

                case Mode.SelectSub:
                    moveArea.DOAnchorPos(new Vector2(0, 0), 0.3f);
                    GetButton((int)Buttons.PrevButton).interactable = true;
                    GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.UI.OpenPopupUI<CheckBeforeShake>(); });
                    SetNextBtnSprite(false);
                    break;

                default:
                    Debug.Log("[SelectMaterialUI] 유효하지 않은 모드입니다.");
                    break;
            }
        }
    }

    RectTransform moveArea;
    List<GameObject> windows = new List<GameObject>();
    float screenWidth;

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

        screenWidth = Define.UIRefResolution.x;
        moveArea = Get<RectTransform>((int)RectTransforms.MaterialMoveArea);
        moveArea.anchoredPosition = new Vector2(screenWidth, 0);

        SetImages();
        SetButtons();
        SetWindows();

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
	void SetImages()
    {
        for (int i = 0; i < (int)MaterialIcons.Count; i++)
        {
            if(i < Define.SpiritCount)
                Get<MaterialIcon>(i).MyMaterial = GameManager.Data.BaseMaterialList[i];
            else
                Get<MaterialIcon>(i).MyMaterial = GameManager.Data.SubMaterialList[i - Define.SpiritCount];
        }
    }
    void SetButtons()
    {
        GetButton((int)Buttons.PrevButton).onClick.AddListener(() => { WindowMode = Mode.SelectBase; });
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { WindowMode = Mode.SelectSub; });
        GetButton((int)Buttons.PrevButton).interactable = false;

        GetButton((int)Buttons.ResetButton).onClick.AddListener(GameManager.Data.ResetSelected);
        GetButton((int)Buttons.OrderButton).onClick.AddListener(() => { GameManager.UI.OpenPopupUI<OrderInfoWindow>(); });

        GetButton((int)Buttons.WindowNext).onClick.AddListener(() => { MoveWindow(true); });
        GetButton((int)Buttons.WindowPrev).onClick.AddListener(() => { MoveWindow(false); });

        SetNextBtnSprite(true);
    }

    void MoveWindow(bool toNext)
    {
        if (toNext)
            windows[CurrentWindow].SwipeWindow(windows[CurrentWindow + 1], true);
        else
            windows[CurrentWindow].SwipeWindow(windows[CurrentWindow - 1], false);

        CurrentWindow = toNext ? CurrentWindow + 1 : CurrentWindow - 1;
    }

    void SetWindows()
    {
        for (int i = 0; i < 3; i++)
		{
            windows.Add(Get<GameObject>(i));
            windows[i].SetActive(false);
        }
        windows.OpenWindow(0);
    }

    void SetNextBtnSprite(bool isArrow)
	{
        GetImage((int)Images.ArrowImage).gameObject.SetActive(isArrow);
        GetImage((int)Images.ShakerImage).gameObject.SetActive(!isArrow);
    }

    void UIReset()
	{
        windows.OpenWindow(0);
        moveArea.anchoredPosition = new Vector2(screenWidth, 0);

        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { WindowMode = Mode.SelectSub; });
        GetButton((int)Buttons.PrevButton).interactable = false;
        SetNextBtnSprite(true);

        for (int i = 0; i < (int)BaseSelectedIcons.Count; i++)
            Get<BaseSelectedIcon>(i).ResetIcon();

        for (int i = 0; i < (int)SubSelectedIcons.Count; i++)
            Get<SubSelectedIcon>(i).ResetIcon();
    }
}
