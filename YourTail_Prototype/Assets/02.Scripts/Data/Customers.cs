using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers
{
    protected Dictionary<int, List<Order>> wishlist = new Dictionary<int, List<Order>>();
    public string Name { get; protected set; }
    public string ID { get; private set; }
    public Sprite image;
    public float Satisfaction { get; set; }
    public int Level { get; set; }
    private int _currentIndex;
    public int CurrentIndex
    {
        get => _currentIndex;
        set
        {
            if (wishlist.Count == 0) _currentIndex = 0;
            else _currentIndex = (value % wishlist.Count);
        }
    }
    protected void SetWishlist(int maxLevel)
    {
        for (int i = 1; i <= maxLevel; i++)
            wishlist.Add(i, new List<Order>());
    }
    public Order GetOrder() => wishlist[Level][CurrentIndex];
    public Order GetRandomOrder() => wishlist[Level][Random.Range(0, wishlist[Level].Count)];
    protected void SetOrder(int level, string orderContents, CocktailName? requiredCocktail = null, int? requiredProofGrade = null, List<Define.CocktailTag> requiredTags = null)
    {
        Order order = new Order();
        order.requiredCocktail = (requiredCocktail != null) ? requiredCocktail : null;
        order.requiredProofGrade = (requiredProofGrade != null) ? requiredProofGrade : null;
        order.requiredTag = (requiredTags != null) ? requiredTags : null;
        order.orderContents = orderContents;
        order.CustomerName = Name;
        if (!wishlist.ContainsKey(level)) wishlist.Add(level, new List<Order>());
        wishlist[level].Add(order);
    }
    public Customers(int index)
    {
        CurrentIndex = 0;
        Level = 1;
        ID = $"CT{index}";
        image = GameManager.Resource.LoadImage(Define.ImageType.Customer, index);
    }
}

public class Eagle : Customers
{
    public Eagle() : base(1)
    {
        Name = "머머리 독수리";
        
        SetOrder(1, "제일 독한걸로 하나 줘 보게. 요즘 술들은 술 같지가 않아.", requiredProofGrade: (int)Define.ProofGrade.매우셈);
        SetOrder(1, "알록달록한 건 됐으니께 소주 한 잔 주쇼. 왜 그 손주놈이 마트에서 토닉 뭐시기 사 와서 비스무리하게 만들더만, 그거 있잖수?", requiredCocktail: CocktailName.GinTonic);
        SetOrder(1, "마시고 취할만한 걸로 하나 주시게. 옛날 소주정도면 딱 좋겠군.", requiredProofGrade: (int)Define.ProofGrade.셈);
        SetOrder(1, "오늘은 좀 피곤하구만. 맥주같이 가벼운걸로 한 잔 하지.", requiredProofGrade: (int)Define.ProofGrade.약함);
        SetOrder(1, "요즘 젊은이들이 마시는걸로 하나 주시게나. 독하지도 맹하지도 않은... 그 왜 옛날 소주랑 맥주 중간쯤되는 시원섭섭한거 말일세. 무슨 맛인지 궁금하군.", requiredProofGrade: (int)Define.ProofGrade.중간);
    }
}

public class Goni : Customers
{
    public Goni() : base(2)
    {
        Name = "고니";

        SetOrder(1, "오렌지 블로섬 주세요.", requiredCocktail: CocktailName.OrangeBlossom);
        SetOrder(1, "드라이 마티니 주세요.", requiredCocktail: CocktailName.DryMartini);
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