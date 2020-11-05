using System;
using System.Collections.Generic;

namespace NDMTest2020
{
    class Program
    {
        // 시간별로 생성되는 신발들의 정보를 담은 Distionary
        static Dictionary<int, List<Shoes>> shoesDict = new Dictionary<int, List<Shoes>>();
        // 문제에서 주어진 거리 x
        static int timeLimit;
        // 신발의 개수
        static int shoesCount;
        // 시간별 최대 거리를 저장하는 배열. maxDistance[a]는 출발 후 a초 경과했을 때의 최대 이동거리를 나타냄.
        static int[] maxDistance;

        static void Main()
        {
            SetInfo();
            Run();

            Console.WriteLine(maxDistance[timeLimit]);
        }

        static void SetInfo()
        {
            timeLimit = int.Parse(Console.ReadLine());
            shoesCount = int.Parse(Console.ReadLine());

            // 각각의 신발은 클래스로 저장됩니다.
            for (int i = 0; i < shoesCount; i++)
            {
                string[] shoesInfo = Console.ReadLine().Split(' ');
                Shoes shoes = new Shoes(int.Parse(shoesInfo[0]), int.Parse(shoesInfo[1]), int.Parse(shoesInfo[2]), int.Parse(shoesInfo[3]));

                if (!shoesDict.ContainsKey(shoes.SpawnTime))
                    shoesDict.Add(shoes.SpawnTime, new List<Shoes>());

                shoesDict[shoes.SpawnTime].Add(shoes);
            }

            // 속력 1을 기준으로 최대 거리 정보를 초기화한다.
            maxDistance = new int[timeLimit + 1];
            for (int i = 0; i < maxDistance.Length; i++)
            {
                maxDistance[i] = i;
            }
        }

        // 1초부터 x초까지 반복하며, 해당 시간에 생성되는 신발이 있는 경우 해당 신발 장착 시의 최대 거리 정보를 업데이트합니다.
        static void Run()
        {
            int currentTime = 1;

            while (currentTime < timeLimit)
            {
                if (!shoesDict.ContainsKey(currentTime))
                {
                    currentTime++;
                    continue;
                }

                foreach (Shoes shoes in shoesDict[currentTime])
                {
                    EquipShoes(shoes);
                }
                currentTime++;
            }

        }

        // 신발을 매개변수로 받아서, 해당 신발을 신었다고 가정하여 최대 거리 정보를 갱신합니다.
        static void EquipShoes(Shoes shoes)
        {
            int currentTime = shoes.SpawnTime;
            int currentDist = maxDistance[currentTime];

            while (currentTime < timeLimit && !shoes.EndDuration(currentTime))
            {
                currentDist += shoes.CurrentSpeed(currentTime);
                currentTime++;

                if (maxDistance[currentTime] < currentDist)
                    maxDistance[currentTime] = currentDist;
            }

            for (int i = currentTime; i < maxDistance.Length; i++)
            {
                if (maxDistance[i - 1] >= maxDistance[i])
                    maxDistance[i] = maxDistance[i - 1] + 1;
            }
        }

    }

    // 신발의 정보를 담은 클래스입니다.
    // [멤버 변수]
    //  생성 시간, 장착 시간, 지속 시간, 장착 시 속력
    //  - 프로퍼티로 외부에서는 Getter만 사용가능하며, Setter는 생성자로 대체합니다.
    // [멤버 함수]
    //  CurrentSpeed:   이 신발을 신었을 때, 출발 후 a초 경과 시점에서의 속력
    //  EndDuration:    신발의 지속시간이 끝났다면 true, 아니라면 false를 반환
    class Shoes
    {
        public int SpawnTime { get; private set; }
        public int EquipTime { get; private set; }
        public int Duration { get; private set; }
        public int MoveSpeed { get; private set; }

        public int CurrentSpeed(int currentTime)
        {
            if (currentTime < (SpawnTime + EquipTime))
                return 0;
            else if (!EndDuration(currentTime))
                return MoveSpeed;
            else
                return -1; // Error

        }
        public bool EndDuration(int currentTime)
        {
            return currentTime >= (SpawnTime + EquipTime + Duration);
        }
        public Shoes(int spawnTime, int equipTime, int duration, int moveSpeed)
        {
            SpawnTime = spawnTime;
            EquipTime = equipTime;
            Duration = duration;
            MoveSpeed = moveSpeed;
        }
    }
}
