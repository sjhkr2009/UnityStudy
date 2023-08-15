using System.Linq;
using TMPro;
using UnityEngine;

public class PauseUI : GameListenerBehaviour {
    [SerializeField] private CellItem[] silverItems;
    [SerializeField] private CellItem[] goldItems;
    [SerializeField] private CellItem[] platinumItems;
    [SerializeField] private TextMeshProUGUI killCount;

    [SerializeField] private GameObject curtain;
    [SerializeField] private StatusItem[] statusItems;

    private void Awake() {
        curtain.SetActive(false);
    }
    
    public void Show() {
        curtain.SetActive(true);
        SetData();
    }

    public void Release() {
        curtain.SetActive(false);
        GameManager.Controller.ResumeGame();
    }

    private void SetData() {
        int indexSilver = 0;
        int indexGold = 0;
        int indexPlatinum = 0;
        
        var combined = silverItems.Zip(goldItems, (a, b) => new { a, b })
            .Zip(platinumItems, (ab, c) => (ab.a, ab.b, c));
        
        foreach (var cell in combined) {
            cell.a.gameObject.SetActive(false);
            cell.b.gameObject.SetActive(false);
            cell.c.gameObject.SetActive(false);
        }

        foreach (var card in CardManager.CurrentSelectedCard) {
            switch (card.cardTier) {
                case CardTier.SILVER:
                    silverItems[indexSilver].gameObject.SetActive(true);
                    silverItems[indexSilver].Init(card.cardIcon);
                    indexSilver++;
                    break;
                case CardTier.GOLD:
                    goldItems[indexGold].gameObject.SetActive(true);
                    goldItems[indexGold].Init(card.cardIcon);
                    indexGold++;
                    break;
                case CardTier.PLATINUM:
                    platinumItems[indexPlatinum].gameObject.SetActive(true);
                    platinumItems[indexPlatinum].Init(card.cardIcon);
                    indexPlatinum++;
                    break;
            }
        }

        killCount.text = GameManager.Controller.KillCount.ToString();

        foreach (var item in statusItems) {
            item.Init();
        }
    }

    public void OnClickPause() {
        GameManager.Controller.PauseGame();
        Show();
    }

    public void OnClickClose() {
        Release();
    }
}