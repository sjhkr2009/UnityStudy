using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInput : MonoBehaviour
{
	public Board board;
	private void OnMouseDown()
	{
		Vector2 mousePos = GameManager.Instance.Camera.ScreenToWorldPoint(Input.mousePosition);
		Debug.Log($"Pos: {mousePos} / Index: {board.ToIndex(mousePos)}");
	}
}
