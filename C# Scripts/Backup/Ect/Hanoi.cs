using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static int[,] blocks;
        static int blockSize;
        static int moveCount;
        
        static void Main()
        {
            while(true)
            {
                Console.Write("탑의 높이를 입력하세요: ");
                int.TryParse(Console.ReadLine(), out blockSize);

                if (blockSize > 0) break;

                Console.WriteLine("탑의 높이는 1 이상의 정수여야 합니다.");
            }

            InitBlock(blockSize);

            bool autoPlay;

            while (true)
            {
                Console.Write("플레이 모드를 입력하세요 (0: 정답 보기, 1: 직접 풀기) >> ");

                int mode = int.Parse(Console.ReadLine());
                autoPlay = (mode == 0) ? true : false;

                if (mode == 0 || mode == 1)
                    break;

                Console.Write("잘못된 입력입니다.");
            }

            PrintBlock();

            while (!autoPlay)
            {
                Console.Write("어떻게 움직일까요? (예: 1 3) >> ");
                string[] input = Console.ReadLine().Split(' ');
                if(input.Length != 2)
                {
                    Console.Write("잘못된 입력입니다.");
                    continue;
                }
                int from = int.Parse(input[0]) - 1;
                int to = int.Parse(input[1]) - 1;

                if(from < 0 || to < 0 || from >= 3 || to >= 3)
                {
                    Console.WriteLine("기둥은 3개이며, 1,2,3 중 하나를 입력해야 합니다.");
                    continue;
                }
                if(from == to)
                {
                    Console.WriteLine("같은 기둥을 입력하셨습니다.");
                    continue;
                }

                Move(from, to);

                if (CorrectCheck())
                {
                    Console.WriteLine("정답입니다!");
                    break;
                }
            }

            if (autoPlay)
            {
                Hanoi(blockSize, 'A', 'B', 'C');
            }
            Console.WriteLine($"실행 완료 (시행 횟수 : {--moveCount})");
        }

        static void InitBlock(int height)
        {
            moveCount = 1;
            blocks = new int[height, 3];
            for (int i = 0; i < height; i++)
            {
                blocks[i, 0] = i + 1;
            }
        }

        static void PrintBlock()
        {
            for (int y = 0; y < blockSize; y++)
            {
                string writeLine = "";
                for (int x = 0; x < 3; x++)
                {
                    writeLine += (blocks[y, x] != 0) ? blocks[y,x].ToString() : " ";
                    if(x != 2) writeLine += '\t';
                }
                Console.WriteLine(writeLine);
            }
        }

        static void Hanoi(int n, char from, char temp, char to)
        {
            if(n <= 1)
            {
                Move(from, to);
            }
            else
            {
                Hanoi(n - 1, from, to, temp);
                Move(from, to);
                Hanoi(n - 1, temp, from, to);
            }
        }

        static void Move(int from, int to) => Move((char)('A' + from), (char)('A' + to));

        static void Move(char from, char to)
        {
            Console.WriteLine();

            int target = 0;
            int fromInt = from - 'A';
            int toInt = to - 'A';

            int targetY = 0;

            for (int i = 0; i < blockSize; i++)
            {
                if (blocks[i, fromInt] > 0) 
                {
                    target = blocks[i, fromInt];
                    targetY = i;
                    break;
                }
            }
            if(target == 0)
            {
                Console.WriteLine($"{fromInt} 번째 기둥에 원반이 없습니다.");
                return;
            }
            for (int i = 1; i < blockSize; i++)
            {
                if (blocks[i, toInt] > 0)
                {
                    if(blocks[i, toInt] < target)
                    {
                        Console.WriteLine($"{from}에서 {to}로 원반을 옮길 수 없습니다. (옮기려는 블럭의 크기({target})보다 작은 원반({blocks[i, toInt]})이 있습니다.)");
                        return;
                    }
                    else
                    {
                        blocks[i - 1, toInt] = target;
                        break;
                    }
                }

                if (i == blockSize - 1)
                    blocks[i, toInt] = target;
            }
            
            Console.WriteLine($"{moveCount++} : {from}에서 {to}로 원반을 옮깁니다.");
            blocks[targetY, fromInt] = 0;
            PrintBlock();
            Console.WriteLine("--------------------------------------------");
        }

        static bool CorrectCheck()
        {
            if (blocks[0, 2] != 0)
                return true;
            else
                return false;
        }
    }
}