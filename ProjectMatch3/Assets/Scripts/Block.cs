using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class Block : MonoBehaviour
{
	private Cell _cell;
	public Cell Cell
	{
		get => _cell;
		set
		{
			_cell = value;
			transform.parent = _cell.transform;
		}
	}

	public bool IsMovable => (Cell.info.sealTypes & (int)SealType.Immovable) == 0 || 
		(Cell.info.specialTypes & (int)SpecialType.Immovable) == 0;

	public void OnClick()
    {
		// TODO: 클릭에 따른 시각 효과 출력
		transform.localScale = Vector3.one * 1.2f; 
    }
	public void UnClick()
    {
		// TODO: 클릭에 따른 시각 효과 해제
		transform.localScale = Vector3.one;
	}

}
