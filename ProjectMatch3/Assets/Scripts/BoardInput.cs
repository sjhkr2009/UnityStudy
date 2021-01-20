using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Board의 입력을 감지하여, 입력에 맞는 동작을 실행합니다.
/// </summary>
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
    /// <summary>
    /// 마우스/터치 Down이 입력된 Cell의 인덱스를 저장합니다. 없을 경우 -1의 값을 갖습니다.
    /// </summary>
    public int PressedIndex
    {
        set { _pressedIndex = Mathf.Clamp(value, -1, Helper.CellCount - 1); }
        get
        {
            return (_pressedIndex >= 0 && board.Cells[_pressedIndex].IsMovable) ?
              _pressedIndex : -1;
        }
    }
    private int _clickedIndex = -1;
    /// <summary>
    /// 클릭된 Cell의 인덱스를 저장합니다. 없을 경우 -1의 값을 갖습니다.
    /// </summary>
    public int ClickedIndex
    {
        set
        {
            if (_clickedIndex >= 0)
            {
                board.Cells[_clickedIndex].UnClick();
                if (_clickedIndex == value)
                    return;
            }

            if ((value >= 0 && value < Helper.CellCount)
                && board.Cells[value].TryClick() == true)
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
            return (_clickedIndex >= 0 && board.Cells[_clickedIndex].IsMovable) ?
              _clickedIndex : -1;
        }
    }

    /// <summary>
    /// InputManager에서 입력을 받으면 호출되는 콜백 함수입니다.
    /// </summary>
    /// <param name="inputType">입력 유형</param>
    /// <param name="inputPoint">입력된 지점의 스크린 좌표</param>
    public void OnInputAction(Define.InputType inputType, Vector2 inputPoint)
    {
        if (inputType == Define.InputType.None)
            return;

        Vector2 pos = GameManager.Camera.ToWorldPos(inputPoint);

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
        PressedIndex = index;
    }

    private void OnDrag(int index)
    {
        if (PressedIndex < 0 || PressedIndex == index)
            return;

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
            RequestSwap(ClickedIndex, targetIdx);
        }
    }

    private void OnRelease(int index)
    {
        if (PressedIndex < 0)
            return;

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

    /// <summary>
    /// 스왑 동작에 맞는 입력을 받을 경우 Behavior에 Swap을 요청합니다.
    /// </summary>
    private void RequestSwap(int index1, int index2)
    {
        PressedIndex = -1;
        ClickedIndex = -1;

        Cell cell1 = board.Cells[index1];
        Cell cell2 = board.Cells[index2];
        Behavior.TrySwap(cell1, cell2);
    }
}
