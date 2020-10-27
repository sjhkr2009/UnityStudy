using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
	public enum UiEvent
	{
		Click,
		Drag
	}

	public enum CameraMode
	{
		QuarterView
	}

	public enum MouseEvent
	{
		Press,
		Click
	}

	abstract class DefaultSetting
	{
		public const int UiOrder = 10;
	}
	abstract class ResourcesPath
	{
		public const string PopupUi = "UI/Popup/";
		public const string SceneUi = "UI/Scene/";
		public const string Prefab = "Prefabs/";
	}
	abstract class DefaultName
	{
		public const string GameManager = "@GameManager";
		public const string UiRoot = "@UI";
	}
}
