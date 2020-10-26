using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirdCollectionUI : UIBase_Popup
{
    enum BirdInfoCards
    {
        BirdInfoCard1,
        BirdInfoCard2,
        BirdInfoCard3,
        BirdInfoCard4,
        BirdInfoCard5,
        Count
    }
    
    enum Buttons
    {
        CloseButton
    }
    
    void Start() => Init();
    public override void Init()
    {
        base.Init();

        GameManager.Instance.ignoreOnMouse = true;

        Bind<BirdInfoCard>(typeof(BirdInfoCards));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.CloseButton).onClick.AddListener(() => { GameManager.UI.ClosePopupUI<BirdCollectionUI>(); });

        SetCards();
    }

    void SetCards()
    {
        for (int i = 0; i < (int)BirdInfoCards.Count; i++)
        {
            Get<BirdInfoCard>(i).SetInfo(GameManager.Data.CustomerList[i]);
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.ignoreOnMouse = true;
    }
    private void OnDisable()
    {
        GameManager.Instance.ignoreOnMouse = false;
    }
    private void OnDestroy()
    {
        ResetButtons();
    }
}
