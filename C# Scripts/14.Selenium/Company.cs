using System.Dynamic;
using System.Globalization;
using OpenQA.Selenium;
using static System.Globalization.NumberStyles;

[Serializable]
public class Company {
    public DateTime LastUpdateTime;
    
    public string CompanyName;
    public int Code;
    
    public int? MarketCap;
    public long? TotalStockCount;
    public int? CurrentPrice;
    public float? Per;
    public float? ExpectedPer;
    public float? Pbr;
    public float? DividendRate;
    public float? SimilarCompanyPer;

    public int WarningPoint;
    public int RecommendPoint;

    [NonSerialized] private const int YearInfoCount = 4;
    [NonSerialized] private const int QuarterInfoCount = 5;

    public Performance[] YearPerformances = new Performance[YearInfoCount];
    public Performance[] QuarterPerformances = new Performance[QuarterInfoCount];

    private Company() { }

    public static Company CreateFromWeb(IWebDriver driver, int code) {
        var ret = new Company();
        
        driver.GoTo(code);
        
        ret.LastUpdateTime = DateTime.Now;

        ret.Code = code;
        ret.CompanyName = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.CompanyName);
        
        string marketCapString = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.MarketCap)
            .Replace("조", "").Replace(",", "").Replace(" ", "");
        ret.MarketCap = marketCapString.ToInt32();
        ret.TotalStockCount = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.StockCount).ToInt64();
        ret.CurrentPrice = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.Price).ToInt32();

        ret.Per = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.CurrentPER).ToFloat();
        ret.ExpectedPer = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.ExpectedPER).ToFloat();
        ret.Pbr = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.PBR).ToFloat();
        ret.DividendRate = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.DividendYield).ToFloat();
        ret.SimilarCompanyPer = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Basic.OthersPER).ToFloat(); 
        
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
        
        public int? Eps;
        public float? Per;
        public float? DividendRate;
        public float? DividendPayoutRatio;
        
        private Performance(){}

        internal static Performance Get(IWebDriver driver, SelectorHelper.Value valueIndex) {
            var ret = new Performance {
                SalesRevenue = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.SalesRevenue, valueIndex).ToInt32(),
                GrossProfit = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.GrossProfit, valueIndex).ToInt32(),
                NetProfit = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.NetProfit, valueIndex).ToInt32(),
                Roe = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.ROE, valueIndex).ToFloat(),
                DebtRatio = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.DebtRatio, valueIndex).ToFloat(),
                QuickRatio = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.QuickRatio, valueIndex).ToFloat(),
                ReserveRation = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.ReserveRation, valueIndex).ToFloat(),
                Eps = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.EPS, valueIndex).ToInt32(),
                Per = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.PER, valueIndex).ToFloat(),
                DividendRate = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.DividendRate, valueIndex).ToFloat(),
                DividendPayoutRatio = SelectorHelper.GetValueByWeb(driver, SelectorHelper.Header.DivPayoutRatio, valueIndex).ToFloat(),
            };

            return ret;
        }
    }
}