using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicCollectionUI : UIBase_Popup
{
    enum Buttons
	{
		Space,
		CloseButton
	}
	
	
	void Start() => Init();
	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));

		SetButtons();
	}

	void SetButtons()
	{
		GetButton((int)Buttons.Space).onClick.AddListener(() => GameManager.UI.ClosePopupUI<MusicCollectionUI>());
		GetButton((int)Buttons.CloseButton).onClick.AddListener(() => GameManager.UI.ClosePopupUI<MusicCollectionUI>());
	}
}
