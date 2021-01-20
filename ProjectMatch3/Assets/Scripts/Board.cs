using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class Board
{
    public Board(int height, int width, int[] boardInfo)
	{
		Behavior = Root.GetOrAddComponent<BoardBehavior>();
		Behavior.board = this;
		Helper = new BoardHelper(this, height, width);
		Input = new BoardInput(this);

		GameManager.Input.InputAction += Input.OnInputAction;

		cells = new Cell[Helper.CellCount];
		for (int i = 0; i < Helper.CellCount; i++)
		{
			cells[i] = new Cell(boardInfo[i], Helper.GetIndexX(i), Helper.GetIndexY(i), i);
			cells[i].transform.parent = Root;
			cells[i].transform.position = Helper.GetWorldPos(i);
		}
	}

	~Board()
    {
		GameManager.Input.InputAction -= Input.OnInputAction;
	}

	public BoardBehavior Behavior { get; private set; }
	public BoardHelper Helper { get; private set; }
	public BoardInput Input { get; private set; }
	public Cell[] cells { get; private set; }

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

}
