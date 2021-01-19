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

	
}
