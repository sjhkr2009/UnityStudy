using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class SetCocktailUI : UIBase_Popup
{
    enum Buttons
    {
        NextButton
    }

    enum Texts
    {
        GradeText
    }


    void Start() => Init();

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        GetButton((int)Buttons.NextButton).onClick.AddListener(() => { GameManager.Instance.GameState = GameState.Idle; });
        GetText((int)Texts.GradeText).text = GradeToText();
    }
    private void OnDestroy()
    {
        GetButton((int)Buttons.NextButton).onClick.RemoveAllListeners();
    }
    string GradeToText()
    {
        DataManager.Grade grade = GameManager.Data.CurrentGrade;

        switch (grade)
        {
            case DataManager.Grade.BAD:
                return "BAD";
            case DataManager.Grade.SOSO:
                return "Soso";
            case DataManager.Grade.GOOD:
                return "GOOD!";
            default:
                return "Error";
        }
    }
}
