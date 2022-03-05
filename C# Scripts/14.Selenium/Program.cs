using System;
using System.Collections.Generic;
using System.Threading;	
using OpenQA.Selenium.Chrome;	
using OpenQA.Selenium;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using Newtonsoft.Json;

// 실행 파일 위치에 현재 설치된 크롬 버전에 맞는 chromedriver.exe 필요

static class Program {
	private static StringBuilder log = new StringBuilder();
	private static StringBuilder output = new StringBuilder();
	private const string SavePath = @"C:\Users\서지호\Desktop\NaverFinanceOutput";
	
	private static readonly int[] AlreadyAnalysis = new int[] { };

	private static readonly List<Company> Companies = new List<Company>();

	private static IWebDriver _driver;

	static async Task Main(string[] args) {
		//await AnalysisAllFromWeb();
		await AnalysisAllFromJson();

		ExtractLogToFile();
	}
	
	/// <summary>
	/// json 파일의 모든 기업을 Companies 배열에 Deserialize하고, 평가를 초기화한 후 분석하여 점수를 재산정합니다. 
	/// </summary>
	static async Task AnalysisAllFromJson() {
		var json = await File.ReadAllTextAsync($"{SavePath}_result.json");
		List<Company> targets = JsonConvert.DeserializeObject<List<Company>>(json);

		foreach (var company in targets) {
			log.AppendLine($"{company.CompanyName}({company.Code:000000})에 대한 분석을 시작합니다.");
			company.WarningPoint = 0;
			company.RecommendPoint = 0;
			Analysis(company);
			log.AppendLine();
		}
	}
	
	/// <summary>
	/// 배열 내 모든 종목을 네이버 금융에서 찾아 분석하고 Companies 리스트에 저장합니다.
	/// </summary>
	static async Task AnalysisAllFromWeb() {
		Companies.Clear();
		
		using (IWebDriver driver = new ChromeDriver()) {
			// 대기 설정. (find로 객체를 찾을 때까지 검색이 되지 않으면 대기하는 시간, 초단위)
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

			var targets = AnalysisTargetData.AllListedCompanies.TargetCodes;
			
			int logCount = 0;
			foreach (var testTarget in targets) {
				if (AlreadyAnalysis.Contains(testTarget)) continue;
				
				log.AppendLine($"{testTarget}에 대한 분석을 시작합니다.");
				try {
					driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
					var company = Company.Get(driver, testTarget);
					Analysis(company);
				} catch (Exception e) {
					log.AppendLine($"{testTarget}을 분석하던 중 오류가 발생했습니다.");
					log.AppendLine($"[{e.GetType().Name}] {e.Message} / {e.StackTrace}");
				}
				
				log.AppendLine();

				logCount++;
				if(logCount > 10) {
					log.AppendLine("로그 저장됨");
					_ = File.WriteAllTextAsync($"{SavePath}_log.txt", log.ToString());
					log.AppendLine();
					logCount = 0;
				}
			}
		}
		
		// 창을 닫고 로그의 중간저장(WriteAllTextAsync)이 끝날 때까지 잠시 대기합니다.
		await Task.Delay(500);
	}
	
	/// <summary>
	/// Companies의 종목들을 json 파일로 저장하고, 높은 점수 순으로 정렬한 output 및 로그를 저장합니다.
	/// </summary>
	static void ExtractLogToFile() {
		var resultJson = JsonConvert.SerializeObject(Companies, Formatting.Indented);

		Companies.Sort((c1, c2) => {
			int score1 = c1.RecommendPoint - c1.WarningPoint;
			int score2 = c2.RecommendPoint - c2.WarningPoint;
			return score2 - score1;
		});

		for (int i = 0; i < Companies.Count; i++) {
			var company = Companies[i];
			output.AppendLine($"[{i + 1}위] {company.CompanyName} ({company.Code:000000}) : " +
			                  $"{(company.RecommendPoint - company.WarningPoint)}점 " +
			                  $"({company.RecommendPoint} - {company.WarningPoint})");
		}
		
		File.WriteAllText($"{SavePath}.txt", output.ToString());
		File.WriteAllText($"{SavePath}_log.txt", log.ToString());
		File.WriteAllText($"{SavePath}_result.json", resultJson);
		
		Console.WriteLine("저장이 완료되었습니다.");
	}
	
	
	/// <summary>
	/// 입력한 company를 분석하여 점수를 산정하고 Companies 리스트에 추가합니다.
	/// TODO: 점수 산정 방식이 복잡해질 경우 항목별로 함수를 나누거나, 분석을 하는 클래스를 별도로 만들 것
	/// </summary>
	static void Analysis(Company company) {
		ConvertToNullable(company);
		
		//시가총액 500억 미만
		if (company.MarketCap == null) {
			AddPenalty("시가총액 정보 없음");
			company.WarningPoint += 1000;
		} else if (company.MarketCap < 1000) {
			AddPenalty($"시가총액 낮음 ({company.MarketCap}억)");
			company.WarningPoint += company.MarketCap < 1000 ? 50 : 20;
		} else if (company.MarketCap > 10000) {
			AddRecommend($"시가총액 높음: {company.MarketCap / 10000}조 {company.MarketCap % 10000}억");
			company.RecommendPoint += 50;
		}
		
		// PER / 미래 PER 예상치 30 이상
		if (company.Per is null or > 30f) {
			AddPenalty(company.Per != null ? $"PER 높음 ({company.Per:0.0}배)" : "PER 정보 없음");
			company.WarningPoint += 20;
		} else if (company.Per is < 15f && company.SimilarCompanyPer == null && company.Per < company.SimilarCompanyPer) {
			AddRecommend($"PER 낮음 ({company.Per:0.0}배 / 동종업계 {company.SimilarCompanyPer:0.0}배)");
			company.RecommendPoint += Math.Min((int)(10f * company.Per / company.SimilarCompanyPer), 20);
		}
		if (company.ExpectedPer is > 30f) {
			AddPenalty(company.ExpectedPer > 0f ? $"예측 PER 높음 ({company.ExpectedPer:0.0}배)" : "예상 PER 정보 없음");
			company.WarningPoint += 30;
		} else if (company.ExpectedPer != null && company.Per != null && company.ExpectedPer < company.Per) {
			AddRecommend($"PER 개선 예측 ({company.Per:0.0}배 -> {company.ExpectedPer:0.0}배)");
			company.RecommendPoint += 20;
		}
		
		// PBR 3 이상
		if (company.Pbr is > 3f) {
			AddPenalty(company.Pbr > 0f ? $"PBR 높음 ({company.Pbr:0.0}배)" : "PBR 정보 없음");
			company.WarningPoint += 10;
		}
		
		// 시가배당률 2% 이상
		if (company.DividendRate is > 2f) {
			AddRecommend($"시가배당률 높음 ({company.DividendRate:0.0}%)");
			company.RecommendPoint += Math.Min((int)((company.DividendRate - 2f) * 5f) + 5, 50);
		}

		var prev = company.YearPerformances[0]; 
		for (int i = 0; i < company.YearPerformances.Length; i++) {
			var cur = company.YearPerformances[i];

			bool isExpected = i == company.YearPerformances.Length - 1;
			string when = isExpected ? "[올해 예상]" : $"[{company.YearPerformances.Length - 1 - i}년 전]";
			
			if (cur.NetProfit is < 0) {
				AddPenalty(when + $"당기순손실 ({cur.NetProfit}억)");
				company.WarningPoint += 20;
			} else if (cur.GrossProfit is < 0) {
				AddPenalty(when + $"영업손실 ({cur.GrossProfit}억)");
				company.WarningPoint += 20;
			}

			if (i > 0 && cur.GrossProfit is > 0 && prev.GrossProfit != null) {
				if (cur.GrossProfit > prev.GrossProfit) {
					int curProfit = (int)cur.GrossProfit;
					int prevProfit = Math.Max(0, (int)prev.GrossProfit);
					if (prevProfit != 0) {
						float increaseRate = ((float)curProfit / prevProfit - 1f) * 100f; 
						company.RecommendPoint += Math.Clamp((int)increaseRate, 10, 30);
						AddRecommend(when + $"영업이익 {increaseRate:0.0}% 증가 ({prev.GrossProfit} -> {cur.GrossProfit})");
					} else {
						company.RecommendPoint += 5;
						AddRecommend(when + $"흑자 전환");
					}
				}

				if (cur.GrossProfit < prev.GrossProfit) {
					float curProfit = (float)cur.GrossProfit;
					float prevProfit = (float)prev.GrossProfit;
					float decreaseRate = (1f - (curProfit / prevProfit)) * 100f;
					AddPenalty(when + $"영업이익 {decreaseRate:0.0}% 감소: {prev.GrossProfit} -> {cur.GrossProfit}");
					company.WarningPoint += Math.Min((int)(decreaseRate * 0.5f), 20);
				}

				prev = cur;
			}

			if (i >= company.YearPerformances.Length - 2) {
				if (cur.DebtRatio is > 100f) {
					AddPenalty(when + $"부채율 높음 ({cur.DebtRatio:0.0}%)");
					company.WarningPoint += 10;
				}
			
				if (cur.QuickRatio is < 100f) {
					AddPenalty(when + $"당좌비율 낮음 ({cur.QuickRatio:0.0}%)");
					company.WarningPoint += 10;
				} else if (cur.QuickRatio > 200f) {
					AddRecommend(when + $"당좌비율 높음 ({cur.QuickRatio:0.0}%)");
					company.RecommendPoint += 10;
				}
			
				if (cur.ReserveRation is < 500f) {
					AddPenalty(when + $"유보율 낮음 ({cur.ReserveRation:0.0}%)");
					company.WarningPoint += 5;
				} else if (cur.ReserveRation is > 2000f) {
					AddRecommend(when + $"유보율 높음 ({cur.ReserveRation:0.0}%)");
					company.RecommendPoint += 5;
				}
			}
		}

		for (int i = 0; i < company.QuarterPerformances.Length; i++) {
			var cur = company.QuarterPerformances[i];
			
			bool isExpected = i == company.QuarterPerformances.Length - 1;
			string when = isExpected ? "[다음분기 예상]" : $"[{company.QuarterPerformances.Length - 1 - i}분기 전]";
			if (cur.NetProfit is < 0) {
				AddPenalty(when + $"당기순손실 ({cur.NetProfit}억)");
				company.WarningPoint += 10;
			}
		}

		Companies.Add(company);

		void AddPenalty(string reason) {
			string msg = $"감점: {reason}";
			log.AppendLine(msg);
		}
		void AddRecommend(string reason) {
			string msg = $"가점: {reason}";
			log.AppendLine(msg);
		}
		
		// 기존에 정보가 없으면 기본값으로 쓰던 부분들을 Nullable 타입으로 변경하면서 한시적으로 사용
		void ConvertToNullable(Company convertTarget) {
			if (convertTarget.MarketCap is 0) convertTarget.MarketCap = null;
			if (convertTarget.Per is -1f) convertTarget.Per = null;
			if (convertTarget.ExpectedPer is -1f) convertTarget.ExpectedPer = null;
			if (convertTarget.DividendRate is 0f) convertTarget.DividendRate = null;
			if (convertTarget.Pbr is -1f) convertTarget.Pbr = null;

			foreach (var performance in convertTarget.YearPerformances) {
				if (performance.SalesRevenue is 0) performance.SalesRevenue = null;
				if (performance.GrossProfit is 0) performance.GrossProfit = null;
				if (performance.NetProfit is 0) performance.NetProfit = null;
				if (performance.Roe is 0f) performance.Roe = null;
				if (performance.DebtRatio is 999f) performance.DebtRatio = null;
				if (performance.QuickRatio is 0f) performance.QuickRatio = null;
				if (performance.ReserveRation is 0f) performance.ReserveRation = null;
			}

			foreach (var performance in convertTarget.QuarterPerformances) {
				if (performance.SalesRevenue is 0) performance.SalesRevenue = null;
				if (performance.GrossProfit is 0) performance.GrossProfit = null;
				if (performance.NetProfit is 0) performance.NetProfit = null;
				if (performance.Roe is 0f) performance.Roe = null;
				if (performance.DebtRatio is 999f) performance.DebtRatio = null;
				if (performance.QuickRatio is 0f) performance.QuickRatio = null;
				if (performance.ReserveRation is 0f) performance.ReserveRation = null;
			}
		}
	}
}
