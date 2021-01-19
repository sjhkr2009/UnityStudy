using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
	public enum SceneType
	{
		Unknown,
		Play
	}

	public enum InputType
    {
		None,
		Press,
		Drag,
		Release
    }

	namespace Default
	{
		public abstract class Name
		{
			public const string GameManager = "GameManager";
		}
	}

	namespace Board
	{
		public abstract class Name
		{
			public const string Root = "Board";

			public const string ObjectLayer = "Layer";
			public const string ObjectBlock = "Block";
			public const string ObjectSeal = "Seal";

			public const string ColorBlue = "blue";
			public const string ColorGreen = "green";
			public const string ColorOrange = "orange";
			public const string ColorPurple = "purple";
			public const string ColorRed = "red";
			public const string ColorYellow = "yellow";
			public const string ColorNone = "other";

			public const string TypeNormal = "normal";
			public const string TypeFish = "fish";
			public const string TypeHorizontalCrush = "horizonCrush";
			public const string TypeVerticalCrush = "verticalCrush";
		}
		public abstract class Path
		{
			public const string BlockSprites = "Sprites/Blocks/";
			public static string ToBlockSpritePath(string name) => $"{BlockSprites}{name}";
		}
		public abstract class Data
		{
			public const float CellSize = 0.5f;
		}
		public struct CellInfo
		{
			public int posX;
			public int posY;

			public BlockType blockType;
			public int specialTypes;
			public int sealTypes;
			public int layerTypes;
		}
		public enum BlockType
		{
			None,
			Blue,
			Green,
			Orange,
			Purple,
			Red,
			Yellow,
			NormalCount,
			Special = 100
		}
		public enum SpecialType
		{
			Normal = 0,
			Immovable = 1,
			HorizontalCrush = 1 << 1,
			VerticalCrush = 1 << 2,
			Fish = 1 << 3
		}
		public enum LayerType
		{
			None = 0
		}
		public enum SealType
		{
			None = 0,
			Immovable = 1
		}
		public enum InitData
		{
			Empty = 0,
			NormalRandom = 1
		}
	}
}