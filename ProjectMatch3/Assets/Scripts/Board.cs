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

		GameManager.Input.InputAction += OnInputAction;

		cells = new Cell[CellCount];
		for (int i = 0; i < CellCount; i++)
		{
			cells[i] = new Cell(boardInfo[i], GetIndexX(i), GetIndexY(i), i);
			cells[i].transform.parent = Root;
			cells[i].transform.position = GetWorldPos(i);
		}

		Debug.Log($"좌상단 {LeftTop} / 우하단 {RightBottom}");
	}

	~Board()
    {
		GameManager.Input.InputAction -= OnInputAction;
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

	private int _pressedIndex = -1;
	private int PressedIndex
	{
		set { _pressedIndex = Mathf.Clamp(value, -1, CellCount - 1); }
		get
		{
			return (_pressedIndex >= 0 && cells[_pressedIndex].IsMovable) ?
			  _pressedIndex : -1;
		}
	}
	private int _clickedIndex = -1;
	private int ClickedIndex
	{
        set
        {
			if(_clickedIndex >= 0)
            {
				cells[_clickedIndex].UnClick();
				if (_clickedIndex == value)
					return;
            }
			
			if((value >= 0 && value < CellCount) 
				&& cells[value].TryClick() == true)
            {
				_clickedIndex = value;
            }
            else
            {
				_clickedIndex = -1;
            }
        }
		get
		{
			return (_clickedIndex >= 0 && cells[_clickedIndex].IsMovable) ?
			  _clickedIndex : -1;
		}
	}

	public int ToIndex(Vector2 worldPos)
	{
		Vector2 distToLeftTop = new Vector2((worldPos.x - LeftTop.x), -(worldPos.y - LeftTop.y));
		
		int x = (int)(distToLeftTop.x / CellSize);
		int y = (int)(distToLeftTop.y / CellSize);
		return CellIndex(x, y);
	}

	private bool Contain(Vector2 pos)
    {
		bool isContain = (pos.x > LeftTop.x) && (pos.x < RightBottom.x) &&
			(pos.y < LeftTop.y) && (pos.y > RightBottom.y);

		return isContain;
    }

	private void OnInputAction(Define.InputType inputType, Vector2 inputPoint)
    {
		if (inputType == Define.InputType.None)
			return;

		Vector2 pos = GameManager.Camera.ToWorldPos(inputPoint);
		//Debug.Log($"{pos}({ToIndex(pos)}번)에서 입력 감지됨: {System.Enum.GetName(typeof(Define.InputType), inputType)}");

		if (!Contain(pos))
        {
			if(inputType == Define.InputType.Drag)
            {
				OnDragOutside(pos);
			}
			else if (inputType == Define.InputType.Release)
            {
				OnReleaseOutside();
            }
			return;
        }

        switch (inputType)
        {
            case Define.InputType.Press:
				OnPress(ToIndex(pos));
				break;
            case Define.InputType.Drag:
				OnDrag(ToIndex(pos));
				break;
            case Define.InputType.Release:
				OnRelease(ToIndex(pos));
				break;
            default:
                return;
        }
    }

	private bool IsNeighbor(int index1, int index2)
    {
		if (index1 < 0 || index1 >= CellCount || index2 < 0 || index2 >= CellCount)
			return false;

		int diff = Mathf.Abs(index1 - index2);
		if(diff == 1 && GetIndexY(index1) == GetIndexY(index2))
        {
			return true;
        }
		else if(diff == Width)
        {
			return true;
        }
        else
        {
			return false;
        }
    }

	private void OnPress(int index)
    {
		//Debug.Log($"{index}번 블록이 Press 되었습니다.");
		PressedIndex = index;
	}

	private void OnDrag(int index)
    {
		if (PressedIndex < 0 || PressedIndex == index)
			return;

		//Debug.Log($"{PressedIndex}번 블록이 Press 되었으며, 현재는 {index}번 위에 있습니다.");
		if (IsNeighbor(PressedIndex, index))
        {
			Debug.Log($"드래그에 의한 Swap 처리");
			Swap(PressedIndex, index);
        }
    }

	private void OnDragOutside(Vector2 pos)
    {
		if (ClickedIndex < 0)
			return;

		Vector2 pressedPos = GameManager.Camera.ToWorldPos(GameManager.Input.LastDownPoint);
		float moveDist = Vector2.Distance(pos, pressedPos);
		if(moveDist <= CellSize)
        {
			return;
        }

		int targetIdx;
		if (Mathf.Abs(pressedPos.x - pos.x) > Mathf.Abs(pressedPos.y - pos.y))
        {
			targetIdx = (pressedPos.x < pos.x) ? ClickedIndex + 1 : ClickedIndex - 1;
        }
        else
        {
			targetIdx = (pressedPos.y < pos.y) ? ClickedIndex - Width : ClickedIndex + Width;
		}
		if (IsNeighbor(ClickedIndex, targetIdx))
		{
			Debug.Log($"선택된 블록의 외부 드래그에 의한 Swap 처리");
			Swap(ClickedIndex, targetIdx);
		}
	}

	private void OnRelease(int index)
    {
		if (PressedIndex < 0)
			return;

		//Debug.Log($"{index}번 블록에서 Release 되었습니다.");
		if (ClickedIndex >= 0 && IsNeighbor(ClickedIndex, index))
        {
			Debug.Log($"인접한 두 블록 선택에 의한 Swap 처리");
			Swap(ClickedIndex, index);
		}
        else
        {
			ClickedIndex = (ClickedIndex == index) ? -1 : index;
		}
	}

	private void OnReleaseOutside()
    {
		PressedIndex = -1;
		ClickedIndex = -1;
	}

	private void Swap(int index1, int index2)
    {
		PressedIndex = -1;
		ClickedIndex = -1;

		Debug.Log($"{index1}번 블록과 {index2}번 블록을 교환합니다.");
    }
}
