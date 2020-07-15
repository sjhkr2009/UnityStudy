using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers
{
    protected List<Order> wishlist = new List<Order>();
    public string Name { get; protected set; }
    public Sprite image;
    public float Satisfaction { get; set; }
    public int _currentIndex;
    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (wishlist.Count == 0) _currentIndex = 0;
            else _currentIndex = (value % wishlist.Count);
        }
    }
    public Order GetOrder()
    {
        return wishlist[CurrentIndex];
    }
    protected void SetOrder(string orderContents, CocktailName? requiredCocktail = null, int? requiredProof = null, List<Define.CocktailTag> requiredTags = null)
    {
        Order order = new Order();
        order.requiredCocktail = (requiredCocktail != null) ? requiredCocktail : null;
        order.requiredProofGrade = (requiredProof != null) ? requiredProof : null;
        order.requiredTag = (requiredTags != null) ? requiredTags : null;
        order.orderContents = orderContents;
        order.CustomerName = Name;
        wishlist.Add(order);
    }
    public Customers(int imageIndex)
    {
        CurrentIndex = 0;
        image = GameManager.Resource.LoadImage(Define.ImageType.Customer, imageIndex);
    }
}

public class Eagle : Customers
{
    public Eagle() : base(1)
    {
        Name = "독수리";
        
        SetOrder("비트윈 더 시트 주세요.", CocktailName.BetweenTheSheets, requiredProof: 40);
        SetOrder("뒷맛 깔끔한 걸로 한 잔 부탁드려요.", requiredProof: 15);
    }
}

public class Dove : Customers
{
    public Dove() : base(2)
    {
        Name = "비둘기";

        SetOrder("블루 하와이! 비슷한거라도 좋아요.", CocktailName.BlueHawaii, 16);
        SetOrder("적당한걸로...", requiredProof: 20);
    }
}

public class Order
{
    public CocktailName? requiredCocktail = null;
    public int? requiredProofGrade = null;
    public List<Define.CocktailTag> requiredTag = null;
    public string orderContents = "";

    public string CustomerName { get; set; }
}