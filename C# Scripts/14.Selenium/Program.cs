using System;
using System.Collections.Generic;
using System.Threading;	
using OpenQA.Selenium.Chrome;	
using OpenQA.Selenium;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

// 실행 파일 위치에 현재 설치된 크롬 버전에 맞는 chromedriver.exe 필요

static class Program {
	private static StringBuilder log = new StringBuilder();
	private static StringBuilder output = new StringBuilder();
	private const string SavePath = @"C:\Users\서지호\Desktop\NaverFinanceOutput";

	private const string NullValue = "N/A";

	private static readonly int[] ConstTarget = new int[] { };

	private static readonly int[] AlreadyAnalysis = new int[] { };

	private static readonly Dictionary<int, List<string>> outputList = new Dictionary<int, List<string>>();

	static async Task Main(string[] args) {
		await AnalysisAll();
		//ExtractTargetCompanyNames();
	}

	static void ExtractTargetCompanyNames() {
		string content = File.ReadAllText($"{SavePath}.txt");
		string info = string.Empty;

		int count = 0;
		int cur = 0;
		while (true) {
			var start = content.IndexOf('[', cur);
			if (start < 0) break;

			var end = content.IndexOf(']', start);
			info += $"{content.Substring(start + 1, end - start - 1)}, ";

			cur = end;
			count++;

			if (count % 5 == 0) info += '\n';
		}
		
		info += '\n';
		info += $"선정된 기업: {count}개";

		File.WriteAllText($"{SavePath}_info.txt", info);
	}

	static async Task AnalysisAll() {
		outputList.Clear();
		
		using (IWebDriver driver = new ChromeDriver()) {
			// 대기 설정. (find로 객체를 찾을 때까지 검색이 되지 않으면 대기하는 시간 초단위)
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

			var targets = TargetData.AnalyticsTargets;
			
			int logCount = 0;
			foreach (var testTarget in targets) {
				if (!ConstTarget.Contains(testTarget) && AlreadyAnalysis.Contains(testTarget)) continue;
				
				log.AppendLine($"{testTarget}에 대한 분석을 시작합니다.");
				bool ok = false;
				try {
					driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
					ok = await Analysis(driver, testTarget);
					
					if (ok) log.AppendLine($"{testTarget}에 대한 분석을 저장합니다.");
					else log.AppendLine($"{testTarget}은 적합하지 않은 대상입니다.");
				} catch (Exception e) {
					log.AppendLine($"{testTarget}을 분석하던 중 오류가 발생했습니다.");
					log.AppendLine($"[{e.GetType().Name}] {e.Message} / {e.StackTrace}");
				}
				
				log.AppendLine();

				logCount++;
				if(logCount > 10) {
					log.AppendLine("로그 저장됨");
					_ = File.WriteAllTextAsync($"{SavePath}_log.txt", log.ToString());
					_ = File.WriteAllTextAsync($"{SavePath}.txt", output.ToString());
					log.AppendLine();
					logCount = 0;
				}
			}
		}

		StringBuilder summary = new StringBuilder().AppendLine();
		foreach (var pair in outputList) {
			summary.AppendLine($"경고 {pair.Key}개 기업 ({pair.Value.Count}개)");
			foreach (var name in pair.Value) {
				summary.Append($"{name},");
			}
			summary.Remove(summary.Length - 1, 1);
			summary.AppendLine();
		}

		output.AppendLine(summary.ToString());
		
		await Task.Delay(500);
		await File.WriteAllTextAsync($"{SavePath}.txt", output.ToString());
		await File.WriteAllTextAsync($"{SavePath}_log.txt", log.ToString());
	}

	static string GetValueByWeb(IWebDriver driver, By cssSelector) {
		try {
			var text = driver.FindElement(cssSelector).Text;
			return (text == NullValue) ? string.Empty : text;
		} catch (Exception e) {
			Console.WriteLine($"값을 읽는 중 에러가 발생했습니다: [{e.GetType().Name}] {e.Message}");
		}
		return string.Empty;
	}

	static async Task<bool> Analysis(IWebDriver driver, int code) {
		StringBuilder temp = new StringBuilder();
		StringBuilder penalty = new StringBuilder();
		int penaltyCount = 0;

		// 해당 Url로 이동한다.
		driver.Url = UrlHelper.GetUrl(code);
		Console.WriteLine($"URL: {driver.Url}");
		
		// Url 내에서 요소를 찾는건 오래 걸리지 않으므로, 0.2초 내에 못 찾으면 정보 없음으로 간주한다.
		driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.2f);

		string companyName = GetValueByWeb(driver, SelectorHelper.GetSelector(SelectorHelper.Basic.CompanyName));
		temp.AppendLine($"[{companyName}]");

		//시가총액 500억 미만이면 리턴
		string marketCapString = GetValueByWeb(driver, SelectorHelper.GetSelector(SelectorHelper.Basic.MarketCap));
		if (string.IsNullOrEmpty(marketCapString)) AddPenalty($"시가총액 정보 없음");
		else if (!marketCapString.Contains('조') &&
		    (int.TryParse(marketCapString.Replace(",", string.Empty), out int v) && v < 500))
			AddPenalty($"시가총액 낮음 ({marketCapString}억)");
		
		// PER 40 이상 or PBR 3 이상이면 리턴 (단, 예상 PER은 없어도 통과)
		string per = GetValueByWeb(driver, SelectorHelper.GetSelector(SelectorHelper.Basic.CurrentPER));
		string expectedPer = GetValueByWeb(driver, SelectorHelper.GetSelector(SelectorHelper.Basic.ExpectedPER));
		string pbr = GetValueByWeb(driver, SelectorHelper.GetSelector(SelectorHelper.Basic.PBR));

		if (string.IsNullOrEmpty(per) || !float.TryParse(per, out var perValue)) AddPenalty($"PER 없음 (당기순손실 예상)");
		else if (perValue > 30f) AddPenalty($"PER 높음 ({perValue:0.0}배)");

		if (string.IsNullOrEmpty(expectedPer) || !float.TryParse(expectedPer, out var ePerValue)) AddPenalty($"PER 예측치 없음");
		else if (ePerValue > 30f) AddPenalty($"예측 PER 높음 ({ePerValue:0.0}배)");
		
		if (!float.TryParse(pbr, out var pbrValue)) AddPenalty($"PBR 없음 (당기순손실 예상)");
		else if (pbrValue > 3f) AddPenalty($"PBR 높음 ({pbrValue:0.0}배)");

		temp.AppendLine($"시가총액: {marketCapString}억 원");
		temp.AppendLine($"PER: {per} / 미래 PER: {expectedPer}");
		temp.AppendLine($"PBR: {pbr}");
		

		var headers = Enum.GetValues<SelectorHelper.Header>();
		var values = Enum.GetValues<SelectorHelper.Value>();

		int emptyInfoCount = 0;
		foreach (var header in headers) {
			foreach (var value in values) {
				if (penaltyCount >= 3) return false;
				
				if (value == SelectorHelper.Value.Name) {
					temp.AppendLine(GetValueByWeb(driver, SelectorHelper.GetSelector(header, value)));
					temp.AppendLine("연도별 예측");
					continue;
				}
				
				var text = GetValueByWeb(driver, SelectorHelper.GetSelector(header, value));
				if (string.IsNullOrEmpty(text) || float.TryParse(text, out var _) == false) {
					temp.Append("???");
					emptyInfoCount++;
					if (emptyInfoCount > 10) {
						AddPenalty($"정보 공란 10개 이상");
						return false;
					}
				} else {
					temp.Append(text);
				}

				if (value == SelectorHelper.Value.CurrentYearExpect) {
					temp.AppendLine();
					temp.AppendLine("분기별 예측");
				} else if (value != SelectorHelper.Value.CurrentQuarterExpect) {
					temp.Append(" -> ");
				}
				
				// 당기순손실이면 감점
				if (header == SelectorHelper.Header.NetProfit) {
					if (float.TryParse(text.Replace(",", ""), out var currentValue) &&
					    currentValue < 0) {
						AddPenalty($"당기순손실 ({currentValue:0.0}억)");
					}
				} // 부채비율이 100% 이상이면 감점
				else if (header == SelectorHelper.Header.DebtRatio) {
					if (float.TryParse(text.Replace(",", ""), out var currentValue) &&
					    currentValue > 100) {
						AddPenalty($"부채비율 높음 ({currentValue:0.00}%)");
					}
				} // 당좌비율이 100% 이하면 감점
				else if (header == SelectorHelper.Header.QuickRatio) {
					if (float.TryParse(text.Replace(",", ""), out var currentValue) &&
					    currentValue < 100) {
						AddPenalty($"당좌비율 낮음 ({currentValue:0.00}%)");
					}
				} // 유보율이 500% 이하면 감점
				else if (header == SelectorHelper.Header.ReserveRation) {
					if (float.TryParse(text.Replace(",", ""), out var currentValue) &&
					    currentValue < 500) {
						AddPenalty($"유보율 낮음 ({currentValue:0.00}%)");
					}
				}
			}
			temp.AppendLine();
			temp.AppendLine();
		}
		
		// 감점요인이 5개 이상이면 기록하지 않음
		if (penaltyCount < 5) {
			output.AppendLine(temp.ToString());
			if (!outputList.ContainsKey(penaltyCount)) outputList.Add(penaltyCount, new List<string>());
			outputList[penaltyCount].Add(companyName);
			if (penaltyCount > 0) {
				output.AppendLine($"경고 {penaltyCount}개");
				output.AppendLine(penalty.AppendLine().ToString());
			}
			return true;
		}
		
		return false;

		void AddPenalty(string reason) {
			penaltyCount++;
			string msg = $"경고: {reason}";
			penalty.AppendLine(msg);
			log.AppendLine(msg);
		}
	}
}
