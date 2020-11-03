using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
	public bool IsOn { get; private set; }

	private void OnEnable()
	{
		IsOn = true;
	}
	private void OnDisable()
	{
		IsOn = false;
	}
}
