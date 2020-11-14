using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToastMessageUI : UIBase_Popup
{
	Image bg;
	Text msg;
	float openTime = 1f;
	float closingTime = 0.3f;

	void Start() => Init();
	public override void Init()
	{
		GameManager.UI.SetCanvasOrder(gameObject, true, 9999);

		bg = gameObject.FindChild<Image>("Background");
		if(msg == null)
			msg = gameObject.FindChild<Text>("Message");

		PopupAnimation();
		DOVirtual.DelayedCall(openTime, Close);
	}
	public void SetMessage(string text)
	{
		if (msg == null)
			msg = gameObject.FindChild<Text>("Message");

		msg.text = text;
	}
	void PopupAnimation()
	{
		bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0f);
		msg.color = new Color(msg.color.r, msg.color.g, msg.color.b, 0f);

		bg.DOFade(0.3f, openTime / 2f).SetEase(Ease.OutCubic);
		msg.DOFade(1f, openTime / 2f).SetEase(Ease.OutCubic);
	}

	void Close()
	{
		bg.DOFade(0f, closingTime).SetEase(Ease.OutCubic);
		msg.DOFade(0f, closingTime).SetEase(Ease.OutCubic).OnComplete(() =>
		{
			GameManager.Resource.Destroy(gameObject);
		});
	}
	private void OnDestroy()
	{
		bg.DOKill();
		msg.DOKill();
	}
}
