using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMaterials : CocktailMaterials
{
    public SubMaterials(int id) : base(MaterialType.Sub)
    {
        SetID(id);
    }
    public SubMaterials() : base(MaterialType.Sub)
    {
        SetID(0);
    }
}

class Curacao : SubMaterials
{
    public Curacao() : base(1)
    {
        Name = "큐라소";
    }

}

class Pineapple : SubMaterials
{
    public Pineapple() : base(2)
    {
        Name = "파인애플";
    }
}

class Lime : SubMaterials
{
    public Lime() : base(3)
    {
        Name = "라임";
    }
}

class Lemon : SubMaterials
{
    public Lemon() : base(4)
    {
        Name = "레몬";
    }
}