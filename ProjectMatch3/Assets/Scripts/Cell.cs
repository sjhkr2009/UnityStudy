using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

public class Cell
{
    public Cell(int initData, int posX, int posY, int index)
	{
		info = CellGenerator.SetCellInfo(initData, posX, posY);
		Index = index;
		gameObject = CellGenerator.CreateCell(info, out block, out layer, out seal);
	}
	public int Index { get; private set; }
	public GameObject gameObject { get; private set; }
	public Transform transform => gameObject.transform;

	public CellInfo info;
	// TODO: special/layer/seal 타입에 따른 클래스로 변경
	public Block block;
	public GameObject layer;
	public GameObject seal;

	public int X => info.posX;
	public int Y => info.posY;
	public bool IsActive => (block != null) && (info.blockType != BlockType.None);
	public bool IsMovable => block.IsMovable;
	
}
