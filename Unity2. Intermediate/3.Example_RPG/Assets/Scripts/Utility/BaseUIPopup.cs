using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseUIPopup : BaseUI
{
	[SerializeField, ReadOnly] private int myOrder;
	protected override void Init()
	{
		myOrder = GameManager.UI.SetPopupCanvas(gameObject);
	}

	protected virtual bool CloseUI(bool byForce = false)
	{
		return GameManager.UI.ClosePopupUI(this, byForce);
	}
}
