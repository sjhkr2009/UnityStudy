using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public event Action KeyAction = null;
	public event Action<Define.MouseEvent> MouseAction = null;

	bool _pressed = false;

    public void OnUpdate()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (Input.anyKey)
			KeyAction?.Invoke();

		if (MouseAction != null)
			CheckMouseAction();

	}

	void CheckMouseAction()
	{
		if (Input.GetMouseButton(0))
		{
			MouseAction(Define.MouseEvent.Press);
			_pressed = true;
		}
		else if (_pressed)
		{
			MouseAction(Define.MouseEvent.Click);
			_pressed = false;
		}
	}

	public void Clear()
	{
		KeyAction = null;
		MouseAction = null;
	}
}
