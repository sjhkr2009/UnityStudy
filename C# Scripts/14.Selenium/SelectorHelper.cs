using System.Diagnostics;
using OpenQA.Selenium;

public static class SelectorHelper {
    private const string NullValue = "N/A";
    
    private const string NameSelector = "#middle > div.h_company > div.wrap_company > h2 > a";

    private const string MarketCapSelector = "#_market_sum";
    private const string TotalStockCountSelector = "#tab_con1 > div.first > table > tbody > tr:nth-child(3) > td > em";
    private const string CurrentPriceSelector = "#content > div.section.invest_trend > div.sub_section.right > table > tbody > tr:nth-child(2) > td:nth-child(2) > em";
    private const string CurrentPerSelector = "#_per";
    private const string ExpectedPerSelector = "#_cns_per";
    private const string PbrSelector = "#_pbr";
    private const string DividendYieldSelector = "#_dvr";
    private const string SimilarCompaniesPerSelector = "#tab_con1 > div:nth-child(6) > table > tbody > tr.strong > td > em";

    private const string AnalyticsRoot = "#content > div.section.cop_analysis > div.sub_section > table > tbody";

    private const string HeaderRoot = "th > strong";
    private const string SalesRevenueRoot = "tr:nth-child(1)";
    private const string GrossProfitRoot = "tr:nth-child(2)";
    private const string NetProfitRoot = "tr:nth-child(3)";
    private const string RoeRoot = "tr:nth-child(6)";
    private const string DebtRatioRoot = "tr:nth-child(7)";
    private const string QuickRatioRoot = "tr:nth-child(8)";
    private const string ReserveRationRoot = "tr:nth-child(9)";
    private const string EpsRoot = "tr:nth-child(10)";
    private const string PerRoot = "tr:nth-child(11)";
    private const string DividendRateRoot = "tr:nth-child(15)";
    private const string DividendPayoutRatioRoot = "tr:nth-child(16)";

    private const int ExpectYearPostfixNumber = 5;
    private const int ExpectQuarterPostfixNumber = 11;
    private const int MaxSearchBeforeYear = 3;
    private const int MaxSearchBeforeQuarter = 4;
    
    private const string HeaderPostfix = "th > strong";
    private static string GetYearBeforePostfix(int beforeYearCount) {
        if (beforeYearCount < 0 || beforeYearCount > MaxSearchBeforeYear) {
            Console.WriteLine($"최대 {MaxSearchBeforeYear}년 전부터 현재까지의 자료만 조회할 수 있습니다. (조회 시도 값: {beforeYearCount}년 전)");
            beforeYearCount = Math.Clamp(beforeYearCount, 0, MaxSearchBeforeYear);
        }
        return $"td:nth-child({ExpectYearPostfixNumber - beforeYearCount})";
    }
    private static string GetQuarterBeforePostfix(int beforeQuarterCount) {
        if (beforeQuarterCount < 0 || beforeQuarterCount > MaxSearchBeforeQuarter) {
            Console.WriteLine($"최대 {MaxSearchBeforeQuarter}분기 전부터 현재까지의 자료만 조회할 수 있습니다. (조회 시도 값: {beforeQuarterCount}분기 전)");
            beforeQuarterCount = Math.Clamp(beforeQuarterCount, 0, MaxSearchBeforeQuarter);
        }
        return $"td:nth-child({ExpectQuarterPostfixNumber - beforeQuarterCount})";
    }

    private static string ToSelectorString(Header header, Value value) {
        return AnalyticsRoot
            .AddSelector(GetHeaderSelector(header))
            .AddSelector(GetValueSelector(value));
    }

    private static string ToSelectorString(Basic selected) {
        return selected switch {
            Basic.CompanyName => NameSelector,
            Basic.DividendYield => DividendYieldSelector,
            Basic.MarketCap => MarketCapSelector,
            Basic.StockCount => TotalStockCountSelector,
            Basic.Price => CurrentPriceSelector,
            Basic.PBR => PbrSelector,
            Basic.CurrentPER => CurrentPerSelector,
            Basic.ExpectedPER => ExpectedPerSelector,
            Basic.OthersPER => SimilarCompaniesPerSelector,
            _ => string.Empty
        };
    }

    public static By GetSelector(Header header, Value value) {
        return By.CssSelector(ToSelectorString(header, value));
    }
    public static By GetSelector(Basic basicInfo) {
        return By.CssSelector(ToSelectorString(basicInfo));
    }

    private static string GetHeaderSelector(Header header) {
        return header switch {
            Header.SalesRevenue => SalesRevenueRoot,
            Header.GrossProfit => GrossProfitRoot,
            Header.NetProfit => NetProfitRoot,
            Header.DebtRatio => DebtRatioRoot,
            Header.QuickRatio => QuickRatioRoot,
            Header.ReserveRation => ReserveRationRoot,
            Header.ROE => RoeRoot,
            Header.DividendRate => DividendRateRoot,
            Header.DivPayoutRatio => DividendPayoutRatioRoot,
            Header.EPS => EpsRoot,
            Header.PER => PerRoot,
            _ => string.Empty
        };
    }

    private static string GetValueSelector(Value value) {
        return value switch {
            Value.Name => HeaderPostfix,
            Value.Year3Before => GetYearBeforePostfix(3),
            Value.Year2Before => GetYearBeforePostfix(2),
            Value.Year1Before => GetYearBeforePostfix(1),
            Value.CurrentYearExpect => GetYearBeforePostfix(0),
            Value.Quarter4Before => GetQuarterBeforePostfix(4),
            Value.Quarter3Before => GetQuarterBeforePostfix(3),
            Value.Quarter2Before => GetQuarterBeforePostfix(2),
            Value.Quarter1Before => GetQuarterBeforePostfix(1),
            Value.CurrentQuarterExpect => GetQuarterBeforePostfix(0),
            _ => string.Empty
        };
    }

    private static string AddSelector(this string origin, string addTarget)
        => $"{origin} > {addTarget}";

    public enum Basic {
        CompanyName,    // 회사명
        MarketCap,      // 시가총액
        StockCount,     // 상장주식수
        Price,          // 현재 주가
        CurrentPER,     // PER
        ExpectedPER,    // 예측 PER
        PBR,            // PBR
        DividendYield,  // 배당수익률
        OthersPER       // 동일업종 PER
    }
    public enum Header {
        SalesRevenue,   // 매출액
        GrossProfit,    // 영업이익
        NetProfit,      // 당기순이익
        DebtRatio,      // 부채비율
        QuickRatio,     // 당좌비율
        ReserveRation,  // 유보율
        ROE,            // ROE (자기자본이익율)
        EPS,            // EPS (주당순이익)
        PER,            // PER (주가수익비율)
        DividendRate,   // 배당률
        DivPayoutRatio, // 배당성향
    }

    public enum Value {
        Name,
        Year3Before,
        Year2Before,
        Year1Before,
        CurrentYearExpect,
        Quarter4Before,
        Quarter3Before,
        Quarter2Before,
        Quarter1Before,
        CurrentQuarterExpect
    }

    public static string GetValueByWeb(IWebDriver driver, Basic basicInfo)
        => GetValueByWeb(driver, GetSelector(basicInfo));
    
    public static string GetValueByWeb(IWebDriver driver, Header header, Value value)
        => GetValueByWeb(driver, GetSelector(header, value));

    private static string GetValueByWeb(IWebDriver driver, By cssSelector) {
        try {
            var text = driver.FindElement(cssSelector).Text;
            return (text == NullValue) ? string.Empty : text;
        } catch (Exception e) {
            Console.WriteLine($"값을 읽는 중 에러가 발생했습니다: [{e.GetType().Name}] {e.Message}");
        }
        return string.Empty;
    }
}