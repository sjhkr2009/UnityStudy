using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UiPopupBase : UiBase
{
	[SerializeField, ReadOnly] private int myOrder;
	protected virtual void Init()
	{
		myOrder = GameManager.UI.SetPopupCanvas(gameObject);
	}

	protected virtual bool CloseUI(bool byForce = false)
	{
		return GameManager.UI.ClosePopupUI(this, byForce);
	}
}
