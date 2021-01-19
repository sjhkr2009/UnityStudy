using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailMaking : UIBase_Popup
{
    void Start() => Init();

    [SerializeField] ShakeCocktail shake;
    public override void Init()
    {
        base.Init();

        if(shake == null) shake = gameObject.FindChild<ShakeCocktail>("MakingImage");

        shake.OnMakingEnd -= OnEnd;
        shake.OnMakingEnd += OnEnd;

    }

    public Action OnEndMaking = () => { };

    void OnEnd()
    {
        shake.OnMakingEnd -= OnEnd;
        OnEndMaking();
        OnEndMaking = null;
        GameManager.UI.ClosePopupUI<CocktailMaking>();
    }
}
