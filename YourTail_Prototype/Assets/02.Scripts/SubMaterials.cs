using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMaterials : CocktailMaterials
{
    public SubMaterials()
    {
        materialType = MaterialType.Sub;
    }
    public SubMaterials(int id) : this()
    {
        Id = id;
    }
}

class Curacao : SubMaterials
{
    public Curacao()
    {
        Id = 1;
    }
}

class Pineapple : SubMaterials
{
    public Pineapple()
    {
        Id = 2;
    }
}

class Lime : SubMaterials
{
    public Lime()
    {
        Id = 3;
    }
}

class Lemon : SubMaterials
{
    public Lemon()
    {
        Id = 4;
    }
}