using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CocktailMaterials
{
    public string Name { get; set; }
    public string Id { get; private set; }
    public string Info { get; protected set; }
    public int Index { get; private set; }
    public void SetID(int code)
    {
        Index = code;
        if (materialType == MaterialType.Base)
            Id = "B" + code.ToString();
        else if (materialType == MaterialType.Sub)
            Id = "S" + code.ToString();
    }

    public List<Define.CocktailTag> Tags { get; protected set; } = new List<Define.CocktailTag>();

    public Sprite image;
    public enum MaterialType
    {
        Base,
        Sub
    }
    public MaterialType materialType { get; protected set; }
    public CocktailMaterials(MaterialType type)
    {
        materialType = type;
        Name = "공기";
    }
}
