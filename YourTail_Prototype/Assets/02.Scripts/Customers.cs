using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers
{
    protected List<Order> wishlist = new List<Order>();
    public float Satisfaction { get; set; }
    public int currentIndex = 0;
    public Order GetOrder()
    {
        return wishlist[currentIndex];
    }
    protected void SetOrder(string orderContents, CocktailName? requiredCocktail = null, int? requiredSweet = null, int? requiredProof = null, int? requiredFresh = null)
    {
        Order order = new Order();
        order.requiredCocktail = (requiredCocktail != null) ? requiredCocktail : null;
        order.requiredSweet = (requiredSweet != null) ? requiredSweet : null;
        order.requiredProof = (requiredProof != null) ? requiredProof : null;
        order.requiredFresh = (requiredFresh != null) ? requiredFresh : null;
        order.orderContents = orderContents;
        wishlist.Add(order);
    }
}

public class Eagle : Customers
{
    public Eagle()
    {
        SetOrder("비트윈 더 시트 주세요.", CocktailName.BetweenTheSheets, requiredProof: 4);
        SetOrder("뒷맛 깔끔한 걸로 한 잔 부탁드려요.", requiredFresh: 4);
    }
}

public class Order
{
    public CocktailName? requiredCocktail = null;
    public int? requiredSweet = null;
    public int? requiredProof = null;
    public int? requiredFresh = null;
    public string orderContents = "";
}