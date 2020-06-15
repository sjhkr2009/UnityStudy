using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocktailManager : MonoBehaviour
{
    public Dictionary<string, BaseMaterials> BaseMaterialData { get; private set; } = new Dictionary<string, BaseMaterials>();
    public Dictionary<string, SubMaterials> SubMaterialData { get; private set; } = new Dictionary<string, SubMaterials>();
    public Dictionary<string, Cocktail> CocktailData { get; private set; } = new Dictionary<string, Cocktail>();
    List<Cocktail> CocktailList = new List<Cocktail>();

    void Start()
    {
        Init();
    }

    void Init()
    {
        if (BaseMaterialData.Count > 0) BaseMaterialData.Clear();
        if (SubMaterialData.Count > 0) SubMaterialData.Clear();
        if (CocktailData.Count > 0) CocktailData.Clear();
        if (CocktailList.Count > 0) CocktailList.Clear();

        AddBase(new Rum());
        AddBase(new Brandy());

        AddSub(new Curacao());
        AddSub(new Pineapple());
        AddSub(new Lime());
        AddSub(new Lemon());

        AddCocktail(new BetweenTheSheets());
        AddCocktail(new BlueHawaii());
    }

    void AddBase(BaseMaterials item)
    {
        string id = item.Id;
        BaseMaterialData.Add(id, item);
    }
    void AddSub(SubMaterials item)
    {
        string id = item.Id;
        SubMaterialData.Add(id, item);
    }
    void AddCocktail(Cocktail item)
    {
        string id = item.Id;
        CocktailData.Add(id, item);
        CocktailList.Add(item);
    }
    public Cocktail MakeCocktail(List<BaseMaterials> currentBases, List<SubMaterials> currentSubs)
    {
        Cocktail empty = new Cocktail();

        foreach (Cocktail cocktail in CocktailList)
        {
            bool isCorrect = true;
            if (currentBases.Count != cocktail.BaseMaterials.Count) continue;
            if (currentSubs.Count != cocktail.SubMaterials.Count) continue;

            foreach (BaseMaterials _base in currentBases)
            {
                if (!cocktail.BaseIDList.Contains(_base.Id))
                {
                    isCorrect = false;
                    break;
                }
            }
            
            if (!isCorrect) continue;

            foreach (SubMaterials _sub in currentSubs)
            {
                if (!cocktail.SubIDList.Contains(_sub.Id))
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect) return cocktail;
        }

        return empty;
    }
}
