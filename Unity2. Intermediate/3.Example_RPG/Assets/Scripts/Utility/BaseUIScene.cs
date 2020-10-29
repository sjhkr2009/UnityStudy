using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class BaseUIScene : BaseUI
{
	[SerializeField] private int myOrder = 0;
	protected override void Init()
	{
		myOrder = GameManager.UI.SetSceneCanvas(gameObject, myOrder);
	}
}
