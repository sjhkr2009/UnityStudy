using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class Cell
{
    public Cell(int initData, int posX, int posY, int index)
	{
		info = CellGenerator.SetCellInfo(initData, posX, posY);
		gameObject = CellGenerator.CreateCell(info, out block, out layer, out seal);

		Index = index;
		block.Cell = this;
	}
	public int Index { get; private set; }
	public GameObject gameObject { get; private set; }
	public Transform transform => gameObject.transform;

	public CellInfo info;
	// TODO: special/layer/seal 타입에 따른 클래스로 변경
	public Block block;
	public GameObject layer;
	public GameObject seal;

	public BlockType BlockType => block.Type;
	public int SpecialType => block.SpecialTypes;
	// TODO: layer/seal 클래스의 타입을 반환하게 변경
	public int LayerType => info.layerTypes;
	public int SealType => info.sealTypes;

	public int X => info.posX;
	public int Y => info.posY;
	public bool IsActive => (block != null) && (info.blockType != BlockType.None);
	public bool IsMovable => IsActive && block.IsMovable;

	public Block InterchangeBlock(Block target)
    {
		Block prev = block;
		block = target;
		target.transform.parent = this.transform;
		return prev;
    }

	public void Crush(CrushType crushType)
    {
		// TODO: Crush 명령을 받았을 때의 처리 - Seal/Layer 체크
    }
	
	public bool TryClick()
    {
		if (!IsMovable)
			return false;

		block.OnClick();

		return true;
    }
	public void UnClick()
    {
		block.UnClick();
    }
}
