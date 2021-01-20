using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// Cell의 구성요소 중 블록에 해당하는 클래스입니다.
/// 이동이 제한되지 않은 경우 유저의 조작을 받아 위치를 이동할 수 있습니다.
/// </summary>
public class Block : MonoBehaviour
{
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

	/// <summary>
	/// 이동 불가한 타입인 경우 false를 반환합니다
	/// </summary>
	public bool IsMovable => (SpecialTypes & (int)SpecialType.Immovable) == 0;

	public BlockType Type { get; private set; }
	public int SpecialTypes { get; private set; }
	/// <summary>
	/// 파괴되기 전에 이 블록의 특수 타입이 저장되어, Board에서 추가 동작을 수행하게 합니다.
	/// </summary>
	public int PrevSpecialType { get; private set; }
	/// <summary>
	/// 타입을 저장하고, 타입에 맞는 Sprite 이미지를 로딩하여 이 오브젝트에 세팅합니다.
	/// </summary>
	public void SetType(BlockType type, int specialTypes)
    {
		Type = type;
		SpecialTypes = specialTypes;
		if(type == BlockType.None)
		{
			SpriteRenderer.sprite = null;
			return;
		}
		SpriteRenderer.sprite = CellGenerator.LoadBlockImage(Type, SpecialTypes);
	}

	/// <summary>
	/// 클릭되었을 때의 효과를 출력합니다. 현재는 크기를 20% 증가시킵니다.
	/// </summary>
	public void OnClick()
    {
		transform.localScale = Vector3.one * 1.2f; 
    }
	/// <summary>
	/// 클릭 해제되었을 때 호출되어 클릭 효과를 해제합니다.
	/// </summary>
	public void UnClick()
    {
		transform.localScale = Vector3.one;
	}

	/// <summary>
	/// 로컬 좌표 기준 원점으로 일정 시간에 걸쳐 이동합니다.
	/// 이 블록이 속한 상위 Cell의 위치로 이동합니다.
	/// </summary>
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

	/// <summary>
	/// 블록 파괴 동작을 수행합니다. 동작이 끝나면 이 오브젝트의 타입을 None으로 변경하며 이미지를 삭제합니다.
	/// (현재는 별도의 이펙트가 없어 작아지다가 사라집니다)
	/// </summary>
	public IEnumerator Crush()
	{
		while (true)
		{
			yield return null;

			Vector2 next = Vector2.Lerp(transform.localScale, Vector2.zero, BlockMove.LerpMoveSpeed);
			transform.localScale = next;

			if(Vector2.Distance(transform.localScale, Vector2.zero) < 0.01f)
			{
				break;
			}
		}
		PrevSpecialType = SpecialTypes;
		SetType(BlockType.None, 0);
		transform.localScale = Vector2.one;
	}
}
