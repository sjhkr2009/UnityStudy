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
	
	public enum Sound
	{
		Bgm,
		Effect,
		Count
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
		public const int PoolCount = 20;
	}
	public abstract class ResourcesPath
	{
		public const string EventSystem = "UI/EventSystem";

		public const string Data = "Data/";

		public static string ToPrefab(string path) => $"{Prefab}{path}";
		public const string Prefab = "Prefabs/";

		public static string ToPopupUI(string path) => $"{PopupUi}{path}";
		public static string ToSceneUI(string path) => $"{SceneUi}{path}";
		public static string ToSubItemUI(string path) => $"{SubItemUi}{path}";
		public const string PopupUi = "UI/Popup/";
		public const string SceneUi = "UI/Scene/";
		public const string SubItemUi = "UI/SubItem/";

		public static string ToAudio(string path) => $"{AudioClip}{path}";
		public const string AudioClip = "Sounds/";
		
	}
	public abstract class DefaultName
	{
		public const string GameManager = "@GameManager";
		public const string EventSystem = "@EventSystem";
		public const string SceneManager = "@Scene";
		public const string UiRoot = "@UI";
		public const string SoundRoot = "@Sound";
		public const string PoolRoot = "@Pool_Root";

		public static string ToPoolable(string origin) => $"Root: {origin}";
	}
}
