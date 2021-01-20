using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define.Board;

/// <summary>
/// 게임이 진행되는 퍼즐판에 해당하는 클래스
/// </summary>
public class Board
{
	/// <summary>
	/// 퍼즐 생성 시 초기화 요소를 입력합니다
	/// </summary>
	/// <param name="height">가로축 1행의 블럭 개수</param>
	/// <param name="width">세로축 1열의 블럭 개수</param>
	/// <param name="boardInfo">초기 블럭을 생성할 때 사용되는 데이터로, 각 원소에 따른 초기값은 Define.Board.InitData에 정의됩니다.</param>
	public Board(int height, int width, int[] boardInfo)
	{
		Behavior = Root.GetOrAddComponent<BoardBehavior>();
		Behavior.SetBoard(this);
		Helper = new BoardHelper(this, height, width);
		Input = new BoardInput(this);

		GameManager.Input.InputAction += Input.OnInputAction;

		Cells = new Cell[Helper.CellCount];
		for (int i = 0; i < Helper.CellCount; i++)
		{
			Cells[i] = new Cell(boardInfo[i], Helper.GetIndexX(i), Helper.GetIndexY(i), i);
			Cells[i].transform.parent = Root;
			Cells[i].transform.position = Helper.GetWorldPos(i);
			Cells[i].OnDestroy += OnBlockDestroy;
		}

		Helper.ProtectMatchOnInit();
	}

	~Board()
    {
		GameManager.Input.InputAction -= Input.OnInputAction;
	}

	public BoardBehavior Behavior { get; private set; }
	public BoardHelper Helper { get; private set; }
	public BoardInput Input { get; private set; }
	/// <summary>
	/// 보드의 각 칸을 저장하는 배열. 좌측 상단이 인덱스 0에 해당합니다.
	/// </summary>
	public Cell[] Cells { get; private set; }
	/// <summary>
	/// 파괴된 블럭이 저장되는 큐. 빈 공간을 채울 때 이곳의 블럭이 사용됩니다.
	/// </summary>
	public Queue<Block> DestroyedBlocks { get; set; } = new Queue<Block>();

	private Transform _root;
	/// <summary>
	/// 보드판에 해당하는 오브젝트로 모든 Cell은 이 Transform 하위에 위치합니다.
	/// </summary>
	public Transform Root
	{
		get
		{
			if(_root == null)
			{
				GameObject go = GameObject.Find(Name.Root);
				if (go == null)
					go = new GameObject(name: Name.Root);

				_root = go.transform;
			}

			return _root;
		}
	}

	/// <summary>
	/// 블럭이 파괴될 때마다 호출됩니다. 파괴된 블럭을 저장하고 특수 블럭에 대한 추가 동작을 실행합니다.
	/// </summary>
	private void OnBlockDestroy(Block block, int index)
	{
		DestroyedBlocks.Enqueue(block);

		// TODO: 특수 블록 파괴 시 처리
		if((block.PrevSpecialType & (int)SpecialType.HorizontalCrush) > 0)
		{
			
		}
	}
}
