using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customers
{
    #region 기본 정보
    public string Name { get; protected set; }
    public string ID { get; private set; }
    public Define.CustomerType CustomerType { get; private set; }
    public Sprite Image { get; set; }
    public bool IsActive { get; set; }
    #endregion

    #region 호감도
    private int _level = 1;
    public int Level
    {
        get => _level;
        set { _level = Mathf.Clamp(value, 1, MaxLevel); }
    }
    public int MaxLevel { get; private set; }
    protected void SetMaxLevel(int value)
    {
        MaxLevel = Mathf.Clamp(value, 1, Define.MaxLevel);
        Level = 1;
        Exp = 0;
    }
    private int _exp;
    public int Exp
    {
        get => _exp;
        set
        {
            if(value > Define.RequiredEXP[_level])
            {
                Level++;
                GameManager.Data.levelUp = true;
                Exp = (value - Define.RequiredEXP[_level]);
            }
            else
            {
                _exp = value;
            }
        }
    }
    #endregion

    #region 오더 관련
    protected Dictionary<int, List<Order>> wishlist = new Dictionary<int, List<Order>>();
    private int _currentOrderIndex;
    public int CurrentOrderIndex
    {
        get => _currentOrderIndex;
        set
        {
            if (value == 0 || wishlist[Level].Count == 0) _currentOrderIndex = 0;
            else _currentOrderIndex = (value % wishlist[Level].Count);
        }
    }
    protected void SetWishlist(int maxLevel)
    {
        for (int i = 1; i <= maxLevel; i++)
            wishlist.Add(i, new List<Order>());
    }
    private Order currentOrder = null;
    public Order GetOrder()
    {
        if (currentOrder == null)
            currentOrder = wishlist[Level][CurrentOrderIndex++];

        return currentOrder;
    }
    public Order GetRandomOrder(int level = int.MaxValue)
    {
        if (currentOrder == null)
            currentOrder = wishlist[Mathf.Min(level, Level)][Random.Range(0, wishlist[Level].Count)];

        return currentOrder;
    }
    public Order GetOrderInLow(params float[] rateToCurrent)
    {
        int targetLevel = Mathf.Max(1, Level - rateToCurrent.RandomInWeighted());
        return GetRandomOrder(targetLevel);
    }
    public void ResetOrder()
    {
        currentOrder = null;
    }
    protected void SetOrder(int level, string orderContents, CocktailName? requiredCocktail = null, 
        List<Define.ProofGrade> requiredProofGrade = null, List<Define.CocktailTag> requiredTags = null,
        List<Define.ProofGrade> avoidProofGrade = null, List<Define.CocktailTag> avoidTag = null)
    {
        Order order = new Order();
        order.requiredCocktail = (requiredCocktail != null) ? requiredCocktail : null;
        order.requiredProofGrade = requiredProofGrade;
        order.requiredTag = requiredTags;
        order.avoidProofGrade = avoidProofGrade;
        order.orderContents = orderContents;
        order.avoidTag = avoidTag;
        order.CustomerName = Name;

        if (!wishlist.ContainsKey(level)) wishlist.Add(level, new List<Order>());
        wishlist[level].Add(order);
    }
    #endregion

    #region 기타 스크립트
    public Dictionary<Define.Reaction, string> reactionSctipt { get; private set; } = new Dictionary<Define.Reaction, string>();
    protected void SetReactionSctipt(string good, string soso, string bad)
    {
        reactionSctipt.Add(Define.Reaction.GOOD, good);
        reactionSctipt.Add(Define.Reaction.SOSO, soso);
        reactionSctipt.Add(Define.Reaction.BAD, bad);
    }
    public string Liking { get; protected set; }

    #endregion

    public Customers(int index)
    {
        CurrentOrderIndex = 0;
        IsActive = true;
        CustomerType = (Define.CustomerType)index;
        ID = $"CT{index}";
        Image = GameManager.Resource.LoadImage(Define.ImageType.Customer, index);
        Level = 1;
        SetMaxLevel(Define.BirdMaxLevel[index]);
        Exp = 0;
    }
}

public class Eagle : Customers
{
    public Eagle() : base(1)
    {
        Name = "머머리 독수리";
        Liking = "(임시) 꼰-머";

        SetOrder(1, "제일 독한걸로 하나 줘 보게. 요즘 술들은 술 같지가 않아.", requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.매우셈 });
        SetOrder(1, "알록달록한 건 됐으니께 소주 한 잔 주쇼. 왜 그 손주놈이 마트에서 토닉 뭐시기 사 와서 비스무리하게 만들더만, 그거 있잖수?", requiredCocktail: CocktailName.GinTonic);
        SetOrder(1, "마시고 취할만한 걸로 하나 주시게. 옛날 소주정도면 딱 좋겠군.", requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.셈 });
        SetOrder(1, "오늘은 좀 피곤하구만. 맥주같이 가벼운걸로 한 잔 하지.", requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함 });
        SetOrder(1, "요즘 젊은이들이 마시는걸로 하나 주시게나. 독하지도 맹하지도 않은... 그 왜 옛날 소주랑 맥주 중간쯤되는 시원섭섭한거 말일세. 무슨 맛인지 궁금하군.", requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.중간 });

        SetReactionSctipt("자네..실력이 대단하군..그려...", "오호..이것도 먹을만 하군....", "으른을...놀리는겐가...?");
    }
}

public class Parrot : Customers
{
    public Parrot() : base(2)
    {
        Name = "앵무새";
        Liking = "(임시) 추상적 표현";

        SetOrder(1, "오렌지 나무에서 나는 향이 맡고 싶어요.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.오렌지, Define.CocktailTag.주스 } );
        SetOrder(1, "키스 인 더 다크'라는 칵테일이 있다던데. 이름이 참 예쁘지 않아요?", requiredCocktail: CocktailName.KissInTheDark);
        SetOrder(1, "제 깃털처럼 새빨간 과일이 들어간 걸로 한 잔 주세요.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.진, Define.CocktailTag.석류 });
        SetOrder(1, "입이 좀 텁텁한데 청량감 있는 칵테일은 없을까요? 톡 쏘는 맛이라든가.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.탄산 });
        SetOrder(1, "그 왜... 레몬이랑 비슷한 거... 갑자기 이름이 기억이 안 나네요. 그게 들어간 칵테일이 마시고 싶어요.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.라임, Define.CocktailTag.주스 });

        SetReactionSctipt("음- 너무 맛있어요!", "뭔가 살짝 아쉬운데.. 그래도 고마워요.", "제가 찾던 건 아니네요.");
    }
}

public class Flamingo : Customers
{
    public Flamingo() : base(3)
    {
        Name = "홍학";
        Liking = "(임시) 반말함";

        SetOrder(1, "아직 기획이 안 되었으니 오렌지나 내놓으세요.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.오렌지 });
        SetOrder(2, "아직 기획이 안 되었으니 오렌지나 내놓으라니까요?.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.오렌지 });

        SetReactionSctipt("우왕 너무 맛있다 ㅎㅎ", "나쁘지 않은걸~?!", "흐음..이건 뭐지?");
    }
}

public class Swan : Customers
{
    public Swan() : base(4)
    {
        Name = "고니";
        Liking = "(임시) 고상함";

        SetOrder(1, "아직 기획이 안 되었으니 레몬이나 내놓으세요.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.레몬 });
        SetOrder(2, "아직 기획이 안 되었으니 레몬이나 내놓으라니까요?", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.레몬 });

        SetReactionSctipt("맛이 좋은걸요~!^^", "이것도 나름 괜찮네요~!", "다시 만들어 주면 좋겠어요..ㅠ");
    }
}

public class Penguin : Customers
{
    public Penguin() : base(5)
    {
        Name = "펭귄";
        Liking = "(임시) 민증검사 필수";

        SetOrder(1, "댁길라'라는 술이 들어간 칵테일이 맛있다고 들었는데... 그게 먹어보고 싶어요오. 약하게 해주세요오.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.데킬라 }, requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함 });
        SetOrder(1, "과일 들어간거면 아무거나 괜찮을거 같아요오.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 });
        SetOrder(1, "과일 들어간거요오! 컨디션은 애매하니 좀 약한걸로...", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 }, requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.약함 });
        SetOrder(1, "앱에이숑'이라는 칵테일이 있다고 들었는데 그게 먹어보고 싶어요오!", requiredCocktail: CocktailName.Aviation);
        SetOrder(1, "과일 들어간 적당한 칵테일이 마시고 싶어요오.", requiredTags: new List<Define.CocktailTag>() { Define.CocktailTag.과일 }, requiredProofGrade: new List<Define.ProofGrade>() { Define.ProofGrade.중간 });

        SetReactionSctipt("와아, 제가 원하던 그 맛이었어요오!", "맛이 괜찮네요오!", "맛없어요오...");
    }
}

public class Order
{
    public CocktailName? requiredCocktail = null;
    public List<Define.ProofGrade> requiredProofGrade = null;
    public List<Define.CocktailTag> requiredTag = null;
    public List<Define.ProofGrade> avoidProofGrade = null;
    public List<Define.CocktailTag> avoidTag = null;
    public string orderContents = "";


    public string CustomerName { get; set; }
}