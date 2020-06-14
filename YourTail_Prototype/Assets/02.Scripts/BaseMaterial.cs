using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMaterial : CocktailMaterials
{
    public BaseMaterial()
    {
        materialType = MaterialType.Base;
    }
    public BaseMaterial(int id) : this()
    {
        Id = id;
    }
}

class Rum : BaseMaterial
{
    public Rum()
    {
        Id = 1;
    }
}

class Brandy : BaseMaterial
{
    public Brandy()
    {
        Id = 2;
    }
}