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
	private SpriteRenderer _spriteRenderer;
	public SpriteRenderer SpriteRenderer
    {
        get
        {
			if (_spriteRenderer == null)
				_spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

			return _spriteRenderer;
        }
		set => _spriteRenderer = value;
    }

	public bool IsMovable => (Cell.info.sealTypes & (int)SealType.Immovable) == 0 && 
		(Cell.info.specialTypes & (int)SpecialType.Immovable) == 0;

	public BlockType Type { get; private set; }
	public int SpecialTypes { get; private set; }
	public void SetType(BlockType type, int specialTypes)
    {
		Type = type;
		SpecialTypes = specialTypes;
		SpriteRenderer.sprite = CellGenerator.LoadBlockImage(Type, SpecialTypes);
	}

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

	public IEnumerator MoveOrigin()
    {
		while (true)
        {
			yield return null;

			Vector2 next = Vector2.Lerp(transform.localPosition, Vector2.zero, BlockMove.LerpMoveSpeed);
			transform.localPosition = next;

			if(Vector2.Distance(transform.localPosition, Vector2.zero) < 0.01f)
            {
				transform.localPosition = Vector2.zero;
				break;
            }
        }
	}
}
