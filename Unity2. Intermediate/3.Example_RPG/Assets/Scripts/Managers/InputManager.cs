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
	float _pressedTime = 0f;

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
			if (!_pressed)
			{
				MouseAction(Define.MouseEvent.PointerDown);
				_pressedTime = Time.time;
			}
			MouseAction(Define.MouseEvent.Press);
			_pressed = true;
		}
		else if (_pressed)
		{
			if (Time.time < _pressedTime + Define.DefaultSetting.ClickSensitivity)
			{
				MouseAction(Define.MouseEvent.Click);
			}

			MouseAction(Define.MouseEvent.PointerUp);

			_pressedTime = 0f;
			_pressed = false;
		}
	}

	public void Clear()
	{
		KeyAction = null;
		MouseAction = null;
	}
}
