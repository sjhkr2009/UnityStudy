using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;	
using OpenQA.Selenium.Chrome;	
using OpenQA.Selenium;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using Newtonsoft.Json;

// 실행 파일 위치에 현재 설치된 크롬 버전에 맞는 chromedriver.exe 필요

static class Director {
	private static StringBuilder log = new StringBuilder();
	private static StringBuilder output = new StringBuilder();
	private const string SavePath = @"C:\Users\서지호\Desktop\AnalysisOutput";
	
	private static readonly int[] AlreadyAnalysis = new int[] { };

	private static readonly List<Company> Companies = new List<Company>();

	static async Task Main(string[] args) {
		AnalysisHelper.Initialize(log);
		
		Stopwatch stopwatch = Stopwatch.StartNew();
		
		//await AnalysisAllFromWeb();
		await AnalysisAllFromJson();
		
		log.AppendLine($"분석 소요시간: {(double)stopwatch.ElapsedMilliseconds / 1000}s");
		
		ExtractLogToFile();
		
		stopwatch.Stop();
		Console.WriteLine($"작업이 완료되었습니다. (소요시간: {(double)stopwatch.ElapsedMilliseconds / 1000}초)");
	}
	
	/// <summary>
	/// json 파일의 모든 기업을 Companies 배열에 Deserialize하고, 평가를 초기화한 후 분석하여 점수를 재산정합니다. 
	/// </summary>
	static async Task AnalysisAllFromJson() {
		var json = await File.ReadAllTextAsync($"{SavePath}_result.json");
		List<Company> targets = JsonConvert.DeserializeObject<List<Company>>(json);

		foreach (var company in targets) {
			Companies.Add(company.AnalysisAll());
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
				
				try {
					driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
					var company = Company.CreateFromWeb(driver, testTarget);
					Companies.Add(company.AnalysisAll());
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
}
