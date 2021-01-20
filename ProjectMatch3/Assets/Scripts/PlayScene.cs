using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이 씬에서 공통으로 사용됩니다. 이 컴포넌트는 씬에 반드시 포함되어야 하며 스테이지 번호가 입력되어야 합니다.
/// 이 스테이지의 초기 데이터를 로딩하여 게임이 진행될 Board를 생성합니다.
/// </summary>
public class PlayScene : SceneBase
{
	public int stageNumber;
	private StageData stageData;
	public Board Board { get; private set; }

	protected override void Init()
	{
		sceneType = Define.SceneType.Play;

		stageData = GameManager.Resource.LoadStageData(stageNumber);
		Board = new Board(stageData.height, stageData.width, stageData.blocks);
	}

	// TODO: SceneManager를 통한 이동 시 SceneBase의 Clear를 호출하게 할 것
	public override void Clear()
	{
		GameManager.Clear();
	}
}
