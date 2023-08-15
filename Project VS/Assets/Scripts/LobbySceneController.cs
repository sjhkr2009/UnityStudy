using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneController : MonoBehaviour {
    [SerializeField] private GameObject statusPopup;
    [SerializeField] private TMP_Text coinCount;
    [SerializeField] private TMP_Text coinCountOnPopup;
    
    [SerializeField] private TMP_Text attackPowerLevel;
    [SerializeField] private TMP_Text moveSpeedLevel;
    [SerializeField] private TMP_Text maxHpLevel;
    [SerializeField] private TMP_Text criticalRateLevel;
    [SerializeField] private TMP_Text attackSpeedLevel;
    
    private bool clickedPlay = false;

    private void Awake() {
        Application.targetFrameRate = 60;
    }

    private void Start() {
        UpdateTextUI();
        statusPopup.SetActive(false);
    }

    void UpdateTextUI() {
        coinCount.text = UserData.Coin.ToString();
        coinCountOnPopup.text = UserData.Coin.ToString();

        var data = UserData.GetData();
        attackPowerLevel.text = data.attackPowerUpgrade >= 5 ? "Max" : $"Lv.{data.attackPowerUpgrade}";
        moveSpeedLevel.text = data.moveSpeedUpgrade >= 5 ? "Max" : $"Lv.{data.moveSpeedUpgrade}";
        maxHpLevel.text = data.maxHpUpgrade >= 5 ? "Max" : $"Lv.{data.maxHpUpgrade}";
        criticalRateLevel.text = data.criticalRateUpgrade >= 5 ? "Max" : $"Lv.{data.criticalRateUpgrade}";
        attackSpeedLevel.text = data.attackSpeedUpgrade >= 5 ? "Max" : $"Lv.{data.attackSpeedUpgrade}";
    }

    public void PlayGame() {
        if (clickedPlay) return;
        
        SceneManager.LoadScene("GameScene");
    }

    public void OpenStatusPopup() {
        statusPopup.SetActive(true);
        UpdateTextUI();
    }
    
    public void CloseStatusPopup() {
        statusPopup.SetActive(false);
        UpdateTextUI();
    }

    public void UpgradeAttackPower() {
        var data = UserData.GetData();
        if (data.attackPowerUpgrade >= 5) return;
        if (UserData.Coin < 1000) return;
        
        data.attackPowerUpgrade++;
        UserData.Coin -= 1000;
        UpdateTextUI();
    }
    
    public void UpgradeCriticalRate() {
        var data = UserData.GetData();
        if (data.criticalRateUpgrade >= 5) return;
        if (UserData.Coin < 1000) return;
        
        data.criticalRateUpgrade++;
        UserData.Coin -= 1000;
        UpdateTextUI();
    }
    public void UpgradeMaxHp() {
        var data = UserData.GetData();
        if (data.maxHpUpgrade >= 5) return;
        if (UserData.Coin < 1000) return;
        
        data.maxHpUpgrade++;
        UserData.Coin -= 1000;
        UpdateTextUI();
    }
    public void UpgradeAttackSpeed() {
        var data = UserData.GetData();
        if (data.attackSpeedUpgrade >= 5) return;
        if (UserData.Coin < 1000) return;
        
        data.attackSpeedUpgrade++;
        UserData.Coin -= 1000;
        UpdateTextUI();
    }
    public void UpgradeMoveSpeed() {
        var data = UserData.GetData();
        if (data.moveSpeedUpgrade >= 5) return;
        if (UserData.Coin < 1000) return;
        
        data.moveSpeedUpgrade++;
        UserData.Coin -= 1000;
        UpdateTextUI();
    }
}
