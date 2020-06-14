using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using Sirenix.OdinInspector;


// 자료구조와 알고리즘(Data Structure & Algorithm)

public class D01_Basic : MonoBehaviour
{
    // 01. 서론

    // 1. BIG-O 표기법
    // 컴퓨터 성능에 의존하지 않고 알고리즘의 좋고 나쁨을 비교하는 방식
    // 수행되는 연산의 개수를 대략적으로 파악한 후, 영향력이 큰 항목만 남기고 삭제한다. 이 때 상수도 무시한다. 어떤 방식으로 연산량이 증가하는가를 보는 것이 중요하기 때문.

    void Example01(int n)
    {
        int sum = 0;                                    // 연산량 1
        
        for (int i = 0; i < 2 * n; i++)                 // 연산량 n (1 * for문 반복 횟수)
        {
            sum += 1;
        }
        for (int i = 0; i < 2 * n; i++)                 // 연산량 6 * n^2 (이중 for문이므로 2n * 3n)
        {
            for (int j = 0; j < 3 * n; j++)
            {
                sum += 2;
            }
        }

        sum -= 20;                                      // 연산량 1
    }
    // 총 연산 개수 : O (1 + n + 6n^2 + 1)               // 앞의 O는 Order Of 의 약자.
    // 가장 영향력이 큰 항목 : O (6n^2)                  // 계수에 따른 n^2 의 증가량에 비하면 나머지의 영향은 미미하다.
    // 상수 삭제 : O (n^2)


    // n 값의 증가에 따라 연산량이 커지는 정도(시간 복잡도, Time Complexity)는
    //  1(상수) < log n < n < n * log n < n^2 < 2^n < n! 순으로 늘어난다. 물론 2^n과 n^2는 2가 아닌 다른 수가 올 수도 있다.
    //  n^2와 같은 다차항부터는 주의가 필요하며, 2^n과 같은 지수함수 형태부터는 기하급수적으로 연산량이 늘어나니 주의할 것.

    //---------------------------------------------------------------------








    // 2. 시간 측정하기
    // Environment.TickCount : 프로그램 시작 후 경과 시간을 나타냄 (단위: 밀리초)                                               - using System 필요
    // StopWatch : 생성 후 Start와 Stop로 특정 코드 사이의 시간 측정 가능. 마지막으로 측정된 시간은 .Elapsed로 확인할 수 있다.  - using System.Diagnostics 필요

    int lastTick = 0;
    const int WaitFrameTime = 33; //프레임 관리를 위한 값. 초당 30프레임 기준으로 테스트할 것이며, 따라서 33밀리초에 한 번만 코드가 실행되게 한다.

    [Button]
    void Test01(int frameCount = 90) 
    {
        int count = 0;

        while (count < frameCount)    //유니티상에서 while을 무한으로 실행하면 프로그램이 먹통이 되므로, 테스트를 위해 실행 횟수를 제한한다. 테스트할 프레임만큼 인자로 입력.
        {
            //프레임 관리
            int currentTick = Environment.TickCount;                // 실행 후 경과한 시간(밀리초)
            if (currentTick - lastTick < WaitFrameTime) continue;   // 최근 코드가 실행된 지 20밀리초가 경과하지 않았다면 실행하지 않음

            lastTick = currentTick;                                 // 20밀리초가 지났으면 코드를 실행할테니, 다시 최근 시간을 현재 시간으로 바꿔준다.
            count++;

            Stopwatch stopwatch = new Stopwatch();                  // 시간 측정을 위해 스톱워치를 생성
            stopwatch.Start();                                      // 스톱워치 시작

            //테스트할 코드 입력
            //------------------------------------------------------------------------------------


            int a = 0;

            //------------------------------------------------------------------------------------

            stopwatch.Stop();                                       // 스톱워치 종료
            UnityEngine.Debug.Log($"코드 실행에 걸린 시간 : {stopwatch.Elapsed}");    // 측정한 시간 확인
        }
        //이 테스트 방법은 아래 AlgorithmTest 클래스에 적용되어 있다.
        //알고리즘을 테스트하고 싶은 경우, MonoBehaviour 대신 AlgorithmTest 클래스를 상속받아서, TestCode01~05를 오버라이드하고 테스트할 코드를 적어 실행할 것.
    }
}

/// <summary>
/// MonoBehaviour를 대체하는 알고리즘 테스트용 코드입니다.
/// TestCode01~05를 오버라이드해서 코드 작동 시간을 테스트할 수 있으며, 오버라이드 시 베이스 코드는 상속받지 마세요. (Debug.Log에 많은 시간이 소요됩니다)
/// 작성한 코드의 순수한 동작 시간을 알기 위해서 비어 있는 가상함수의 작동시간을 측정하고 싶다면, Code Number를 None으로 설정하면 됩니다.
/// code 1~5는 인스펙터에서 식별하기 위한 정보입니다. 코드가 어떤 내용인지 간략한 설명을 적어주세요.
/// </summary>
public class AlgorithmTest : MonoBehaviour
{
    int lastTick = 0;
    protected int WaitFrameTime = 17;
    List<double> results = new List<double>();

    [SerializeField, ReadOnly] protected string code1 = "-";
    [SerializeField, ReadOnly] protected string code2 = "-";
    [SerializeField, ReadOnly] protected string code3 = "-";
    [SerializeField, ReadOnly] protected string code4 = "-";
    [SerializeField, ReadOnly] protected string code5 = "-";
    [SerializeField, ReadOnly] protected string code6 = "-";
    [SerializeField, ReadOnly] protected string code7 = "-";
    [SerializeField, ReadOnly] protected string code8 = "-";
    [SerializeField, ReadOnly] protected string code9 = "-";
    [SerializeField, ReadOnly] protected string code10 = "-";
    [SerializeField, HideIf(nameof(doOneTime))] bool debugEveryWorktime = false;

    public enum TestCodeNumber { Code1, Code2, Code3, Code4, Code5, Code6, Code7, Code8, Code9, Code10, None }

    [SerializeField] TestCodeNumber codeNumber = TestCodeNumber.Code1;
    [SerializeField, HideIf(nameof(doOneTime))] int totalTestCount = 30;
    [SerializeField, PropertyOrder(-1)] protected bool doOneTime = false;
    [SerializeField, HideIf(nameof(doOneTime))] bool isCustomFPS = false;
    [SerializeField, ShowIf(nameof(isCustomFPS)), HideIf(nameof(doOneTime))] int fps = 30;

    [Button, DetailedInfoBox("선택한 코드와 실행횟수를 바탕으로 테스트를 실행합니다.", "선택한 코드와 실행횟수를 바탕으로 테스트를 실행합니다." +
        "\n - 최대 5개의 코드 동작시간을 체크할 수 있습니다. Code Number에서 테스트할 코드를 선택하고, 테스트할 횟수를 Total Test Count에 입력하세요. Debug Every Worktime을 체크하면 매 실행마다 작업 시간을 출력합니다." +
        "\n - 기본적으로 초당 60프레임을 기준으로 작동합니다. FPS를 변경하길 원할 경우, Is Custom FPS를 체크하고 원하는 초당 프레임을 입력하세요.")]
    void Test()
    {
        int count = 0;
        int waitTime = WaitFrameTime;
        if (isCustomFPS) waitTime = 1000 / fps;
        string codeNum = "";

        results.Clear();
        double totalTime = 0;

        lastTick = Environment.TickCount;

        while (count < totalTestCount)
        {
            //프레임 관리
            int currentTick = Environment.TickCount;
            if (currentTick - lastTick < waitTime) continue;

            lastTick = currentTick;
            count++;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //테스트할 코드 입력
            //------------------------------------------------------------------------------------

            if (codeNumber == TestCodeNumber.Code1)
            {
                TestCode01();
                codeNum = "1번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code2)
            {
                TestCode02();
                codeNum = "2번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code3)
            {
                TestCode03();
                codeNum = "3번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code4)
            {
                TestCode04();
                codeNum = "4번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code5)
            {
                TestCode05();
                codeNum = "5번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code6)
            {
                TestCode06();
                codeNum = "6번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code7)
            {
                TestCode07();
                codeNum = "7번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code8)
            {
                TestCode08();
                codeNum = "8번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code9)
            {
                TestCode09();
                codeNum = "9번 함수";
            }
            else if (codeNumber == TestCodeNumber.Code10)
            {
                TestCode10();
                codeNum = "10번 함수";
            }
            else
            {
                TestCodeDefault();
                codeNum = "빈 함수";
            }

            //------------------------------------------------------------------------------------

            stopwatch.Stop();

            if (doOneTime)
            {
                UnityEngine.Debug.Log($"[{codeNum}] 실행에 걸린 시간 : {stopwatch.Elapsed.TotalMilliseconds}ms");
                return;
            }

            if (debugEveryWorktime) UnityEngine.Debug.Log($"[{codeNum}] {count}회차 실행시간 : {stopwatch.Elapsed.TotalMilliseconds}ms");
            results.Add(stopwatch.Elapsed.TotalMilliseconds);
        }

        results.RemoveAt(0);    //첫 시행엔 시간이 오래 걸리므로 첫 번째 측정값은 지운다.
        foreach (double value in results) totalTime += value;

        UnityEngine.Debug.Log($"[{codeNum}] 실행 당 평균 동작 시간 : {(totalTime / (double)(totalTestCount - 1)).ToString("0.000000")} 밀리초" + 
            $"\n실행 횟수 : {totalTestCount - 1}회 (첫 실행 제외)");    //첫 시행값은 지웠으니 시행횟수에서도 1을 빼고 나눈다.
    }
    public virtual void TestCode01() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode02() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode03() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode04() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode05() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode06() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode07() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode08() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode09() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }
    public virtual void TestCode10() { UnityEngine.Debug.Log("이 코드는 비어 있습니다."); }

    public virtual void TestCodeDefault() { }

}