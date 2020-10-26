using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CocktailReactionUI : UIBase_Popup
{
    enum Images
	{
		CocktailImage,
		HeartIcon
	}

	enum Buttons
	{
		NextButton
	}

	enum Texts
	{
		ReactionText,
		HeartText
	}
	enum RectTransforms
	{
		HeartIcon,
		Pos1,
		Pos2,
		Pos3,
		VariableImage
	}

	DataManager data = GameManager.Data;
	float space;
	
	void Start() => Init();

	public override void Init()
	{
		base.Init();

		Bind<Image>(typeof(Images));
		Bind<Text>(typeof(Texts));
		Bind<Button>(typeof(Buttons));
		Bind<RectTransform>(typeof(RectTransforms));

		GetButton((int)Buttons.NextButton).onClick.AddListener(GameManager.Instance.SetDialog);
		space = Get<RectTransform>((int)RectTransforms.Pos2).position.x - Get<RectTransform>((int)RectTransforms.Pos1).position.x;

		data.SetReward();

		SetDialog();
		SetImage();
		SetHeartIcon();

	}

	void SetDialog()
	{
		GetButton((int)Buttons.NextButton).interactable = false;

		string reactionText = data.CurrentCustomer.ReactionSctipt[(Define.Reaction)data.CurrentGrade];
		GetText((int)Texts.ReactionText).DOText(reactionText, reactionText.Length * Define.DoTextSpeed).OnComplete(() =>
		{
			GetButton((int)Buttons.NextButton).interactable = true;
		});
	}

	void SetHeartIcon()
	{
		Image icon = GetImage((int)Images.HeartIcon);
		Vector3 iconPos = icon.transform.position;

		icon.DOFade(1f, 0.5f);
		icon.transform.DOMoveY(iconPos.y * 1.1f, 0.5f).OnComplete(() =>
		{
			SetExpText();
		});
	}

	void SetImage()
	{
		GetImage((int)Images.CocktailImage).sprite = data.CurrentCocktail.image;

		RectTransform images = Get<RectTransform>((int)RectTransforms.VariableImage);
		if (data.CurrentTableIndex == 0)
		{
			images.DOMoveX(images.position.x - space, 0f);
		}
		else if(data.CurrentTableIndex == 2)
		{
			images.DOMoveX(images.position.x + space, 0f);
		}
	}

	void SetExpText()
	{
		Text text = GetText((int)Texts.HeartText);
		if(data.levelUp)
		{
			text.text = "1 UP!";
			text.transform.DOScale(1.1f, 0.25f).SetEase(Ease.OutBack);
		}
		else
		{
			int required = Define.RequiredEXP[data.CurrentCustomer.Level];
			float beforePercent = ((float)data.beforeExp / required) * 100f;
			float afterPercent = ((float)data.afterExp / required) * 100f;

			int result = (int)(afterPercent - beforePercent);

			text.text = $"+ {result}%";
		}
	}

	private void OnDestroy()
	{
		GetText((int)Texts.HeartText).transform.DOKill();
		GetImage((int)Images.HeartIcon).DOKill();
		GetImage((int)Images.HeartIcon).transform.DOKill();
	}
}
