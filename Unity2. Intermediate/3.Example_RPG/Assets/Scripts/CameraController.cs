using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Define.CameraMode cameraMode = Define.CameraMode.QuarterView;
	[SerializeField] Vector3 _delta = new Vector3(0, 4, -5);
	[SerializeField] Transform player;

	private void Start()
	{
		if (player == null)
			player = FindObjectOfType<PlayerController>().transform;
	}

	void LateUpdate()
	{
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
		if (Physics.Raycast(player.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
		{
			float dist = (hit.point - player.position).magnitude * 0.8f;
			transform.position = (player.position + Vector3.up) + _delta.normalized * dist;
		}
		else
		{
			transform.position = (player.position + Vector3.up) + _delta;
			transform.LookAt(player);
		}
	}
}
