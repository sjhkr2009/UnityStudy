using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Item : UiBase
{
    enum Texts
	{
		itemName
	}

	enum Images
	{
		itemImage
	}
	
	void Start() => Init();

	protected override void Init()
	{
		Bind<Image>(typeof(Images));
		Bind<Text>(typeof(Texts));

		GetText((int)Texts.itemName).text = "집판검";
	}
}
