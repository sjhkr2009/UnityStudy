using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : BaseUI
{
	enum Sliders
	{
		HPBar
	}

	Transform _parent;
	Transform _cam;
	Vector3 _delta;
	Stat _stat;

	protected override void Init()
	{
		Bind<Slider>(typeof(Sliders));

		_parent = transform.parent;
		_cam = Camera.main.transform;
		_delta = Vector3.up * (_parent.GetComponent<Collider>().bounds.size.y * 1.2f);
		_stat = _parent.GetComponent<Stat>();
	}

	private void Update()
	{
		transform.position = _parent.position + _delta;
		transform.rotation = _cam.rotation;

		UpdateHpBar();
	}

	void UpdateHpBar()
	{
		float ratio = (float)_stat.Hp / _stat.MaxHp;
		SetHpRatio(ratio);
	}

	void SetHpRatio(float ratio)
	{
		Get<Slider>((int)Sliders.HPBar).value = ratio;
	}
}
