using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class BoardBehavior : MonoBehaviour
{
    public Board board;
    public Cell[] Cells => board.cells;
    private BoardHelper Helper => board.Helper;

    public bool TrySwap(Cell c1, Cell c2)
    {
        if (!c1.IsMovable || !c2.IsMovable)
            return false;

        if (c1.BlockType < BlockType.NormalCount || c2.BlockType < BlockType.NormalCount)
        {
            StartCoroutine(NormalSwap(c1, c2));
            return true;
        }

        return true;
    }

    IEnumerator NormalSwap(Cell c1, Cell c2)
    {
        Block block1 = c1.InterchangeBlock(c2.block);
        Block block2 = c2.InterchangeBlock(block1);

        GameManager.Input.cannotInput = true;

        List<int> horizontalMatch1 = Helper.FindLinearMatchBlocks(c1.Index, true);
        List<int> horizontalMatch2 = Helper.FindLinearMatchBlocks(c2.Index, true);
        List<int> verticalMatch1 = Helper.FindLinearMatchBlocks(c1.Index, false);
        List<int> verticalMatch2 = Helper.FindLinearMatchBlocks(c2.Index, false);

        Debug.Log($"{c1.Index}번 블록과 {c2.Index}번 블록을 교환합니다.");
        Debug.Log($"{c1.Index}번 매치: 가로 {horizontalMatch1.Count}개, 세로 {verticalMatch1.Count}개");
        Debug.Log($"{c2.Index}번 매치: 가로 {horizontalMatch2.Count}개, 세로 {verticalMatch2.Count}개");

        block1.StartCoroutine(nameof(block1.MoveOrigin));
        yield return block2.StartCoroutine(nameof(block2.MoveOrigin));

        GameManager.Input.cannotInput = false;


    }


}
