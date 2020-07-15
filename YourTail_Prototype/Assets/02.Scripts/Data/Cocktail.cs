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
    public string Name_kr { get; private set; }
    public string Name_en { get; private set; }

    protected void SetName(string koreanName, string englishName)
    {
        Name_kr = koreanName;
        Name_en = englishName;
    }

    public Sprite image;

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

    public int Proof { get; protected set; }
    public List<Define.CocktailTag> Tags { get; private set; } = new List<Define.CocktailTag>();
    public string Info { get; protected set; }

    protected void AddTag(Define.CocktailTag tag) => Tags.Add(tag);
    public List<string> GetTagToString()
    {
        List<string> tagList = new List<string>();
        
        foreach (Define.CocktailTag tag in Tags)
            tagList.Add(tag.ToString());

        return tagList;
    }
    public int GetProofGrade()
    {
        if (Proof <= 5) return 0;
        else if (Proof <= 10) return 1;
        else if (Proof <= 19) return 2;
        else if (Proof <= 30) return 3;
        else return 4;
    }

    public string Id { get; private set; }
    public void SetID(int code)
    {
        Id = "C" + code.ToString();
    }
    public Cocktail(int id)
    {
        SetID(id);
        image = GameManager.Resource.LoadImage(Define.ImageType.Cocktail, id);
    }
    public Cocktail()
    {
        SetID(0);
        image = GameManager.Resource.LoadImage(Define.ImageType.Cocktail, 0);

        cocktailName = CocktailName.None;
        SetName("쓰레기", "Food Waste");

        Proof = 1;
        Info = "뭔가 잘못된 것 같다.";
    }
}

class BetweenTheSheets : Cocktail
{
    public BetweenTheSheets() : base(1)
    {
        AddBase(new Bmt_Vodka());
        AddSub(new Smt_GrenadineSyrup());

        cocktailName = CocktailName.BetweenTheSheets;
        SetName("비트윈 더 시트", "Between The Sheets");

        Proof = 40;
        AddTag(Define.CocktailTag.태그2);
        AddTag(Define.CocktailTag.태그3);
        Info = "이 술을 누군가에게 건넬 때는 한 번 더 고민해보는 게 좋다.";
    }
}

class BlueHawaii : Cocktail
{
    public BlueHawaii() : base(2)
    {
        AddBase(new Bmt_Tequilla());

        cocktailName = CocktailName.BlueHawaii;
        SetName("블루 하와이", "Blue Hawaii");

        Proof = 16;
        AddTag(Define.CocktailTag.태그3);
        Info = "하와이의 푸른 바다를 연상케 하는 청량감이 느껴진다.";
    }
}