using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Define.Board;

public class CellGenerator
{
    public static CellInfo SetCellInfo(int initData, int posX, int posY)
	{
		CellInfo info = new CellInfo()
		{
			blockType = BlockType.None,
			layerTypes = 0,
			sealTypes = 0,
			specialTypes = 0,
			posX = posX,
			posY = posY
		};

		switch (initData)
		{
			case (int)InitData.NormalRandom:
				info.blockType = GetRandomBlockType();
				break;
			default:
				break;
		}

		return info;
	}
	static BlockType GetRandomBlockType()
	{
		int type = Random.Range(1, (int)BlockType.NormalCount);
		return (BlockType)type;
	}

	public static GameObject CreateCell(CellInfo info, out Block block, out GameObject layer, out GameObject seal)
	{
		GameObject cellObject = GameManager.Resource.Instantiate("Cell");

		Transform childBlock = cellObject.transform.Find(Name.ObjectBlock);
		if (childBlock == null)
		{
			childBlock = new GameObject(Name.ObjectBlock).transform;
			childBlock.parent = cellObject.transform;
		}
		block = childBlock.GetOrAddComponent<Block>();

		layer = cellObject.transform.Find(Name.ObjectLayer).gameObject;
		seal = cellObject.transform.Find(Name.ObjectSeal).gameObject;

		AddBlockType(block, info);
		AddLayerType(layer, info);
		AddSealType(seal, info);

		return cellObject;
	}

	static void AddBlockType(Block block, CellInfo info)
	{
		StringBuilder name = new StringBuilder();
		switch (info.blockType)
		{
			case BlockType.Blue:
				name.Append(Name.ColorBlue);
				break;
			case BlockType.Green:
				name.Append(Name.ColorGreen);
				break;
			case BlockType.Orange:
				name.Append(Name.ColorOrange);
				break;
			case BlockType.Purple:
				name.Append(Name.ColorPurple);
				break;
			case BlockType.Red:
				name.Append(Name.ColorRed);
				break;
			case BlockType.Yellow:
				name.Append(Name.ColorYellow);
				break;
			default:
				name.Append(Name.ColorNone);
				break;
		}
		name.Append('_');
		AddSpecialBlockType(info, name);

		var sr = block.gameObject.GetOrAddComponent<SpriteRenderer>();
		Sprite sprite = GameManager.Resource.Load(Path.ToBlockSpritePath(name.ToString()));
		sr.sprite = sprite;
	}
	
	static void AddSpecialBlockType(CellInfo info, StringBuilder name)
	{
		int typeMask = info.specialTypes;
		if (typeMask == 0)
		{
			name.Append(Name.TypeNormal);
			return;
		}

		if ((typeMask & (int)SpecialType.Fish) > 0)
		{
			name.Append(Name.TypeFish);
		}
		if ((typeMask & (int)SpecialType.HorizontalCrush) > 0)
		{
			name.Append(Name.TypeHorizontalCrush);
		}
		if ((typeMask & (int)SpecialType.VerticalCrush) > 0)
		{
			name.Append(Name.TypeVerticalCrush);
		}
	}

	static void AddLayerType(GameObject layer, CellInfo info)
	{
		int layerMask = info.layerTypes;
		if (layerMask == 0)
			return;
	}

	static void AddSealType(GameObject seal, CellInfo info)
	{
		int sealMask = info.sealTypes;
		if (sealMask == 0)
			return;
	}
}
