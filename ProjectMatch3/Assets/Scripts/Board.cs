using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class Board
{
    public Board(int height, int width, int[] boardInfo)
	{
		Height = height;
		Width = width;
		CellCount = height * width;

		BoxCollider2D col = Root.GetOrAddComponent<BoxCollider2D>();
		col.size = new Vector2(Data.CellSize * Height, Data.CellSize * Width);
		Behavior.board = this;

		cells = new Cell[CellCount];
		for (int i = 0; i < CellCount; i++)
		{
			cells[i] = new Cell(boardInfo[i], GetIndexPosX(i), GetIndexPosY(i), i);
			cells[i].transform.parent = Root;
			cells[i].transform.position = GetWorldPos(i);
		}
	}

	private Cell[] cells;
	private Transform _root;
	public Transform Root
	{
		get
		{
			if(_root == null)
			{
				GameObject go = GameObject.Find(Name.Root);
				if (go == null)
					go = new GameObject(name: Name.Root);

				_root = go.transform;
			}

			return _root;
		}
	}
	private BoardInput _behavior;
	public BoardInput Behavior
	{
		get
		{
			if (_behavior == null)
				_behavior = Root.GetOrAddComponent<BoardInput>();

			return _behavior;
		}
	}

	public int Height { get; private set; }
	public int Width { get; private set; }
	public int CellCount { get; private set; }

	public int CellIndex(int x, int y) => (Width * y) + x;
	public int GetIndexPosX(int index) => (index % Width);
	public int GetIndexPosY(int index) => (index / Width);

	public float CellSize => Data.CellSize;
	public float InitPosX => -(Width / 2f) * CellSize;
	public float InitPosY => (Height / 2f) * CellSize;
	public Vector2 GetWorldPos(int x, int y) => new Vector2(InitPosX + (x * CellSize), InitPosY - (y * CellSize));
	public Vector2 GetWorldPos(int index) => GetWorldPos(GetIndexPosX(index), GetIndexPosY(index));

	public int ToIndex(Vector2 worldPos)
	{
		Vector2 distToLeftTop = new Vector2(Mathf.Abs(worldPos.x - InitPosX), Mathf.Abs(worldPos.y - InitPosY));
		int x = (int)(distToLeftTop.x / CellSize);
		int y = (int)(distToLeftTop.y / CellSize);
		return CellIndex(x, y);
	}

}
