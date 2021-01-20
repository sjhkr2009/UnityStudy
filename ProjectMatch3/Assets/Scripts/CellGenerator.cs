using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Define.Board;

/// <summary>
/// Cell과 그 구성 요소를 생성하는 전역 함수로 구성된 클래스
/// </summary>
public class CellGenerator
{
    /// <summary>
	/// 초기 데이터와 위치값을 입력받아 Cell 클래스의 생성을 위한 CellInfo 구조체를 만들어 반환합니다.
	/// </summary>
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
	/// <summary>
	/// 특수 타입을 제외한 블럭 타입을 랜덤하게 생성하여 반환합니다.
	/// </summary>
	public static BlockType GetRandomBlockType()
	{
		int type = Random.Range(1, (int)BlockType.NormalCount);
		return (BlockType)type;
	}
	/// <summary>
	/// Cell의 생성자에서 호출됩니다. CellInfo를 통해 Cell 오브젝트를 생성하고, Cell의 구성요소인 Layer, Block, Seal을 초기화하여 반환합니다.
	/// </summary>
	public static GameObject CreateCell(CellInfo info, out Block block, out GameObject layer, out Seal seal)
	{
		GameObject cellObject = GameManager.Resource.Instantiate(Name.Cell);

		Transform childBlock = cellObject.transform.Find(Name.ObjectBlock);
		if (childBlock == null)
		{
			childBlock = new GameObject(Name.ObjectBlock).transform;
			childBlock.parent = cellObject.transform;
		}
		block = childBlock.GetOrAddComponent<Block>();

		// TODO: Layer 클래스 생성 및 초기화 처리
		layer = cellObject.transform.Find(Name.ObjectLayer).gameObject;

		childBlock = cellObject.transform.Find(Name.ObjectSeal);
		if(childBlock == null)
		{
			childBlock = new GameObject(Name.ObjectBlock).transform;
			childBlock.parent = cellObject.transform;
		}
		seal = childBlock.GetOrAddComponent<Seal>();

		AddBlockType(block, info);
		AddLayerType(layer, info);
		AddSealType(seal, info);

		return cellObject;
	}

	/// <summary>
	/// 블록의 이미지를 세팅합니다.
	/// </summary>
	static void AddBlockType(Block block, CellInfo info)
	{
		block.gameObject.GetOrAddComponent<SpriteRenderer>();
		block.SetType(info.blockType, info.specialTypes);
	}

	/// <summary>
	/// 블록의 타입 및 특수 타입을 입력받아, 해당하는 Sprite 이미지를 로딩하여 반환합니다.
	/// </summary>
	public static Sprite LoadBlockImage(BlockType blockType, int specialTypes)
    {
		StringBuilder name = new StringBuilder();
		switch (blockType)
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

		if (specialTypes == 0)
		{
			name.Append(Name.TypeNormal);
		}
		if ((specialTypes & (int)SpecialType.Fish) > 0)
		{
			name.Append(Name.TypeFish);
		}
		if ((specialTypes & (int)SpecialType.HorizontalCrush) > 0)
		{
			name.Append(Name.TypeHorizontalCrush);
		}
		if ((specialTypes & (int)SpecialType.VerticalCrush) > 0)
		{
			name.Append(Name.TypeVerticalCrush);
		}

		Sprite sprite = GameManager.Resource.Load(Path.ToBlockSpritePath(name.ToString()));
		return sprite;
	}

	// TODO: Layer 클래스 추가 및 초기화 동작
	static void AddLayerType(GameObject layer, CellInfo info)
	{
		int layerMask = info.layerTypes;
		if (layerMask == 0)
			return;
	}

	static void AddSealType(Seal seal, CellInfo info)
	{
		int sealMask = info.sealTypes;
		seal.sealType = info.sealTypes;

		if (sealMask == 0)
			return;
	}
}
