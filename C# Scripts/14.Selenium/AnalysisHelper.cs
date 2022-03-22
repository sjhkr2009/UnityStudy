using System.Text;

public static class AnalysisHelper {
    public static StringBuilder Logger { get; private set; } = new StringBuilder();

    public static void Initialize(StringBuilder logger) {
        Logger = logger;
    }

    public static Company AddPenalty(this Company company, int point, string reason) {
        string msg = $"-{point} | {reason}";
        Logger?.AppendLine(msg);
        company.WarningPoint += point;
        return company;
    }
    
    public static Company AddRecommend(this Company company, int point, string reason) {
        string msg = $"+{point} | {reason}";
        Logger?.AppendLine(msg);
        company.RecommendPoint += point;
        return company;
    }
    
    public static Company Clear(this Company company) {
        company.WarningPoint = 0;
        company.RecommendPoint = 0;
        return company;
    }

    public static Company AnalyzeMarketCap(this Company company) {
        switch (company.MarketCap) {
            case null:
                return company.AddPenalty(1000, "시가총액 정보 없음");
            case < 1000:
                int point = (int)Math.Sqrt(1000 - (int) company.MarketCap); 
                return company.AddPenalty(point, $"시가총액 낮음 ({company.MarketCap}억)");
            case > 10000:
                return company.AddRecommend(10, $"시가총액 높음: {company.MarketCap / 10000}조 {company.MarketCap % 10000}억");
        }

        return company;
    }

    public static Company AnalyzePer(this Company company) {
        if (company.Per == null) return company.AddPenalty(20, "PER 정보 없음 (당기순손실)");

        return company.SimilarCompanyPer == null ? company.AnalyzePerAbsolute() : company.AnalyzePerRelative();
    }

    private static Company AnalyzePerRelative(this Company company) {
        if (company.Per == null || company.Per == 0 || company.SimilarCompanyPer == null) return company;
        if (company.SimilarCompanyPer < 1f || company.SimilarCompanyPer > 30f) return company.AnalyzePerAbsolute();

        float per = (float)company.Per;
        float otherPer = (float)company.SimilarCompanyPer;
        int point = (int)per.CalculateDiffRate(otherPer, 10f, 30f);

        return per > otherPer
            ? company.AddPenalty(point, $"PER 높음: {company.Per:0.0} > 동종업계 평균 {company.SimilarCompanyPer:0.0}")
            : company.AddRecommend(point, $"PER 낮음: {company.Per:0.0} < 동종업계 평균 {company.SimilarCompanyPer:0.0}");
    }

    private static Company AnalyzePerAbsolute(this Company company) {
        return company.Per switch {
            > 30f => company.AddPenalty(20, $"PER 높음: {company.Per:0.0}"),
            < 10f => company.AddRecommend(20, $"PER 낮음: {company.Per:0.0}"),
            _ => company
        };
    }

    public static Company AnalyzeExpectedPer(this Company company) {
        if (company.ExpectedPer == null) return company;

        return company.Per == null ? company.AnalyzeExpectedPerAbsolute() : company.AnalyzeExpectedPerRelative();
    }

    private static Company AnalyzeExpectedPerRelative(this Company company) {
        if (company.ExpectedPer == null || company.Per == null) return company;
        
        float per = (float)company.Per;
        float expectPer = (float)company.ExpectedPer;
        int point = (int)per.CalculateDiffRate(expectPer, 10f, 20f);

        return per > expectPer
            ? company.AddRecommend(point, $"PER 개선: {company.Per:0.0} -> {company.ExpectedPer:0.0}")
            : company.AddPenalty(point, $"PER 악화: {company.Per:0.0} -> {company.ExpectedPer:0.0}");
    }
    
    private static Company AnalyzeExpectedPerAbsolute(this Company company) {
        return company.ExpectedPer switch {
            > 30f => company.AddPenalty(20, $"미래 PER 높음: {company.Per:0.0}"),
            < 10f => company.AddRecommend(20, $"미래 PER 낮음: {company.Per:0.0}"),
            _ => company
        };
    }

    public static Company AnalyzePbr(this Company company) {
        return company.Pbr switch {
            null => company.AddPenalty(10, "PBR 정보 없음"),
            > 3f => company.AddPenalty((int)company.Pbr.CalculateDiffRate(3f, 5f, 10f), $"PBR 높음 ({company.Pbr:0.0}배)"),
            < 1f => company.AddRecommend((int)company.Pbr.CalculateDiffRate(1f, 5f, 10f), $"PBR 낮음 ({company.Pbr:0.0}배)"),
            _ => company
        };
    }

    public static Company AnalyzeDividendRate(this Company company) {
        if (company.DividendRate is > 2f) {
            int point = (int)company.DividendRate.CalculateDiffRate(2f, 5f, 20f, 10f);
            return company.AddRecommend(point, $"시가배당률 높음: {company.DividendRate}");
        }
        
        return company;
    }
    
    public static Company AnalyzeYearPerformance(this Company company) {
        var prev = company.YearPerformances[0];
		for (int i = 0; i < company.YearPerformances.Length; i++) {
			var cur = company.YearPerformances[i];

			bool isExpected = i == company.YearPerformances.Length - 1;
			string when = isExpected ? "[올해 예상]" : $"[{company.YearPerformances.Length - 1 - i}년 전]";
			
			if (cur.NetProfit is < 0) {
				company.AddPenalty(10 * (i + 1), when + $"당기순손실 ({cur.NetProfit}억)");
			} else if (cur.GrossProfit is < 0) {
				company.AddPenalty(10 * (i + 1), when + $"영업손실 ({cur.GrossProfit}억)");
			}

			if (i > 0 && cur.GrossProfit is > 0 && prev.GrossProfit != null) {
				if (cur.GrossProfit > prev.GrossProfit) {
					int curProfit = (int)cur.GrossProfit;
					int prevProfit = Math.Max(0, (int)prev.GrossProfit);
					if (prevProfit != 0) {
						float increaseRate = ((float)curProfit / prevProfit - 1f) * 100f;
						company.AddRecommend(Math.Clamp((int)increaseRate, 10, 10 + 10 * i), when + $"영업이익 {increaseRate:0.0}% 증가 ({prev.GrossProfit} -> {cur.GrossProfit})");
					} else {
						company.AddRecommend(10, when + $"흑자 전환");
					}
				}

				if (cur.GrossProfit < prev.GrossProfit) {
					float curProfit = (float)cur.GrossProfit;
					float prevProfit = (float)prev.GrossProfit;
					float decreaseRate = (1f - (curProfit / prevProfit)) * 100f;
					company.AddPenalty(Math.Min((int)(decreaseRate * 0.5f), 10 + 10 * i), when + $"영업이익 {decreaseRate:0.0}% 감소: {prev.GrossProfit} -> {cur.GrossProfit}");
				}

				prev = cur;
			}
			
			if (i > 0 && cur.Per is > 1f && prev.Per is > 1f) {
				float max = cur.Per < 10f ? 10f : 5f;
				max += i * (cur.Per < 10f ? 5f : 2f);
				if (company.SimilarCompanyPer is > 0f && cur.Per < company.SimilarCompanyPer) {
					company.AddRecommend((int)cur.Per.CalculateDiffRate((float) company.SimilarCompanyPer, 10f, max), when + $"PER 낮음 ({cur.Per})");
				}
				if (cur.Per < prev.Per) {
					company.AddRecommend((int)cur.Per.CalculateDiffRate((float) prev.Per, 20f, max), when + $"PER 감소 ({prev.Per} -> {cur.Per})");
				}

				if (cur.GrossProfit < prev.GrossProfit) {
					float curProfit = (float)cur.GrossProfit;
					float prevProfit = (float)prev.GrossProfit;
					float decreaseRate = (1f - (curProfit / prevProfit)) * 100f;
					company.AddPenalty(Math.Min((int)(decreaseRate * 0.5f), 20), when + $"영업이익 {decreaseRate:0.0}% 감소: {prev.GrossProfit} -> {cur.GrossProfit}");
				}

				prev = cur;
			}

			if (i >= company.YearPerformances.Length - 2) {
				if (cur.DebtRatio is > 100f) {
					company.AddPenalty(10, when + $"부채율 높음 ({cur.DebtRatio:0.0}%)");
				}
			
				if (cur.QuickRatio is < 100f) {
					company.AddPenalty(10, when + $"당좌비율 낮음 ({cur.QuickRatio:0.0}%)");
				} else if (cur.QuickRatio > 200f) {
					company.AddRecommend(10, when + $"당좌비율 높음 ({cur.QuickRatio:0.0}%)");
				}
			
				if (cur.ReserveRation is < 500f) {
					company.AddPenalty(5, when + $"유보율 낮음 ({cur.ReserveRation:0.0}%)");
				} else if (cur.ReserveRation is > 2000f) {
					company.AddRecommend(5, when + $"유보율 높음 ({cur.ReserveRation:0.0}%)");
				}
			}
		}

		return company;
    }

    public static Company AnalysisQuarterPerformance(this Company company) {
	    for (int i = 0; i < company.QuarterPerformances.Length; i++) {
		    var cur = company.QuarterPerformances[i];
			
		    bool isExpected = i == company.QuarterPerformances.Length - 1;
		    string when = isExpected ? "[다음분기 예상]" : $"[{company.QuarterPerformances.Length - 1 - i}분기 전]";
		    if (cur.NetProfit is < 0) {
			    company.AddPenalty(10, when + $"당기순손실 ({cur.NetProfit}억)");
		    }
	    }

	    return company;
    }

    public static Company AnalysisAll(this Company company) {
	    Logger?.AppendLine($"{company.CompanyName}({company.Code:000000})에 대한 분석을 시작합니다.");
	    company.WarningPoint = 0;
	    company.RecommendPoint = 0;
	    return company.AnalyzeMarketCap()
		    .AnalyzePer()
		    .AnalyzeExpectedPer()
		    .AnalyzePbr()
		    .AnalyzeDividendRate()
		    .AnalyzeYearPerformance()
		    .AnalysisQuarterPerformance();
    }
}