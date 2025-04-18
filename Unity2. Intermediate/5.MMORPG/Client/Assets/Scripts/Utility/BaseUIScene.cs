﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIScene : BaseUI
{
	[SerializeField] private int myOrder = 0;
	protected override void Init()
	{
		myOrder = Director.UI.SetSceneCanvas(gameObject, myOrder);
	}
}
