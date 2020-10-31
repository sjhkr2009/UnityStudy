using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIPopup : BaseUI
{
	private int myOrder;
	protected override void Init()
	{
		myOrder = GameManager.UI.SetPopupCanvas(gameObject);
	}

	protected virtual bool CloseUI(bool byForce = false)
	{
		return GameManager.UI.ClosePopupUI(this, byForce);
	}
}
