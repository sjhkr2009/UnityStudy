using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// Board의 정보 저장 및 변환, 특정 요소 탐색 등을 수행합니다.
/// </summary>
public class BoardHelper
{
	Board Board;
	public BoardHelper(Board board, int height, int width)
	{
		this.Board = board;
		Height = height;
		Width = width;
		CellCount = height * width;
        moveDelta = BlockMove.GetDelta(width);
	}
		
	private Cell[] Cells => Board.Cells;

	public int Height { get; private set; }
	public int Width { get; private set; }
	public int CellCount { get; private set; }

	/// <summary>
	/// 입력된 x,y 좌표를 Cell 배열의 인덱스 좌표로 변환하여 반환합니다.
	/// </summary>
	public int CellIndex(int x, int y) => (Width * y) + x;
	/// <summary>
	/// 입력받은 인덱스의 X 좌표를 반환합니다. (좌측이 0입니다)
	/// </summary>
	public int GetIndexX(int index) => (index % Width);
	/// <summary>
	/// 입력받은 인덱스의 Y 좌표를 반환합니다. (상단이 0입니다)
	/// </summary>
	public int GetIndexY(int index) => (index / Width);

	public float CellSize => Data.CellSize;
	/// <summary>
	/// 처음 생성된 블록의 월드 기준 X 좌표값
	/// </summary>
	public float InitPosX => -(Width * CellSize * 0.5f) + (CellSize / 2f);
	/// <summary>
	/// 처음 생성된 블록의 월드 기준 Y 좌표값
	/// </summary>
	public float InitPosY => (Height * CellSize * 0.5f) - (CellSize / 2f);
	/// <summary>
	/// Board의 월드 기준 좌측 상단 끝 지점 (LeftTop ~ RightBottom 사이는 Board 영역으로 간주됩니다)
	/// </summary>
	public Vector2 LeftTop => new Vector2(-(Width * CellSize * 0.5f), (Height * CellSize * 0.5f));
	/// <summary>
	/// Board의 월드 기준 우측 하단 끝 지점 (LeftTop ~ RightBottom 사이는 Board 영역으로 간주됩니다)
	/// </summary>
	public Vector2 RightBottom => new Vector2(-LeftTop.x, -LeftTop.y);
	/// <summary>
	/// 입력한 인덱스의 월드 기준 좌표
	/// </summary>
	public Vector2 GetWorldPos(int x, int y) => new Vector2(InitPosX + (x * CellSize), InitPosY - (y * CellSize));
	/// <summary>
	/// 입력한 인덱스의 월드 기준 좌표
	/// </summary>
	public Vector2 GetWorldPos(int index) => GetWorldPos(GetIndexX(index), GetIndexY(index));
	/// <summary>
	/// 이동 시 변경되는 인덱스 좌표의 변화량. BlockMove.Direction을 통해 호출합니다.
	/// </summary>
	private int[] moveDelta;

	/// <summary>
	/// 입력된 월드 좌표에 해당하는 Cell의 인덱스를 반환합니다.
	/// </summary>
	public int ToIndex(Vector2 worldPos)
	{
		Vector2 distToLeftTop = new Vector2((worldPos.x - LeftTop.x), -(worldPos.y - LeftTop.y));

		int x = (int)(distToLeftTop.x / CellSize);
		int y = (int)(distToLeftTop.y / CellSize);
		return CellIndex(x, y);
	}

	/// <summary>
	/// 입력된 월드 좌표가 Board 위에 있으면 true를 반환합니다.
	/// </summary>
	public bool Contain(Vector2 pos)
	{
		bool isContain = (pos.x > LeftTop.x) && (pos.x < RightBottom.x) &&
			(pos.y < LeftTop.y) && (pos.y > RightBottom.y);

		return isContain;
	}

	/// <summary>
	/// 입력된 두 좌표가 1칸 이내로 인접해 있으면 true를 반환합니다. 유효하지 않은 좌표가 있다면 false를 반환합니다.
	/// </summary>
	/// <param name="includeDiagonal">true일 경우 대각선으로 인접한 좌표일 경우에도 true를 반환합니다.</param>
	public bool IsNeighbor(int index1, int index2, bool includeDiagonal = false)
	{
		if (index1 < 0 || index1 >= CellCount || index2 < 0 || index2 >= CellCount)
			return false;

		int diff = Mathf.Abs(index1 - index2);
		if (diff == 1 && GetIndexY(index1) == GetIndexY(index2))
		{
			return true;
		}
		if (diff == Width)
		{
			return true;
		}
		if(includeDiagonal &&
			Mathf.Abs(GetIndexX(index1) - GetIndexX(index2)) == 1 &&
			Mathf.Abs(GetIndexY(index1) - GetIndexY(index2)) == 1)
		{
			return true;
		}

		return false;
	}

	/// <summary>
	/// 입력된 인덱스로부터 가로 또는 세로 방향에 일렬로 이어져 있는 동일 타입 블럭들의 리스트를 반환합니다.
	/// </summary>
	/// <param name="startIdx">탐색 시작 지점</param>
	/// <param name="isHorizontal">탐색 방향. true면 가로로, false면 세로로 탐색합니다.</param>
	/// <returns>탐색 시작 지점을 포함한 동일 타입의 인덱스 리스트가 반환됩니다. 매치된 개수는 리스트의 요소 개수로 확인할 수 있습니다.</returns>
	public List<int> FindLinearBlocks(int startIdx, bool isHorizontal)
    {
		List<int> foundList = new List<int>() { startIdx };
		BlockType originType = Cells[startIdx].BlockType;

		int[] deltaIndexes = isHorizontal ? BlockMove.GetHorizontalDir : BlockMove.GetVerticalDir;
        for (int i = 0; i < deltaIndexes.Length; i++)
        {
			int delta = moveDelta[deltaIndexes[i]];
			int now = startIdx;
			int next = now + delta;

            while (IsNeighbor(now, next))
            {
				if(Cells[next].BlockType == originType)
                {
					foundList.AddUnique(next);
					now = next;
					next += delta;
                }
                else
                {
					break;
                }
            }
        }

		return foundList;
    }

	/// <summary>
	/// 입력된 인덱스에서 가로 또는 세로로 3개(BlockMove.LinearMatchCount에 정의된 개수) 이상 매치된 모든 블럭들을 찾아 인덱스 리스트를 반환합니다.
	/// </summary>
	/// <param name="maxMatchCount">일렬로 가장 많이 매치된 블럭의 개수를 반환합니다.</param>
	/// <returns>인덱스는 중복되지 않으며 입력된 인덱스를 포함합니다. 단, 가로 세로 모두 매치되지 않은 경우 빈 리스트가 반환됩니다.</returns>
	public List<int> FindLinearMatchBlocks(int index, out int maxMatchCount)
	{
		List<int> foundList = new List<int>();

		List<int> horizontal = FindLinearBlocks(index, true);
		if(horizontal.Count >= Data.LinearMatchCount)
		{
			foundList.OverlapFrom(horizontal);
		}

		List<int> vertical = FindLinearBlocks(index, false);
		if(vertical.Count >= Data.LinearMatchCount)
		{
			foundList.OverlapFrom(vertical);
		}

		maxMatchCount = Mathf.Max(horizontal.Count, vertical.Count);

		return foundList;
	}

	/// <summary>
	/// 입력한 지점에서 4방향으로 동일한 타입의 2x2 블럭이 생성되는지 확인합니다.
	/// (2x2 형태에 추가로 이어진 동일 타입 블럭은 FindLinearMatchBlocks에서 3칸 이상 매치 여부로 확인할 수 있습니다)
	/// </summary>
	/// <param name="originIdx">탐색을 시작할 지점</param>
	/// <returns>2x2 블럭이 만들어진다면 해당 블럭들의 인덱스 리스트가 반환됩니다. 만들 수 없다면 빈 리스트가 반환됩니다.</returns>
	public List<int> FindSquareMatchBlocks(int originIdx)
	{
		List<int> foundList = new List<int>();
		BlockType originType = Cells[originIdx].BlockType;

		foreach (int[] checkList in BlockMove.GetSquareDirections)
		{
			List<int> matched = new List<int>();
			for (int i = 0; i < checkList.Length; i++)
			{
				int targetIdx = originIdx + moveDelta[checkList[i]];
				if(IsNeighbor(originIdx, targetIdx, true) && Cells[targetIdx].BlockType == originType)
				{
					matched.Add(targetIdx);
				}
				else
				{
					break;
				}
			}
			
			if(matched.Count == 3)
			{
				matched.Add(originIdx);
				foundList.OverlapFrom(matched);
			}
		}

		return foundList;
	}

	/// <summary>
	/// 입력된 인덱스와 같은 X축상에서 위쪽에 있는 가까운 블럭을 찾아 반환합니다. 없을 경우 null을 반환합니다.
	/// 찾은 블럭이 위치하던 Cell에 연결된 Block은 삭제됩니다.
	/// </summary>
	public Block PopAboveBlockTo(int index)
	{
		int now = index - Width;
		while (true)
		{
			if(now < 0)
			{
				return null;
			}
			
			if (Cells[now].IsActive)
			{
				Block block = Cells[now].Block;
				Cells[now].Block = null;
				Cells[index].Block = block;
				return block;
			}

			now -= Width;
		}
	}

	/// <summary>
	/// 직선 또는 사각형으로 매치된 모든 블럭을 찾아 해당 인덱스들의 리스트를 반환합니다. 리스트의 요소는 중복되지 않습니다.
	/// </summary>
	/// <param name="cell">탐색을 시작할 지점의 Cell</param>
	/// <param name="matchType">MatchType 열거형으로 정의된 매치 유형이 연산 및 저장될 변수</param>
	public List<int> CheckMatchedBlock(Cell cell, ref int matchType)
	{
		List<int> matched = new List<int>();

		List<int> linearMatched = FindLinearMatchBlocks(cell.Index, out int matchCount);
		if (linearMatched.Count > 0)
		{
			matchType |= (int)MatchType.Match3;
			if (matchCount > Data.LinearMatchCount)
			{
				matchType |= (int)MatchType.Match4OrMore;
			}
		}
		List<int> squareMatch = FindSquareMatchBlocks(cell.Index);
		if (squareMatch.Count > 0)
		{
			matchType |= (int)MatchType.SquareMatch;
		}

		matched.OverlapFrom(linearMatched);
		matched.OverlapFrom(squareMatch);

		return matched;
	}

	/// <summary>
	/// 해당 인덱스와 같은 가로축에 있는 모든 인덱스의 리스트를 반환합니다.
	/// </summary>
	public List<int> FindHorizontalLine(int index)
	{
		List<int> foundList = new List<int>() { index };
		int[] deltaIndexes = BlockMove.GetHorizontalDir;

		for (int i = 0; i < deltaIndexes.Length; i++)
		{
			int delta = moveDelta[deltaIndexes[i]];
			int now = index;
			int next = now + delta;

			while(GetIndexY(now) == GetIndexY(next))
			{
				foundList.Add(next);
				next += delta;
			}
		}

		return foundList;
	}

	/// <summary>
	/// 해당 인덱스와 같은 세로축에 있는 모든 인덱스의 리스트를 반환합니다.
	/// </summary>
	public List<int> FindVerticalLine(int index)
	{
		List<int> foundList = new List<int>() { index };
		int[] deltaIndexes = BlockMove.GetVerticalDir;

		for (int i = 0; i < deltaIndexes.Length; i++)
		{
			int delta = moveDelta[deltaIndexes[i]];
			int now = index;
			int next = now + delta;

			while (next >= 0 && next < CellCount)
			{
				foundList.Add(next);
				next += delta;
			}
		}

		return foundList;
	}

	/// <summary>
	/// 해당 인덱스에서 3-Match 또는 2x2 사각형으로 매치가 가능할 경우 true가 반환됩니다.
	/// </summary>
	public bool IsMatchedFrom(int index)
	{
		bool matched = FindLinearBlocks(index, true).Count >= Data.LinearMatchCount ||
				FindLinearBlocks(index, false).Count >= Data.LinearMatchCount ||
				FindSquareMatchBlocks(index).Count > Data.LinearMatchCount;

		return matched;
	}

	/// <summary>
	/// 시작 시 처음부터 매치되는 블럭이 나오는 경우를 방지합니다.
	/// TODO: 상하좌우 타입과 다른 블럭 중에서만 랜덤한 타입으로 교체 시 더 효율적
	/// </summary>
	public void ProtectMatchOnInit()
	{
		for (int i = 0; i < CellCount; i++)
		{
			int count = 0;
			while (IsMatchedFrom(i))
			{
				Cells[i].Block.SetType(CellGenerator.GetRandomBlockType(), Cells[i].SpecialType);
				if(count++ > 20)
				{
					Debug.LogWarning($"{i}번 블럭이 매칭 블럭으로 20회 이상 변경을 시도했습니다.");
					break;
				}
			}
		}
	}
}
