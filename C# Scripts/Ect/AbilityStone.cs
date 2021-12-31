using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

// 로스트아크 어빌리티 스톤 세공을 많은 횟수 시뮬레이션하는 스크립트
namespace AbilityStone
{
	public enum Grade
	{
		Rare = 7,
		Hero = 8,
		Legend = 9,
		Relic = 10
	}

	class Program
	{
		static Dictionary<int, List<AbilityStone>> abilityStoneDict = new Dictionary<int, List<AbilityStone>>();
		const int TryCount = 100000;
		static Grade targetGrade = Grade.Legend;
		static string fileName = "AbilityStoneData.txt";
		static void Main(string[] args)
		{
			List<int> keys = new List<int>();
			for (int i = 0; i < TryCount; i++)
			{
				var stone = GetAutoWorkedAbilityStone(targetGrade);
				int successCount = stone.Option1 + stone.Option2;
				
				if (!abilityStoneDict.ContainsKey(successCount))
				{
					abilityStoneDict.Add(successCount, new List<AbilityStone>());
					keys.Add(successCount);
				}

				abilityStoneDict[successCount].Add(stone);
			}
			keys.Sort();

			StringBuilder infoLog = new StringBuilder();
			StringBuilder summary = new StringBuilder();
			foreach (var key in keys)
			{
				var stones = abilityStoneDict[key];
				infoLog.AppendLine($"{key}회 성공: {stones.Count}개");
				summary.AppendLine($"{key}회 성공: {stones.Count}개 ({(stones.Count / (float)TryCount) * 100:0.00}%)");
				foreach (var stone in stones)
				{
					infoLog.AppendLine($"옵션1: {stone.Option1} / 옵션2: {stone.Option2} / 옵션3: {stone.Option3}");
				}
				infoLog.AppendLine();
			}
			summary.AppendLine();

			int count77 = 0;
			int count95 = 0;
			int count86 = 0;
			int count410 = 0;
			if (abilityStoneDict.TryGetValue(14, out var stoneList))
			{
				summary.AppendLine("참고: 14회 성공 시");
				int sum = stoneList.Count;
				foreach (var stone in stoneList)
				{
					if (stone.Option1 == 7) count77++;
					else if (stone.Option1 == 5 || stone.Option1 == 9) count95++;
					else if (stone.Option1 == 6 || stone.Option1 == 8) count86++;
					else count410++;
				}
				summary.AppendLine($"-> 77돌: {count77}개 ({(count77 / (float)sum) * 100:0.00}%)");
				summary.AppendLine($"-> 95돌: {count95}개 ({(count95 / (float)sum) * 100:0.00}%)");
				summary.AppendLine($"-> 86돌: {count86}개 ({(count86 / (float)sum) * 100:0.00}%)");
				summary.AppendLine($"-> 10/4돌: {count410}개 ({(count410 / (float)sum) * 100:0.00}%)");
				summary.AppendLine();
			}

			string output = $"{summary}\n\n--------------------------------------\n\n{infoLog}";

			File.WriteAllText($"C:/Users/sjhkr/Desktop/{fileName}", output);

		}
		static AbilityStone GetAutoWorkedAbilityStone(Grade grade)
		{
			AbilityStone stone = new AbilityStone(grade);
			stone.AutoWork();
			return stone;
		}
	}

	class AbilityStone
	{
		public Grade Grade { get; private set; }
		public int PositiveCount1 { get; private set; }
		public int PositiveCount2 { get; private set; }
		public int NegativeCount { get; private set; }
		public int Option1 { get; private set; } = 0;
		public int Option2 { get; private set; } = 0;
		public int Option3 { get; private set; } = 0;
		public bool isWorked => (PositiveCount1 == 0) && (PositiveCount2 == 0) && (NegativeCount == 0);

		private Random random = new Random();

		private int _probability = 75;
		public int Probability
		{
			get => _probability;
			private set
			{
				_probability = Math.Clamp(value, 25, 75);
			}
		}

		public AbilityStone(Grade grade)
		{
			Grade = grade;
			int count = (int)grade;
			PositiveCount1 = PositiveCount2 = NegativeCount = count;
		}

		private bool GetWorkResult()
		{
			int value = random.Next(0, 100);
			if (Probability >= value)
			{
				Probability -= 10;
				return true;
			}
			else
			{
				Probability += 10;
				return false;
			}
		}

		private bool CanWork(int line)
		{
			if ((line == 1 && PositiveCount1 == 0) ||
				(line == 2 && PositiveCount2 == 0) ||
				(line == 3 && NegativeCount == 0) ||
				(line <= 0 || line > 3)) return false;

			return true;
		}

		public int TryWork(int line)
		{
			if (CanWork(line) == false) return -1;

			int result = GetWorkResult() ? 1 : 0;
			switch (line)
			{
				case 1:
					PositiveCount1--;
					if (result > 0) Option1++;
					break;
				case 2:
					PositiveCount2--;
					if (result > 0) Option2++;
					break;
				case 3:
					NegativeCount--;
					if (result > 0) Option3++;
					break;
				default:
					return -1;
			}
			return result;
		}

		private void AutoWorkOneTime()
		{
			if (isWorked) return;
			if (Probability > 60)
			{
				if (CanWork(1)) TryWork(1);
				else if (CanWork(2)) TryWork(2);
				else if (CanWork(3)) TryWork(3);
				else return;
			}
			else if (Probability > 50)
			{
				if (CanWork(2)) TryWork(2);
				else if (CanWork(1)) TryWork(1);
				else if (CanWork(3)) TryWork(3);
				else return;
			}
			else
			{
				if (CanWork(3)) TryWork(3);
				else if (CanWork(2)) TryWork(2);
				else if (CanWork(1)) TryWork(1);
				else return;
			}
		}

		public void AutoWork()
		{
			while (isWorked == false)
			{
				AutoWorkOneTime();
			}
		}
	}
}

