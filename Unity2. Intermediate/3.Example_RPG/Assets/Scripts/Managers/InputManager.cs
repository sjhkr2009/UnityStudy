using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public event Action KeyAction = null;

    public void OnUpdate()
	{
		if (!Input.anyKey) return;

		KeyAction?.Invoke();
	}
}
