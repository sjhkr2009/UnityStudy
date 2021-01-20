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
		public abstract class BlockMove
		{
			public enum Direction
			{
				Up,
				Right,
				Down,
				Left,
				UpRight,
				DownRight,
				UpLeft,
				DownLeft,
				Count
			}
			public static int[] GetDelta(int boardWidth)
				=> new int[(int)Direction.Count] { -boardWidth, 1, boardWidth, -1, -boardWidth + 1, boardWidth + 1, -boardWidth - 1, boardWidth - 1 };

			public static int[] GetHorizontalDir => new int[2] { (int)Direction.Right, (int)Direction.Left };
			public static int[] GetVerticalDir => new int[2] { (int)Direction.Up, (int)Direction.Down };
			public static int[] GetUpRightSquareDir => new int[3] { (int)Direction.Up, (int)Direction.Right, (int)Direction.UpRight };
			public static int[] GetUpLeftSquareDir => new int[3] { (int)Direction.Up, (int)Direction.Left, (int)Direction.UpLeft };
			public static int[] GetDownRightSquareDir => new int[3] { (int)Direction.Down, (int)Direction.Right, (int)Direction.DownRight };
			public static int[] GetDownLeftSquareDir => new int[3] { (int)Direction.Down, (int)Direction.Left, (int)Direction.DownLeft };
			public static List<int[]> GetSquareDirections => new List<int[]> { GetUpRightSquareDir, GetUpLeftSquareDir, GetDownRightSquareDir, GetDownLeftSquareDir };

			public const int LinearMatchCount = 3;
			public const float LerpMoveSpeed = 0.2f;
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
		public enum CrushType
        {
			Common,
			ByMatchLinear3,
			ByMatchLinearMore4,
			ByMatchSquare,
			ByFish,
			ByLinearClear
		}
	}
}