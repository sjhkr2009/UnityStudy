using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = "CustomDatabase/Database", order = int.MinValue + 2)]
public class ScriptableData : ScriptableObject
{
    [ShowInInspector, ReadOnly] public Dictionary<string, int> CustomerLevel { get; private set; } = new Dictionary<string, int>() { };
    [ShowInInspector, ReadOnly] public Dictionary<string, int> CustomerExp { get; private set; } = new Dictionary<string, int>() { };
    int CustomerCount => CustomerLevel.Count;
    [ShowInInspector, ReadOnly] public int Birdcoin { get; private set; }
    [ShowInInspector, ReadOnly] public List<string> CollectedRecipe { get; private set; } = new List<string>();
    int RecipeCount => CollectedRecipe.Count;
    [ShowInInspector, ReadOnly] public List<string> CollectedSubmaterial { get; private set; } = new List<string>();
    int SubmaterialCount => CollectedSubmaterial.Count;

    #region Set Data
    public void SetLevel(string name, int level)
    {
        if (CustomerLevel.ContainsKey(name)) CustomerLevel[name] = level;
        else CustomerLevel.Add(name, level);
    }
    public void SetExp(string name, int exp)
    {
        if (CustomerLevel.ContainsKey(name)) CustomerLevel[name] = exp;
        else CustomerLevel.Add(name, exp);
    }
    public void SetBirdcoin(int value) => Birdcoin = value;
    public void SetRecipe(string id) => CollectedRecipe.Add(id);
    public void SetMaterial(string id) => CollectedSubmaterial.Add(id);
    #endregion

    #region Save Data
    public void Save()
    {
        SaveLevel();
        SaveExp();
        SaveCoin();
        SaveRecipe();
        SaveMaterials();
    }
    void SaveLevel()
    {
        int count = 0;
        foreach (var item in CustomerLevel)
        {
            PlayerPrefs.SetInt(item.Key, item.Value);
            PlayerPrefs.SetString($"Customer{count}Level", item.Key);
            count++;
        }
        PlayerPrefs.SetInt(nameof(CustomerCount), CustomerCount);
    }
    void SaveExp()
    {
        int count = 0;
        foreach (var item in CustomerExp)
        {
            PlayerPrefs.SetInt(item.Key, item.Value);
            PlayerPrefs.SetString($"Customer{count}Exp", item.Key);
            count++;
        }
        PlayerPrefs.SetInt(nameof(CustomerCount), CustomerCount);
    }
    void SaveCoin() => PlayerPrefs.SetInt(nameof(Birdcoin), Birdcoin);
    void SaveRecipe()
    {
        for (int i = 0; i < RecipeCount; i++)
        {
            PlayerPrefs.SetString($"{nameof(CollectedRecipe)}{i}", CollectedRecipe[i]);
        }
        PlayerPrefs.SetInt(nameof(RecipeCount), RecipeCount);
    }
    void SaveMaterials()
    {
        for (int i = 0; i < SubmaterialCount; i++)
        {
            PlayerPrefs.SetString($"{nameof(CollectedSubmaterial)}{i}", CollectedSubmaterial[i]);
        }
        PlayerPrefs.SetInt(nameof(SubmaterialCount), SubmaterialCount);
    }

    #endregion

    #region Load Data
    public void Load()
    {
        LoadLevel();
        LoadExp();
        LoadCoin();
        LoadRecipe();
        LoadMaterials();
    }

    void LoadLevel()
    {
        int count = PlayerPrefs.GetInt(nameof(CustomerCount));
        CustomerLevel.Clear();
        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString($"Customer{i}Level");
            if (string.IsNullOrEmpty(key)) continue;

            int value = PlayerPrefs.GetInt(key);
            CustomerLevel.Add(key, value);
        }
    }
    void LoadExp()
    {
        int count = PlayerPrefs.GetInt(nameof(CustomerCount));
        CustomerExp.Clear();
        for (int i = 0; i < count; i++)
        {
            string key = PlayerPrefs.GetString($"Customer{i}Exp");
            if (string.IsNullOrEmpty(key)) continue;
            int value = PlayerPrefs.GetInt(key);
            CustomerExp.Add(key, value);
        }
    }
    void LoadCoin() => Birdcoin = PlayerPrefs.GetInt(nameof(Birdcoin));
    void LoadRecipe()
    {
        int count = PlayerPrefs.GetInt(nameof(RecipeCount));
        CollectedRecipe.Clear();
        for (int i = 0; i < count; i++)
        {
            string id = PlayerPrefs.GetString($"{nameof(CollectedRecipe)}{i}");
            CollectedRecipe.Add(id);
        }
    }
    void LoadMaterials()
    {
        int count = PlayerPrefs.GetInt(nameof(SubmaterialCount));
        CollectedSubmaterial.Clear();
        for (int i = 0; i < count; i++)
        {
            string id = PlayerPrefs.GetString($"{nameof(CollectedSubmaterial)}{i}");
            CollectedSubmaterial.Add(id);
        }
    }
    #endregion

    [Button]
    void Reset()
    {
        CustomerLevel.Clear();
        CollectedRecipe.Clear();
        CollectedSubmaterial.Clear();
        Birdcoin = 0;
        PlayerPrefs.DeleteAll();
    }
}
