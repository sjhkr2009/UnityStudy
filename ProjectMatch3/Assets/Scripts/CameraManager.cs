using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
	private Camera _main;
	public Camera Main
	{
		get
		{
			if (_main == null)
				_main = Camera.main;

			return _main;
		}
	}

	public Vector2 ToWorldPos(Vector2 pos)
    {
		return Main.ScreenToWorldPoint(pos);
    }
}
