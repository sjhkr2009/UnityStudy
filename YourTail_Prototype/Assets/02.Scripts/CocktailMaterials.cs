using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CocktailMaterials
{
    public int Id { get; protected set; }
    public Sprite image;
    public enum MaterialType
    {
        Base,
        Sub
    }
    public MaterialType materialType { get; protected set; }
    public CocktailMaterials()
    {
        Id = 0;
    }
}
