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
/*
public class Cocktail
{
    protected BaseMaterials baseMaterial;
    protected List<SubMaterials> subMaterials = new List<SubMaterials>();
    public List<int> materialCodes { get; set; }
    public string Name { get; protected set; }
    public CocktailName cocktailName;

    public int Sweetness { get; protected set; }
    public int Proof { get; protected set; }
    public int Refreshment { get; protected set; }
}*/

public class Cocktail
{
    public BaseMaterial BaseMaterial { get; protected set; }
    protected List<SubMaterials> subMaterials = new List<SubMaterials>();
    public List<int> MaterialCode { get; protected set; }
    public CocktailName cocktailName = CocktailName.None;

    public int Sweetness { get; protected set; }
    public int Proof { get; protected set; }
    public int Refreshment { get; protected set; }

    protected List<int> GetMaterialCode()
    {
        List<int> codes = new List<int>();

        foreach (var subMaterial in subMaterials)
        {
            codes.Add(subMaterial.Id);
        }

        codes.Sort();
        codes.Insert(0, BaseMaterial.Id);

        return codes;
    }
    public Cocktail() { }
}

class BetweenTheSheets : Cocktail
{
    public BetweenTheSheets()
    {
        BaseMaterial = new Brandy();
        subMaterials.Add(new Lemon());
        subMaterials.Add(new Curacao());

        MaterialCode = GetMaterialCode();

        cocktailName = CocktailName.BetweenTheSheets;

        Sweetness = 2;
        Proof = 4;
        Refreshment = 2;
    }
}

class BlueHawaii : Cocktail
{
    public BlueHawaii()
    {
        BaseMaterial = new Rum();
        subMaterials.Add(new Curacao());
        subMaterials.Add(new Pineapple());

        MaterialCode = GetMaterialCode();

        cocktailName = CocktailName.BlueHawaii;

        Sweetness = 3;
        Proof = 2;
        Refreshment = 5;
    }
}