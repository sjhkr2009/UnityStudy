using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
	public enum Scene
	{
		Unknown,
		Login,
		Lobby,
		Game
	}
	
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

	public abstract class DefaultSetting
	{
		public const int UiOrder = 10;
	}
	public abstract class ResourcesPath
	{
		public const string Prefab = "Prefabs/";

		public const string EventSystem = "UI/EventSystem";
		public const string PopupUi = "UI/Popup/";
		public const string SceneUi = "UI/Scene/";
		public const string SubItemUi = "UI/SubItem/";
		
	}
	public abstract class DefaultName
	{
		public const string GameManager = "@GameManager";
		public const string EventSystem = "@EventSystem";
		public const string Scene = "@Scene";
		public const string UiRoot = "@UI";
	}
}
