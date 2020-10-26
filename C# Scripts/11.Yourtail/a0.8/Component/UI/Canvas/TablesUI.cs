using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablesUI : UIBase_Scene
{
    enum Tables
	{
		Customer1,
		Customer2,
		Customer3,
		Count
	}
	
	void Start() => Init();
	public override void Init()
	{
		Init(0);

		Bind<CustomerTable>(typeof(Tables));
		for (int i = 0; i < (int)Tables.Count; i++)
		{
			Get<CustomerTable>(i).EventOnSelectCustomer -= GetOrder;
			Get<CustomerTable>(i).EventOnSelectCustomer += GetOrder;
			Get<CustomerTable>(i).myIndex = i;
		}
		GameManager.Data.DeleteCustomer -= DeleteCustomer;
		GameManager.Data.DeleteCustomer += DeleteCustomer;
		GameManager.Instance.OnGameStateChange -= OnGameStateChange;
		GameManager.Instance.OnGameStateChange += OnGameStateChange;
		StopAllCoroutines();
		StartCoroutine(nameof(SetTable));

		Inited = true;
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
				newCustomer = GameManager.Data.GetRandomCustomer();
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

		GameManager.Data.SelectCustomer(customer);
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
			if (Get<CustomerTable>(i).currentCustomer == GameManager.Data.CurrentCustomer)
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
