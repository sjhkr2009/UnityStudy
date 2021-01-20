using System.Text;
using System.Collections.Generic;

/// <summary>
/// 범용으로 사용되는 열거형 자료 및 상수를 저장합니다.
/// 하드코딩된 요소는 이곳에서 수정할 수 있습니다.
/// </summary>
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
		/// <summary>
		/// 기본값으로 사용되는 게임오브젝트의 이름
		/// </summary>
		public abstract class Name
		{
			public const string GameManager = "GameManager";
		}
	}

	namespace Board
	{
		/// <summary>
		/// 보드판에 사용되는 오브젝트 이름 및 Resource 로딩에 사용되는 이름
		/// </summary>
		public abstract class Name
		{
			public const string Root = "Board";
			public const string Cell = "Cell";

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
		/// <summary>
		/// 블록 관련 리소스 로딩 시 사용되는 기본 경로
		/// </summary>
		public abstract class Path
		{
			public const string BlockSprites = "Sprites/Blocks/";
			public static string ToBlockSpritePath(string name) => $"{BlockSprites}{name}";
		}
		/// <summary>
		/// 보드판 내 요소들에 사용되는 기본값
		/// </summary>
		public abstract class Data
		{
			/// <summary>
			/// 블록 하나의 world 좌표 기준 크기
			/// </summary>
			public const float CellSize = 0.5f;
			/// <summary>
			/// 블럭이 해당 개수 이상 연달아 있을 경우 파괴됩니다.
			/// </summary>
			public const int LinearMatchCount = 3;
		}
		/// <summary>
		/// 블록 이동에 관련된 요소. 이동 방향에 따른 인덱스 변화값과, 탐색 방식에 따라 탐색할 인덱스 배열 등을 저장합니다.
		/// </summary>
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

			public const float LerpMoveSpeed = 0.2f;
		}
		/// <summary>
		/// 각 Cell을 생성하기 위한 정보를 담은 구조체
		/// </summary>
		public struct CellInfo
		{
			public int posX;
			public int posY;

			public BlockType blockType;
			public int specialTypes;
			public int sealTypes;
			public int layerTypes;
		}
		/// <summary>
		/// 매칭의 기준이 되는 각 블록의 타입을 나타냅니다. 3매치가 가능한 유형의 블록은 NormalCount 이하의 값을 갖습니다.
		/// Special(또는 NormalCount 이상의 값)일 경우 매칭되지 않으며, None이면 블록이 없는 것으로 간주합니다.
		/// </summary>
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
		/// <summary>
		/// 블록의 특수한 속성을 나타냅니다. 비트 플래그 형식으로 하나 이상의 값을 가질 수 있습니다.
		/// 3매치가 불가능한 특수 블록일 경우 BlockType은 Special로 고정됩니다.
		/// </summary>
		public enum SpecialType
		{
			Normal = 0,
			Immovable = 1,
			HorizontalCrush = 1 << 1,
			VerticalCrush = 1 << 2,
			Fish = 1 << 3
		}
		/// <summary>
		/// 블록 뒷배경에 배치되는 레이어 유형을 나타냅니다. 플래그 형식으로 하나 이상의 값을 가질 수 있습니다.
		/// </summary>
		public enum LayerType
		{
			None = 0
		}
		/// <summary>
		/// 블록 위쪽에 추가된 장치를 나타냅니다. 플래그 형식으로 하나 이상의 값을 가질 수 있습니다.
		/// Immovable 플래그가 켜져 있다면 해당 블럭은 이동할 수 없습니다.
		/// </summary>
		public enum SealType
		{
			None = 0,
			Immovable = 1
		}
		/// <summary>
		/// 보드판 생성 시 StageData에 정의된 초기값에 따라 어떤 블럭을 생성할 지 나타냅니다.
		/// </summary>
		public enum InitData
		{
			Empty = 0,
			NormalRandom = 1
		}
		/// <summary>
		/// 파괴 유형을 나타냅니다. 블록 파괴 방식에 따라 다른 처리가 필요할 때 사용됩니다.
		/// (현재는 사용되지 않습니다)
		/// </summary>
		public enum CrushType
        {
			NoCrush,
			ByMatch,
			ByFish,
			ByLineClear
		}
		/// <summary>
		/// 블록 매치 유형을 나타냅니다. 매치 방식에 따른 특수 블록 생성 등, 추가 처리가 필요할 때 사용됩니다.
		/// </summary>
		public enum MatchType
		{
			NoMatch = 0,
			Match3 = 1,
			Match4OrMore = Match3 | (1 << 1),
			SquareMatch = 1 << 2
		}
	}
}