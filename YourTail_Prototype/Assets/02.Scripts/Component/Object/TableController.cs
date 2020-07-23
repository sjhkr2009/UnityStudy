using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public Table[] tables = new Table[3];
    public GameObject selectUI;

    void Start()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i] == null) tables[i] = transform.GetChild(i).GetOrAddComponent<Table>();
            tables[i].EventOnSelectCustomer -= GetOrder;
            tables[i].EventOnSelectCustomer += GetOrder;
        }

        if (selectUI == null)
            selectUI = transform.Find("CustomerSelectUI").gameObject;

        if (selectUI.activeSelf)
            selectUI.SetActive(false);

        EscapeOnClick clickMethod = selectUI.GetOrAddComponent<EscapeOnClick>();
        clickMethod.isUI = false;
        clickMethod.interactableOn.Add(GameState.Order);
        clickMethod.interactableOn.Add(GameState.Idle);
        GameManager.Input.InputEscape -= CancelOrder;
        GameManager.Input.InputEscape += CancelOrder;

        GameManager.Instance.OnGameStateChange -= OnGameStateChange;
        GameManager.Instance.OnGameStateChange += OnGameStateChange;
        StartCoroutine(nameof(SetTable));
    }
    private void OnDestroy()
    {
        foreach (Table item in tables) item.EventOnSelectCustomer -= GetOrder;
        GameManager.Input.InputEscape -= CancelOrder;
        GameManager.Instance.OnGameStateChange -= OnGameStateChange;
    }

    void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Idle:
                if (selectUI.activeSelf) selectUI.SetActive(false);
                StartCoroutine(nameof(SetTable));
                break;
            case GameState.SetCocktail:
                DeleteCustomer(GameManager.Data.CurrentCustomer);
                StopAllCoroutines();
                break;
            default:
                StopAllCoroutines();
                break;
        }
    }

    IEnumerator SetTable()
    {
        while (GameManager.Instance.GameState == GameState.Idle)
        {
            yield return null;

            yield return new WaitForSeconds(3f);

            if (!HasEmptyTable()) continue;

            Customers newCustomer = GameManager.Data.GetRandomCustomer();

            if (IsExistCustomer(newCustomer)) continue;

            while (true)
            {
                yield return null;
                int index = Random.Range(0, tables.Length);
                if (!tables[index].gameObject.activeSelf)
                {
                    tables[index].gameObject.SetActive(true);
                    tables[index].SetCustomer(newCustomer);
                    break;
                }
            }
        }
    }

    bool HasEmptyTable()
    {
        bool hasEmptyTable = false;

        foreach (var table in tables)
        {
            if (!table.HasCustomer)
            {
                hasEmptyTable = true;
                break;
            }
        }
        return hasEmptyTable;
    }
    bool IsExistCustomer(Customers customer)
    {
        bool isExist = false;

        foreach (var table in tables)
        {
            if (table.HasCustomer && table.currentCustomer == customer)
            {
                isExist = true;
                break;
            }
        }
        return isExist;
    }

    void GetOrder(Customers customer)
    {
        GameManager.Data.SelectCustomer(customer);
        GameManager.UI.OpenPopupUI<OrderBubble>();
        selectUI.SetActive(true);
    }
    void CancelOrder()
    {
        if (!selectUI.activeSelf || GameManager.Instance.GameState != GameState.Idle) return;
        GameManager.UI.ClosePopupUI();
        selectUI.SetActive(false);
    }
    void DeleteCustomer(Customers customer)
    {
        foreach (Table table in tables)
        {
            if (table.currentCustomer == GameManager.Data.CurrentCustomer)
                table.DeleteCustomer();
        }
    }
}
