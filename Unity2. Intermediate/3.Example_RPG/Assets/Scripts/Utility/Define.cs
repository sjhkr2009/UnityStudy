using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
	public enum ObjectType
	{
		Unknown,
		Player,
		Monster
	}

	public enum Layer
	{
		IgnoreRaycast = 8,
		Wall = 9,
		Ground = 10,
		Block = 11,
		Monster = 12
	}
	
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
		PointerDown,
		PointerUp,
		Click
	}

	public enum CreatureState
	{
		Die,
		Idle,
		Moving,
		Skill
	}

	public abstract class DefaultSetting
	{
		public static int UiOrder => 10;
		public static int PoolCount => 10;
		public static float ClickSensitivity => 0.33f;
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
		public static string ToWorldSpaceUI(string path) => $"{WorldSpaceUI}{path}";
		public const string PopupUi = "UI/Popup/";
		public const string SceneUi = "UI/Scene/";
		public const string SubItemUi = "UI/SubItem/";
		public const string WorldSpaceUI = "UI/WorldSpace/";

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
