﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : BaseUIScene
{
	enum Transforms
	{
		Background
	}
    
	void Start() => Init();

	protected override void Init()
	{
		base.Init();

		Bind<RectTransform>(typeof(Transforms));

		RectTransform bg = GetRectTransform((int)Transforms.Background);
		foreach (Transform child in bg)
		{
			GameManager.Resource.Destroy(child.gameObject);
		}

		for (int i = 0; i < 8; i++)
		{
			GameManager.UI.MakeSubItem<UI_Inventory_Item>(bg)
				.SetInfo($"집판검 {i}번");
		}
	}
}
