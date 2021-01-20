using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 제어와 좌표계 변환을 담당합니다.
/// TODO: 보드 크기에 따른 카메라 크기 조정
/// </summary>
public class CameraManager
{
	private Camera _main;
	/// <summary>
	/// 메인 카메라를 반환합니다. 게임 중 메인 카메라 반복 호출 시 Camera.main을 통한 Find계열 함수의 호출을 방지합니다.
	/// </summary>
	public Camera Main
	{
		get
		{
			if (_main == null)
				_main = Camera.main;

			return _main;
		}
	}

	/// <summary>
	/// 입력한 좌표를 World 좌표로 변환하여 반환합니다.
	/// </summary>
	public Vector2 ToWorldPos(Vector2 pos)
    {
		return Main.ScreenToWorldPoint(pos);
    }

	/// <summary>
	/// 입력된 크기를 조정하여 변경합니다. (종횡비가 불균형할수록 size가 커집니다)
	/// </summary>
	public void SetSizeByAspect(float size)
	{
		Main.orthographicSize = Main.aspect < 1f ?
			size * (1f / Main.aspect) : size * (Main.aspect);
	}
	/// <summary>
	/// 현재 카메라 크기를 조정하여 변경합니다. (종횡비가 불균형할수록 size가 커집니다)
	/// </summary>
	public void SetSizeByAspect()
	{
		SetSizeByAspect(Main.orthographicSize);
	}
}
