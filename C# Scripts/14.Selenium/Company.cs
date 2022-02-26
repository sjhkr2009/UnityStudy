
using System.Dynamic;
using OpenQA.Selenium;

[Serializable]
public class Company {
    public string CompanyName;
    public int Code;
    
    public int? MarketCap;
    public float? Per;
    public float? ExpectedPer;
    public float? Pbr;
    public float? DividendRate;

    public int WarningPoint;
    public int RecommendPoint;

    [NonSerialized] private const int YearInfoCount = 4;
    [NonSerialized] private const int QuarterInfoCount = 5;

    public Performance[] YearPerformances = new Performance[YearInfoCount];
    public Performance[] QuarterPerformances = new Performance[QuarterInfoCount];

    private Company() { }

    public static Company Get(IWebDriver driver, int code) {
        var ret = new Company();
        
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        
        // 해당 Url로 이동한다.
        driver.Url = UrlHelper.GetUrl(code);
        Console.WriteLine($"URL: {driver.Url}");
        
        // Url 로딩 후에는 페이지의 요소를 찾는 시간을 0.1초로 제한한다.
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1f);

        ret.Code = code;
        ret.CompanyName = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.CompanyName);
        
        string marketCapString = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.MarketCap)
            .Replace("조", "").Replace(",", "").Replace(" ", "");
        ret.MarketCap = int.TryParse(marketCapString, out var marketCap) ? marketCap : null;
        ret.Per = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.CurrentPER), out var per) ? per : null;
        ret.ExpectedPer = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.ExpectedPER), out var ePer) ? ePer : ret.Per;
        ret.Pbr = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.PBR), out var pbr) ? pbr : null;
        ret.DividendRate = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.DividendYield), out var rate) ? rate : null;
        
        var headers = Enum.GetValues<SelectorHelper.Header>();
        var yearValues = new SelectorHelper.Value[YearInfoCount] {
            SelectorHelper.Value.Year3Before,
            SelectorHelper.Value.Year2Before,
            SelectorHelper.Value.Year1Before,
            SelectorHelper.Value.CurrentYearExpect
        };
        var quarterValues = new SelectorHelper.Value[QuarterInfoCount] {
            SelectorHelper.Value.Quarter4Before,
            SelectorHelper.Value.Quarter3Before,
            SelectorHelper.Value.Quarter2Before,
            SelectorHelper.Value.Quarter1Before,
            SelectorHelper.Value.CurrentQuarterExpect
        };

        for (int i = 0; i < YearInfoCount; i++) {
            ret.YearPerformances[i] = Performance.Get(driver, yearValues[i]);
        }

        for (int i = 0; i < QuarterInfoCount; i++) {
            ret.QuarterPerformances[i] = Performance.Get(driver, quarterValues[i]);
        }
        
        return ret;
    }

    [Serializable]
    public class Performance {
        public int? SalesRevenue;
        public int? GrossProfit;
        public int? NetProfit;
        
        public float? Roe;
        public float? DebtRatio;
        public float? QuickRatio;
        public float? ReserveRation;
        
        private Performance(){}

        internal static Performance Get(IWebDriver driver, SelectorHelper.Value valueIndex) {
            var ret = new Performance {
                SalesRevenue = int.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.SalesRevenue, valueIndex), out var cap) ? cap : null,
                GrossProfit = int.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.GrossProfit, valueIndex), out var gross) ? gross : null,
                NetProfit = int.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.NetProfit, valueIndex), out var net) ? net : null,
                Roe = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.ROE, valueIndex), out var roe) ? roe : null,
                DebtRatio = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.DebtRatio, valueIndex), out var debt) ? debt : null,
                QuickRatio = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.QuickRatio, valueIndex), out var quick) ? quick : null,
                ReserveRation = float.TryParse(SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.ReserveRation, valueIndex), out var rr) ? rr : null
            };

            return ret;
        }
    }
}