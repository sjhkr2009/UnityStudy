using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInput
{
	Board board;
	public BoardInput(Board board)
    {
        this.board = board;
    }

    private BoardHelper Helper => board.Helper;
    private BoardBehavior Behavior => board.Behavior;

    private int _pressedIndex = -1;
    public int PressedIndex
    {
        set { _pressedIndex = Mathf.Clamp(value, -1, Helper.CellCount - 1); }
        get
        {
            return (_pressedIndex >= 0 && board.cells[_pressedIndex].IsMovable) ?
              _pressedIndex : -1;
        }
    }
    private int _clickedIndex = -1;
    public int ClickedIndex
    {
        set
        {
            if (_clickedIndex >= 0)
            {
                board.cells[_clickedIndex].UnClick();
                if (_clickedIndex == value)
                    return;
            }

            if ((value >= 0 && value < Helper.CellCount)
                && board.cells[value].TryClick() == true)
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
            return (_clickedIndex >= 0 && board.cells[_clickedIndex].IsMovable) ?
              _clickedIndex : -1;
        }
    }


    public void OnInputAction(Define.InputType inputType, Vector2 inputPoint)
    {
        if (inputType == Define.InputType.None)
            return;

        Vector2 pos = GameManager.Camera.ToWorldPos(inputPoint);
        //Debug.Log($"{pos}({ToIndex(pos)}번)에서 입력 감지됨: {System.Enum.GetName(typeof(Define.InputType), inputType)}");

        if (!Helper.Contain(pos))
        {
            if (inputType == Define.InputType.Drag)
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
                OnPress(Helper.ToIndex(pos));
                break;
            case Define.InputType.Drag:
                OnDrag(Helper.ToIndex(pos));
                break;
            case Define.InputType.Release:
                OnRelease(Helper.ToIndex(pos));
                break;
            default:
                return;
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
        if (Helper.IsNeighbor(PressedIndex, index))
        {
            Debug.Log($"드래그에 의한 Swap 처리");
            RequestSwap(PressedIndex, index);
        }
    }

    private void OnDragOutside(Vector2 pos)
    {
        if (ClickedIndex < 0)
            return;

        Vector2 pressedPos = GameManager.Camera.ToWorldPos(GameManager.Input.LastDownPoint);
        float moveDist = Vector2.Distance(pos, pressedPos);
        if (moveDist <= Helper.CellSize)
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
            targetIdx = (pressedPos.y < pos.y) ?
                (ClickedIndex - Helper.Width) : (ClickedIndex + Helper.Width);
        }
        if (Helper.IsNeighbor(ClickedIndex, targetIdx))
        {
            Debug.Log($"선택된 블록의 외부 드래그에 의한 Swap 처리");
            RequestSwap(ClickedIndex, targetIdx);
        }
    }

    private void OnRelease(int index)
    {
        if (PressedIndex < 0)
            return;

        //Debug.Log($"{index}번 블록에서 Release 되었습니다.");
        if (ClickedIndex >= 0 && Helper.IsNeighbor(ClickedIndex, index))
        {
            Debug.Log($"인접한 두 블록 선택에 의한 Swap 처리");
            RequestSwap(ClickedIndex, index);
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

    private void RequestSwap(int index1, int index2)
    {
        PressedIndex = -1;
        ClickedIndex = -1;

        Cell cell1 = board.cells[index1];
        Cell cell2 = board.cells[index2];
        Behavior.TrySwap(cell1, cell2);
    }
}
