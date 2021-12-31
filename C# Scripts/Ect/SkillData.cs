using System;
using System.Collections.Generic;
using System.Threading;	
using OpenQA.Selenium.Chrome;	
using OpenQA.Selenium;
using System.IO;
 	
namespace Example
{
	// loawa 사이트에서 랭커들의 정보를 긁어와서 파일로 정리하는 스크립트
	// 실행하는 위치에 현재 설치된 크롬 버전에 맞는 chromedriver.exe 필요
    static class Program
    {
        public const int TargetPlayerCount = 100;

        public static int ignoredData = 0;
        public static int raidPlayerCount = 0;
        public static int notRaidPlayer = 0;
        public static List<Player> playerData = new List<Player>();

        public static List<Skill> skillsForRaid = new List<Skill>();
        public static Dictionary<string, List<Skill>> raidSkills = new Dictionary<string, List<Skill>>();
        public static List<SkillData> skillDB = new List<SkillData>();

        static void Main(string[] args)
        {
            // ChromeDriver 인스턴스 생성	
            // 스택 영역이 종료되면 자동으로 Chrome 브라우저는 닫힌다.	
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Url = "https://loawa.com/rank";
                // 대기 설정. (find로 객체를 찾을 때까지 검색이 되지 않으면 대기하는 시간 초단위)
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
				try
				{
                    driver.FindElement(By.CssSelector("#pills-item > div:nth-child(1) > table > tbody > tr > td.pt-2.pb-2 > table > tbody > tr:nth-child(12) > td:nth-child(4) > label")).Click();
                }
                catch(Exception e)
				{
                    Console.WriteLine("사이트에 접속할 수 없습니다.");
                    return;
				}
                Thread.Sleep(2000);

				for (int i = 1; i <= TargetPlayerCount; i++)
				{
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
                    try
					{
                        driver.FindElement(By.CssSelector($"#input_data > tr:nth-child({i * 2}) > td:nth-child(2)")).Click();
                        driver.FindElements(By.CssSelector("body > div:nth-child(3) > div > div.col-lg-6.col-md-8.col-xl-6.p-0 > div.bg-theme-4.rounded.shadow-sm.pt-2.pb-0.pr-0.pl-0.text-left > div.bg-theme-4.rounded > div.btn-group.btn-group-toggle.pt-2.pb-2.pr-2.pl-2 > label.btn.bg-theme-5.btn2-theme-4.btn-sm.shadow-sm"))[1].Click();
                    }
                    catch (Exception e)
					{
                        ignoredData++;
                        continue;
                    }
                    
                    Player player = new Player();

					for (int k = 1; k <= 8; k++)
					{
                        string skillname = "";
                        try
						{
                            skillname = driver.FindElement(By.CssSelector($"#skillbox1 > div > div:nth-child({2 * k}) > div > div.media.p-0.m-0 > p > span.d-block.text-theme-0 > strong")).Text;
                        }
                        catch (Exception e)
						{
                            ignoredData++;
                            break;
                        }

                        string tripod1 = string.Empty, tripod2 = string.Empty, tripod3 = string.Empty;
						try
						{
                            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
                            tripod1 = driver.FindElement(By.CssSelector($"#skillbox1 > div > div:nth-child({2 * k}) > div > div.media.p-0.m-0 > p > span.text-grade2")).Text;
                            tripod2 = driver.FindElement(By.CssSelector($"#skillbox1 > div > div:nth-child({2 * k}) > div > div.media.p-0.m-0 > p > span.text-grade1")).Text;
                            tripod3 = driver.FindElement(By.CssSelector($"#skillbox1 > div > div:nth-child({2 * k}) > div > div.media.p-0.m-0 > p > span.text-grade4")).Text;
                        }
                        catch (Exception e) { }
                        
                        Skill skill = new Skill(skillname, tripod1, tripod2, tripod3);
                        Console.WriteLine($"[{skill.Name}] {skill.tripods[0]} / {skill.tripods[1]} / {skill.tripods[2]}");
                        player.AddSkill(skill);
                    }
                    playerData.Add(player);
                    driver.Url = "https://loawa.com/rank";
                }
            }
            Thread.Sleep(2000);

            RaidSkillAnalysis();
            MakeAnalysisFile();
        }

        static void RaidSkillAnalysis()
		{
			foreach (var player in playerData)
			{
                if (player.notForRaid)
				{
                    notRaidPlayer++;
                    continue;
                }

				foreach (var skill in player.skills)
				{
                    skillsForRaid.Add(skill);
                    if (!raidSkills.ContainsKey(skill.Name))
                        raidSkills.Add(skill.Name, new List<Skill>());

                    raidSkills[skill.Name].Add(skill);
                }
                raidPlayerCount++;
            }

            foreach (var skill in raidSkills)
            {
                AnalysisSkill(skill.Key, skill.Value);
            }
        }

        static void AnalysisSkill(string name, List<Skill> skills)
		{
            if (skills == null || skills.Count == 0) return;

            SkillData data = new SkillData(skills.Count, raidPlayerCount, name);

			foreach (var skill in skills)
			{
				for (int i = 0; i < 3; i++)
				{
                    data.AddTripod(i, skill.tripods[i]);
				}
			}

            skillDB.Add(data);
		}

        static void MakeAnalysisFile()
		{
            string fileText = "";
            foreach (var data in skillDB)
            {
                string skillText = $"[{data.Name}] 선택률 : {data.SelectionRate * 100f}%\n";

                for (int i = 0; i < data.Tripods.Length; i++)
                {
                    skillText += $"- 트라이포드 {i + 1} -\n";
                    foreach (var selected in data.Tripods[i])
                    {
                        skillText += $"{selected.Key} : {(selected.Value * 100f).ToString("0.00")}%\n";
                    }
                }
                fileText += skillText;
                fileText += "----------------------------------\n";
            }
            fileText += $"분석된 스킬트리 {raidPlayerCount}개 \n({TargetPlayerCount}명의 랭커 중 {ignoredData}개의 데이터는 로딩에 실패했으며, {notRaidPlayer}개의 스킬트리는 필터링되었습니다.)";

            File.WriteAllText("C:/Users/sjhkr/Desktop/SkillData.txt", fileText);
        }
    }

    public class Skill
	{
        public string Name { get; private set; }
        public string[] tripods;

        public Skill(string name)
		{
            Name = name;
            tripods = new string[3];
		}
        public Skill(string name, params string[] tripod) : this(name)
		{
            int len = Math.Min(tripod.Length, tripods.Length);
            for (int i = 0; i < len; i++)
			{
                tripods[i] = ExtractName(tripod[i]);
			}
		}

        private string ExtractName(string origin)
		{
            if (string.IsNullOrEmpty(origin)) return string.Empty;

            int levelIndex = origin.IndexOf('L');
            if (levelIndex < 1) return string.Empty;

            string sub = origin.Substring(0, levelIndex - 1);
            return sub;
		}
	}

    public class Player
	{
        public List<Skill> skills = new List<Skill>();
        public bool notForRaid = false;

        public void AddSkill(Skill skill)
		{
            if (skill.Name.Contains("셀레스티얼 레인") && skill.tripods[2].Contains("폭우"))
                notForRaid = true;

            if (skill.Name.Contains("체크메이트") && skill.tripods[2].Contains("카드의 파도"))
                notForRaid = true;

            if (skill.Name.Contains("언리미티드 셔플"))
                notForRaid = true;

            skills.Add(skill);
		}
	}

    public class Stat
	{
        public int Critical { get; private set; }
	}

    public class SkillData
	{
        public string Name { get; private set; }
        public float SelectionRate { get; private set; }
        public int SelectCount { get; private set; }
        private float addPoint;

        public Dictionary<string, float>[] Tripods { get; private set; } = new Dictionary<string, float>[3];
    
        public void AddTripod(int tripodNumber, string selectedTripod)
		{
            var dict = Tripods[tripodNumber];

            if (string.IsNullOrEmpty(selectedTripod))
                selectedTripod = "(선택안함)";

            if (!dict.ContainsKey(selectedTripod))
                dict.Add(selectedTripod, 0f);

            dict[selectedTripod] += addPoint;
        }

        public SkillData(int selectCount, int playerCount, string skillname)
		{
            Name = skillname;
            SelectCount = selectCount;
            SelectionRate = (float)selectCount / playerCount;
            addPoint = 1f / selectCount;

			for (int i = 0; i < 3; i++)
			{
                Tripods[i] = new Dictionary<string, float>();
			}
        }
    }
}
