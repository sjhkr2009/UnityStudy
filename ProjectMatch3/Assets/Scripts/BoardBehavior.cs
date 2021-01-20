using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// 블록의 이동과 파괴 등 Board 위에서 발생하는 동작들을 처리합니다.
/// </summary>
public class BoardBehavior : MonoBehaviour
{
    public Board Board { get; private set; }
    public void SetBoard(Board board)
	{
        Board = board;
	}
    public Cell[] Cells => Board.Cells;
    private BoardHelper Helper => Board.Helper;

    /// <summary>
    /// 두 Cell의 자리바꿈을 시도합니다.
    /// TODO: 특수 블럭 2개를 합치는 등 인접한 두 블록을 교체하는 Swap 동작이 아닌 경우, NormalSwap이 아닌 다른 동작으로 연결되어야 합니다.
    /// </summary>
    /// <returns>블록의 이동이 불가능할 경우 false를 반환합니다.</returns>
    public bool TrySwap(Cell c1, Cell c2)
    {
        if (!c1.IsMovable || !c2.IsMovable)
            return false;

        StartCoroutine(NormalSwap(c1, c2));
        return true;
    }

    /// <summary>
    /// 두 블록의 자리를 바꾸고, 매치된 블럭이 있으면 파괴합니다. 없으면 원래 자리로 돌아갑니다.
    /// </summary>
    IEnumerator NormalSwap(Cell c1, Cell c2)
    {
        ChangeBlock(c1, c2);

        GameManager.Input.cannotInput = true;

        int matchType1 = 0;
        int matchType2 = 0;
        List<int> matched = new List<int>();

        matched.OverlapFrom(Helper.CheckMatchedBlock(c1, ref matchType1));
        matched.OverlapFrom(Helper.CheckMatchedBlock(c2, ref matchType2));

        c1.Behavior.StartCoroutine(c1.Behavior.BlockMoveToOrigin());
        yield return c2.Behavior.StartCoroutine(c2.Behavior.BlockMoveToOrigin());

        if(matched.Count > 0)
		{
            BlockType prevType1 = c1.Block.Type;
            BlockType prevType2 = c2.Block.Type;

            yield return StartCoroutine(CrushBlocks(matched, CrushType.ByMatch));

            SetSpecialBlock(matchType1, c1, prevType1);
            SetSpecialBlock(matchType2, c2, prevType2);

            yield return StartCoroutine(nameof(FillBlocks));
        }
        else
		{
            ChangeBlock(c1, c2);

            c1.Behavior.StartCoroutine(c1.Behavior.BlockMoveToOrigin());
            yield return c2.Behavior.StartCoroutine(c2.Behavior.BlockMoveToOrigin());
        }

        GameManager.Input.cannotInput = false;
    }

    /// <summary>
    /// 두 Cell에 할당된 블록을 변경합니다. 블록은 할당된 Cell의 하위 Transform으로 이동하지만 위치는 변하지 않습니다.
    /// </summary>
    public void ChangeBlock(Cell c1, Cell c2)
	{
        Block temp = c1.Behavior.InterchangeBlock(c2.Block);
        c2.Behavior.InterchangeBlock(temp);
    }

    /// <summary>
    /// 입력된 인덱스에 해당하는 Cell에 파괴 동작을 실행합니다.
    /// </summary>
    public IEnumerator CrushBlocks(List<int> crushList, CrushType crushType)
	{
        for (int i = 0; i < crushList.Count; i++)
        {
            CellBehavior behavior = Cells[crushList[i]].Behavior;
            if (i == crushList.Count - 1)
            {
                yield return behavior.StartCoroutine(behavior.Crush(crushType));
                break;
            }
            behavior.StartCoroutine(behavior.Crush(crushType));
        }
    }

    /// <summary>
    /// 매치된 타입에 따라 해당 자리에 같은 색상의 특수 블록을 생성합니다.
    /// </summary>
    void SetSpecialBlock(int matchType, Cell cell, BlockType blockType)
	{
        if ((matchType & (int)MatchType.Match4OrMore) == (int)MatchType.Match4OrMore)
        {
            SpecialType type = (Random.value < 0.5f) ? SpecialType.HorizontalCrush : SpecialType.VerticalCrush;
            Block block = Board.DestroyedBlocks.Dequeue();
            cell.Block = block;
            block.transform.localPosition = Vector2.zero;
            cell.Block.SetType(blockType, (int)type);
        }
        else if ((matchType & (int)MatchType.SquareMatch) == (int)MatchType.SquareMatch)
        {
            Block block = Board.DestroyedBlocks.Dequeue();
            cell.Block = block;
            block.transform.localPosition = Vector2.zero;
            cell.Block.SetType(blockType, (int)SpecialType.Fish);
        }
    }

    /// <summary>
    /// 빈 자리에 블록을 채웁니다. 같은 X축의 위쪽 블록으로 채우며, 없을 경우 파괴된 블록을 채웁니다.
    /// </summary>
    public IEnumerator FillBlocks()
	{
        List<int> filled = new List<int>();

        for (int x = 0; x < Helper.Width; ++x)
        {
            for (int y = Helper.Height - 1; y >= 0; --y)
            {
                int idx = Helper.CellIndex(x, y);

                Cell now = Cells[idx];
                if (now.IsActive)
                    continue;

                Block block = Helper.PopAboveBlockTo(idx);
                if (block == null)
                    continue;

                now.Block = block;
                filled.Add(idx);
            }
        }

        for (int x = 0; x < Helper.Width; ++x)
        {
            for (int y = Helper.Height - 1; y >= 0; --y)
            {
                int idx = Helper.CellIndex(x, y);
                if (!Cells[idx].IsActive)
				{
                    Block block = Board.DestroyedBlocks.Dequeue();
                    block.SetType(CellGenerator.GetRandomBlockType(), 0);
                    Cells[idx].Block = block;
                    block.transform.localPosition = new Vector2(0f, 5f);
                    filled.Add(idx);
                }
            }
        }

        for (int i = 0; i < filled.Count; i++)
		{
            CellBehavior behavior = Cells[filled[i]].Behavior;
            if (i == filled.Count - 1)
            {
                yield return behavior.StartCoroutine(behavior.BlockMoveToOrigin());
                break;
            }
            behavior.StartCoroutine(behavior.BlockMoveToOrigin());
        }
	}
}
