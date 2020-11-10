using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour
{
	public int PoolCount = -1;
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
