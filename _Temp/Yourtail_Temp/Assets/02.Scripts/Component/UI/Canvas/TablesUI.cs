using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablesUI : UIBase_Scene
{
    enum Tables
	{
		Customer1,
		Customer2,
		Customer3,
		Count
	}
	enum Buttons
	{
		MakeButton
	}
	
	void Start() => Init();
	public override void Init()
	{
		Init(0);

		Bind<CustomerTable>(typeof(Tables));
		Bind<Button>(typeof(Buttons));

		GetButton((int)Buttons.MakeButton).onClick.AddListener(OnMakeButtonClick);

		for (int i = 0; i < (int)Tables.Count; i++)
		{
			Get<CustomerTable>(i).EventOnSelectCustomer -= GetOrder;
			Get<CustomerTable>(i).EventOnSelectCustomer += GetOrder;
			Get<CustomerTable>(i).myIndex = i;
		}
		GameManager.Game.DeleteCustomer -= DeleteCustomer;
		GameManager.Game.DeleteCustomer += DeleteCustomer;
		GameManager.Instance.OnGameStateEnter -= OnGameStateChange;
		GameManager.Instance.OnGameStateEnter += OnGameStateChange;
		StopAllCoroutines();
		StartCoroutine(nameof(SetTable));

		Inited = true;
	}

	void OnMakeButtonClick()
	{
		if (GameManager.Game.CurrentOrder == null)
		{
			GameManager.UI.ToastMessage("주문을 받아야 칵테일 제조를 시작할 수 있습니다.");
			return;
		}

		GameManager.Instance.GameState = GameState.Select;
	}

	void OnGameStateChange(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Idle:
				//if (selectUI.activeSelf) selectUI.SetActive(false);
				StartCoroutine(nameof(SetTable));
				break;
			default:
				StopAllCoroutines();
				break;
		}
	}

	IEnumerator SetTable()
	{
		yield return new WaitForSeconds(1f);

		while (GameManager.Instance.GameState == GameState.Idle)
		{
			yield return null;

			Customers newCustomer;

			do
			{
				yield return null;
				newCustomer = GameManager.Game.GetRandomCustomer();
			} while (!newCustomer.IsActive);

			if (IsExistCustomer(newCustomer) || !HasEmptyTable())
			{
				yield return new WaitForSeconds(1f);
				continue;
			}

			while (true)
			{
				yield return null;
				int index = Random.Range(0, (int)Tables.Count);
				if (Get<CustomerTable>(index).IsEmpty)
				{
					Get<CustomerTable>(index).SetCustomer(newCustomer);
					break;
				}
			}

			yield return new WaitForSeconds(2f);
		}
	}
	bool HasEmptyTable()
	{
		for (int i = 0; i < (int)Tables.Count; i++)
		{
			if(Get<CustomerTable>(i).IsEmpty)
				return true;
		}
		return false;
	}
	bool IsExistCustomer(Customers target)
	{
		for (int i = 0; i < (int)Tables.Count; i++)
		{
			if (!Get<CustomerTable>(i).IsEmpty && Get<CustomerTable>(i).currentCustomer.Name == target.Name)
				return true;
		}
		return false;
	}

	void GetOrder(Customers customer)
	{
		if (GameManager.Instance.GameState != GameState.Idle)
			return;

		GameManager.Game.SelectCustomer(customer);
		GameManager.UI.OpenPopupUI<OrderBubble>().tablesUI = this;
		//foreach (Table item in tables)
		//	item.SetLayer(customer == item.currentCustomer);
		//selectUI.SetActive(true);
	}
	public void CancelOrder()
	{
		
	}
	void DeleteCustomer()
	{
		for (int i = 0; i < (int)Tables.Count; i++)
		{
			if (Get<CustomerTable>(i).currentCustomer == GameManager.Game.CurrentCustomer)
			{
				Get<CustomerTable>(i).DeleteCustomer();
				break;
			}
		}
	}
	private void OnDestroy()
	{
		for (int i = 0; i < (int)Tables.Count; i++)
			Get<CustomerTable>(i).EventOnSelectCustomer -= GetOrder;
	}
}
