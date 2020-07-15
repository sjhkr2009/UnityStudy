using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMaterials : CocktailMaterials
{
    public SubMaterials(int id) : base(MaterialType.Sub)
    {
        SetID(id);
        image = GameManager.Resource.LoadImage(Define.ImageType.Sub, id);
    }
    public SubMaterials() : base(MaterialType.Sub)
    {
        SetID(0);
    }
}

class Smt_OrangeJuice : SubMaterials
{
    public Smt_OrangeJuice() : base(1)
    {
        Name = "오렌지 주스";
        Info = "";
    }

}

class Smt_LimeJuice : SubMaterials
{
    public Smt_LimeJuice() : base(2)
    {
        Name = "라임 주스";
        Info = "";
    }
}

class Smt_LemonJuice : SubMaterials
{
    public Smt_LemonJuice() : base(3)
    {
        Name = "레몬 주스";
        Info = "";
    }
}

class Smt_GrenadineSyrup : SubMaterials
{
    public Smt_GrenadineSyrup() : base(4)
    {
        Name = "그레나딘 시럽";
        Info = "";
    }
}

class Smt_TonicWater : SubMaterials
{
    public Smt_TonicWater() : base(5)
    {
        Name = "토닉 워터";
        Info = "";
    }
}

class Smt_OrangeLiqueur : SubMaterials
{
    public Smt_OrangeLiqueur() : base(6)
    {
        Name = "오렌지 리큐어";
        Info = "";
    }
}

class Smt_SodaWater : SubMaterials
{
    public Smt_SodaWater() : base(7)
    {
        Name = "소다수";
        Info = "";
    }
}

class Smt_Amaretto : SubMaterials
{
    public Smt_Amaretto() : base(8)
    {
        Name = "아마레또";
        Info = "";
    }
}

class Smt_GingerBeer : SubMaterials
{
    public Smt_GingerBeer() : base(9)
    {
        Name = "진저 비어";
        Info = "";
    }
}
class Smt_CoffeeLiqueur : SubMaterials
{
    public Smt_CoffeeLiqueur() : base(10)
    {
        Name = "커피 리큐어";
        Info = "";
    }
}
class Smt_Vermouth : SubMaterials
{
    public Smt_Vermouth() : base(11)
    {
        Name = "베르무트";
        Info = "";
    }
}
class Smt_Mint : SubMaterials
{
    public Smt_Mint() : base(12)
    {
        Name = "민트 잎";
        Info = "";
    }
}
class Smt_Sugar : SubMaterials
{
    public Smt_Sugar() : base(13)
    {
        Name = "설탕";
        Info = "";
    }
}
class Smt_Campari : SubMaterials
{
    public Smt_Campari() : base(14)
    {
        Name = "캄파리";
        Info = "";
    }
}
class Smt_CherryLiqueur : SubMaterials
{
    public Smt_CherryLiqueur() : base(15)
    {
        Name = "체리 리큐어";
        Info = "";
    }
}
class Smt_Cola : SubMaterials
{
    public Smt_Cola() : base(16)
    {
        Name = "콜라";
        Info = "";
    }
}