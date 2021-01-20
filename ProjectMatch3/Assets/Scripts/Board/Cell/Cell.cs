using System;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// Board의 한 칸을 이루는 공간에 해당합니다. 하위 오브젝트인 Layer, Block, Seal로 구성됩니다.
/// </summary>
public class Cell
{
    public Cell(int initData, int posX, int posY, int index)
	{
		CellInfo info = CellGenerator.SetCellInfo(initData, posX, posY);

		GameObject gameObject = CellGenerator.CreateCell(info, out _block, out layer, out seal);
		Behavior = gameObject.GetOrAddComponent<CellBehavior>();
		Behavior.SetCell(this);
		Behavior.OnDestroyBlock += OnDestroyBlock;

		Index = index;
		X = posX;
		Y = posY;
	}
	public int Index { get; private set; }
	public CellBehavior Behavior { get; private set; }
	public GameObject gameObject => Behavior.gameObject;
	public Transform transform => Behavior.transform;

	// TODO: special/layer/seal 타입에 따른 클래스로 변경
	private Block _block;
	/// <summary>
	/// 이 Cell에 연결된 블록을 세팅합니다. 해당 블록을 이 셀의 자식 Transform으로 위치시킵니다.
	/// </summary>
	public Block Block
	{
		get => _block;
		set
		{
			_block = value;
			if(value != null)
			{
				_block.transform.parent = transform;
			}
		}
	}
	public GameObject layer;
	public Seal seal;

	/// <summary>
	/// 블록 파괴 시 이 Cell의 인덱스와 연결되어 있던 Block을 외부에 전달하기 위한 이벤트입니다.
	/// </summary>
	public Action<Block, int> OnDestroy = null;

	/// <summary>
	/// 연결된 블록의 타입을 반환합니다.
	/// </summary>
	public BlockType BlockType
	{
		get
		{
			if(Block == null)
			{
				Debug.Log($"{Index}번 셀에 블록이 없습니다");
				return 0;
			}
			return Block.Type;
		}
	}
	public int SpecialType => (Block != null) ? Block.SpecialTypes : 0;

	public int X { get; private set; }
	public int Y { get; private set; }

	/// <summary>
	/// 이 Cell에 연결된 블록이 없거나 None 타입인 경우 false를 반환합니다.
	/// </summary>
	public bool IsActive => (Block != null) && (Block.Type != BlockType.None);
	/// <summary>
	/// 이 Cell의 블록이 없거나 이동할 수 없는 타입인 경우, 추가 장치로 인해 이동 불가한 상태인 경우 false를 반환합니다.
	/// </summary>
	public bool IsMovable => IsActive && Block.IsMovable && !seal.IsSealed;
	
	/// <summary>
	/// 블럭에 클릭 동작을 명령합니다. 클릭이 불가능할 경우 false를 반환합니다.
	/// </summary>
	public bool TryClick()
    {
		if (!IsMovable)
			return false;

		Block.OnClick();

		return true;
    }
	public void UnClick()
    {
		Block.UnClick();
    }

	public void OnDestroyBlock()
	{
		OnDestroy?.Invoke(Block, Index);
		Block = null;
	}
}
