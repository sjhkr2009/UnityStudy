using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class BoardHelper
{
	Board board;
	public BoardHelper(Board board, int height, int width)
	{
		this.board = board;
		Height = height;
		Width = width;
		CellCount = height * width;
        moveDelta = Define.Board.BlockMove.GetDelta(width);
	}

	Cell[] Cells => board.cells;

	public int Height { get; private set; }
	public int Width { get; private set; }
	public int CellCount { get; private set; }

	public int CellIndex(int x, int y) => (Width * y) + x;
	public int GetIndexX(int index) => (index % Width);
	public int GetIndexY(int index) => (index / Width);

	public float CellSize => Data.CellSize;
	public float InitPosX => -(Width * CellSize * 0.5f) + (CellSize / 2f);
	public float InitPosY => (Height * CellSize * 0.5f) - (CellSize / 2f);
	public Vector2 LeftTop => new Vector2(-(Width * CellSize * 0.5f), (Height * CellSize * 0.5f));
	public Vector2 RightBottom => new Vector2(-LeftTop.x, -LeftTop.y);
	public Vector2 GetWorldPos(int x, int y) => new Vector2(InitPosX + (x * CellSize), InitPosY - (y * CellSize));
	public Vector2 GetWorldPos(int index) => GetWorldPos(GetIndexX(index), GetIndexY(index));
	private int[] moveDelta;

	public int ToIndex(Vector2 worldPos)
	{
		Vector2 distToLeftTop = new Vector2((worldPos.x - LeftTop.x), -(worldPos.y - LeftTop.y));

		int x = (int)(distToLeftTop.x / CellSize);
		int y = (int)(distToLeftTop.y / CellSize);
		return CellIndex(x, y);
	}

	public bool Contain(Vector2 pos)
	{
		bool isContain = (pos.x > LeftTop.x) && (pos.x < RightBottom.x) &&
			(pos.y < LeftTop.y) && (pos.y > RightBottom.y);

		return isContain;
	}

	public bool IsNeighbor(int index1, int index2)
	{
		if (index1 < 0 || index1 >= CellCount || index2 < 0 || index2 >= CellCount)
			return false;

		int diff = Mathf.Abs(index1 - index2);
		if (diff == 1 && GetIndexY(index1) == GetIndexY(index2))
		{
			return true;
		}
		else if (diff == Width)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public List<int> FindLinearMatchBlocks(int startIdx, bool isHorizontal)
    {
		List<int> foundList = new List<int>() { startIdx };
		BlockType originType = Cells[startIdx].BlockType;

		int[] deltaIndexes = isHorizontal ? Define.Board.BlockMove.GetHorizontalDir : Define.Board.BlockMove.GetVerticalDir;
        for (int i = 0; i < deltaIndexes.Length; i++)
        {
			int delta = moveDelta[deltaIndexes[i]];
			int now = startIdx;
			int next = now + delta;

            while (IsNeighbor(now, next))
            {
				if(Cells[next].BlockType == originType)
                {
					if (!foundList.Contains(next))
					{
						foundList.Add(next);
					}
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

	//public List<int> FineSquareMatchBlocks
}
