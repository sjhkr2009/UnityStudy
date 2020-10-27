using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Inventory : UiSceneBase
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
			GameObject item = GameManager.Resource.Instantiate("UI/Scene/UI_InvenItem", bg);
		}
	}
}
