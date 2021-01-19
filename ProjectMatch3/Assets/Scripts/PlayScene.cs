using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayScene : SceneBase
{
	public int stageNumber;
	private StageData stageData;
	private Board board;

	protected override void Init()
	{
		sceneType = Define.SceneType.Play;

		stageData = GameManager.Resource.LoadStageData(stageNumber);
		board = new Board(stageData.height, stageData.width, stageData.blocks);
	}

	public override void Clear()
	{

	}
}
