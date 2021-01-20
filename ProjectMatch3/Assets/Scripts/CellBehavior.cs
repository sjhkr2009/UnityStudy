using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// Cell에 대한 이동 및 파괴 등의 동작을 BoardBehavior로부터 받아서 수행합니다.
/// Cell의 구성 요소를 확인하여 현재 상태에 맞는 동작을 명령합니다.
/// </summary>
public class CellBehavior : MonoBehaviour
{
    public Cell Cell { get; private set; }
	public void SetCell(Cell cell)
	{
		Cell = cell;
	}

	/// <summary>
	/// Block이 파괴된 경우 호출되는 이벤트
	/// </summary>
	public event Action OnDestroyBlock = null;

	/// <summary>
	/// 파괴 명령을 받아서 현재 Cell의 상태에 따라 처리합니다. 블록 파괴가 가능하면 파괴합니다.
	/// </summary>
	public IEnumerator Crush(CrushType crushType)
	{
		// TODO: Crush 명령을 받았을 때의 처리 (파괴 가능 조건 체크)

		yield return Cell.Block.StartCoroutine(nameof(Cell.Block.Crush));
		OnDestroyBlock?.Invoke();
	}

	/// <summary>
	/// 이 Cell에 연결된 블록 오브젝트를 Cell의 위치로 이동시킵니다.
	/// </summary>
	public IEnumerator BlockMoveToOrigin()
	{
		yield return Cell.Block.StartCoroutine(nameof(Cell.Block.MoveOrigin));
	}

	/// <summary>
	/// 입력된 블록을 이 Cell에 연결시키고, 원래 연결되어 있던 블록을 반환합니다.
	/// </summary>
	public Block InterchangeBlock(Block target)
	{
		Block prev = Cell.Block;
		Cell.Block = target;
		return prev;
	}
}
