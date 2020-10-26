using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DialogUI : UIBase_Popup
{
    enum Images
    {
        BackgroundImage,
        CharacterImage,
        TableImage,
        NextButton
    }
    enum Texts
    {
        DialogText,
        ButtonText
    }
    enum Buttons
    {
        NextButton
    }

    private int currentIndex = 0;
    private List<string> dialogList = new List<string>();
    Text dialogText;
    [SerializeField] Sprite endButtonImage;

    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        currentIndex = 0;
        dialogList = GameManager.Dialog.GetDialog(GameManager.Data.CurrentCustomer);
        if(dialogList == null)
        {
            Debug.Log($"대사 탐색 실패 : {GameManager.Data.CurrentCustomer.Name}의 레벨 {GameManager.Data.CurrentCustomer.Level - 1}에 해당하는 대사가 없습니다.");
            GameManager.UI.ClosePopupUI<DialogUI>();
            return;
        }

        dialogText = GetText((int)Texts.DialogText);

        SetText(currentIndex);
        GetButton((int)Buttons.NextButton).onClick.AddListener(SetNextText);

        Image charImage = GetImage((int)Images.CharacterImage);
        charImage.sprite = GameManager.Data.CurrentCustomer.Image;
        charImage.SetNativeSize();
    }

    void SetText(int index)
    {
        if (index >= dialogList.Count) return;

        dialogText.DOKill();

        string text = dialogList[index];
        dialogText.text = "";
        dialogText.DOText(text, text.Length * Define.DoTextSpeedFast);

    }
    void SetNextText()
    {
        currentIndex++;
        if (currentIndex == dialogList.Count - 1) NextButtonToCancel();

        if (currentIndex >= dialogList.Count) return;
        SetText(currentIndex);
    }
    void NextButtonToCancel()
    {
        if(endButtonImage == null)
            GetText((int)Texts.ButtonText).text = "X";
        else
            GetImage((int)Images.NextButton).sprite = endButtonImage;

        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
        GetButton((int)Buttons.NextButton).onClick.AddListener(() => 
        {
            dialogText.DOKill();
            GameManager.Instance.GameState = GameState.Idle;
        });
    }
}
