using System;
//using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorTest : MonoBehaviour
{
    public bool isShow;
    [ShowIf(nameof(isShow))] public int n;

    [Flags] //using System 필요
    public enum CharacterType { None = 0, Player, Monster, NPC = 4 } //enum에 숫자 지정 가능. 지정하지 않으면 바로 앞 숫자 + 1이 된다.

    [OnValueChanged(nameof(OnCharacterTypeChanged))]
    public CharacterType characterType;

    [Button]
    public void PlayerAndMonSter()
    {
        characterType = CharacterType.Player | CharacterType.Monster;
        //비트연산자
        // | 는 각 비트에서 자리수가 같은 두 숫자(0 또는 1) 중 하나라도 1이면 1, 둘 다 0이면 0. 예를 들어, 01 | 10 = 11
        // & 는 두 숫자 모두 1이면 1, 하나라도 0이면 0. 예를 들어, 110 & 101 = 100
        // ^ 는 두 숫자가 다르면 1, 같으면 0. 예를 들어, 110 ^ 101 = 011
        // '(int값) << n' 은 n의 자리만큼 왼쪽으로 옮기기. >> n은 오른쪽으로 옮기기. 예를 들어, 1010 << 1 = 10100, 1011 >> 1 = 101

    }


    void OnCharacterTypeChanged()
    {
        n = (int)characterType;
    }


    [Button]
    void Test()
    {
        string hello = "안녕";
        string hi = null;
        string str = hi ?? hello; //hi가 있으면 대입하고, hi가 null이면 뒤의 것(hello)을 대입한다.
        Debug.Log(str);

        Action nullAction = null;
        Action action = nullAction ?? (() => { Debug.Log("null 이다."); }); //무명 함수 만들기 () => {}
        action();

        bool isTest = 0 == 1 ? true : false; //삼항연산자. 변수 = 조건 ? 참일 때 대입할 것 : 거짓일 때 대입할 것;
        Debug.Log(isTest);
    }

    /*
    [Button]
    void CodeSpeedTest() //개별 코드나 함수만 이렇게 테스트하고, 게임 자체의 최적화는 Window - Analysis - Profiler (Ctrl + 7)에서 가장 오래 걸리는 요소를 찾아서 최적화한다. 
    {
        Stopwatch sw = new Stopwatch(); //using System.Diagnostics 필요
        sw.Start(); //스톱워치 시작
        for (int i = 0; i < 1000; i++) //작업을 1회만 실행 시 너무 빨리 끝날 수 있으니, 가벼운 작업은 여러 번 실행하여 속도를 측정한다.
        {
            //작업할 내용
        }
        sw.Stop();
        Debug.Log(sw.ElapsedMilliseconds); //걸린 시간을 밀리초 단위로 보기
    }
    */


    void Start()
    {
        Debug.Log(Application.dataPath); //현재 프로젝트의 에셋 폴더 주소
        Debug.Log(Application.persistentDataPath); //게임 데이터가 저장되는 장소

    }

    public class TestClass
    {
        public int n;
        public bool b;
    }

    void Match() //데이터 만들기, 찾기 (LINQ)
    {
        List<TestClass> database = new List<TestClass>();
        for (int i = 0; i < 100; i++)
        {
            database.Add(new TestClass
            {
                n = i, b = i % 2 == 0 ? true : false
            });
        }
        List<TestClass> list = database.FindAll(obj => obj.b == true);

        foreach (var testClass in list)
        {
            Debug.Log(testClass.n);
        }
    }
}