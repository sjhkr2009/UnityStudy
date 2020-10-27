using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

	string _name;
	
	void Start() => Init();

	protected override void Init()
	{
		Bind<Image>(typeof(Images));
		Bind<Text>(typeof(Texts));

		GetText((int)Texts.itemName).text = _name;
		GetImage((int)Images.itemImage).gameObject.BindEvent((PointerEventData evt) => 
			{
				Debug.Log($"{_name} 아이템 클릭!");
			});
	}

	public UI_Inventory_Item SetInfo(string name)
	{
		_name = name;
		return this;
	}
}
