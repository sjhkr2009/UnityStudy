using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UiSceneBase : UiBase
{
	[SerializeField] private int myOrder = 0;
	protected virtual void Init()
	{
		myOrder = GameManager.UI.SetSceneCanvas(gameObject, myOrder);
	}
}
