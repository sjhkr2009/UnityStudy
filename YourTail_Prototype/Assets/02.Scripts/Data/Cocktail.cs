using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CocktailName
{
    None,
    BlueHawaii,
    BetweenTheSheets
}

public class Cocktail
{
    public List<BaseMaterials> BaseMaterials { get; private set; } = new List<BaseMaterials>();
    public List<SubMaterials> SubMaterials { get; private set; } = new List<SubMaterials>();
    public List<string> BaseIDList { get; private set; } = new List<string>();
    public List<string> SubIDList { get; private set; } = new List<string>();
    public CocktailName cocktailName = CocktailName.None;

    protected void AddBase(BaseMaterials material)
    {
        BaseMaterials.Add(material);
        BaseIDList.Add(material.Id);
    }
    protected void AddSub(SubMaterials material)
    {
        SubMaterials.Add(material);
        SubIDList.Add(material.Id);
    }

    public int Sweetness { get; protected set; }
    public int Proof { get; protected set; }
    public int Refreshment { get; protected set; }
    public string Id { get; private set; }
    public void SetID(int code)
    {
        Id = "C" + code.ToString();
    }
    public Cocktail(int id)
    {
        SetID(id);
    }
    public Cocktail()
    {
        SetID(0);
        cocktailName = CocktailName.None;
    }
}

class BetweenTheSheets : Cocktail
{
    public BetweenTheSheets() : base(1)
    {
        AddBase(new Brandy());
        AddSub(new Lemon());
        AddSub(new Curacao());

        cocktailName = CocktailName.BetweenTheSheets;

        Sweetness = 2;
        Proof = 4;
        Refreshment = 2;
    }
}

class BlueHawaii : Cocktail
{
    public BlueHawaii() : base(2)
    {
        AddBase(new Rum());
        AddSub(new Curacao());
        AddSub(new Pineapple());

        cocktailName = CocktailName.BlueHawaii;

        Sweetness = 3;
        Proof = 2;
        Refreshment = 5;
    }
}