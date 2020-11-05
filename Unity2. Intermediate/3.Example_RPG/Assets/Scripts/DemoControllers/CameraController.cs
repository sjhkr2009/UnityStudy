using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{
	public Define.CameraMode cameraMode = Define.CameraMode.QuarterView;
	[SerializeField] Vector3 _delta = new Vector3(0, 4, -5);
	[SerializeField] Transform player;


	void LateUpdate()
	{
		if (!player.gameObject.IsValid())
			return;
		
		if (cameraMode == Define.CameraMode.QuarterView)
			QuarterViewAction();
	}

	public void SetQuarterView(Vector3 delta)
	{
		cameraMode = Define.CameraMode.QuarterView;
		_delta = delta;
	}
	void QuarterViewAction()
	{
		RaycastHit hit;
		bool raycastHit = Physics.Raycast(player.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Block", "Wall"));
		transform.position = raycastHit ?
			(player.position + Vector3.up * 1.7f) + (_delta.normalized * (hit.point - player.position).magnitude * 0.8f) :
			(player.position + Vector3.up * 1.7f) + _delta;

		transform.LookAt(player);
	}

	[Button]
	public void SetCameraPos()
	{
		if (player == null)
			player = FindObjectOfType<PlayerController>().transform;

		transform.position = (player.position + Vector3.up) + _delta;
		transform.LookAt(player);
	}
}
