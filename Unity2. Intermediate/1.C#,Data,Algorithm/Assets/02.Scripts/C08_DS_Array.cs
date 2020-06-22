using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C08_DS_Array : MonoBehaviour
{
    // 08. 자료구조(Data Structure) -  배열(Array)

    // 1. 일차원 배열

    // 배열(Array)은 특정 형태의 값들을 여러 개 저장할 수 있는 공간이다.
    // 저장 가능한 값의 개수가 고정되어 있으며, 대입을 통해 재정의하는 방식 외에 공간의 확대/축소는 불가능하다.

    [Button]
    void TestArray01()
    {
        int[] scores = new int[10]; //10개의 int값을 저장할 수 있는 공간을 만든다.
        scores[0] = 10; // [index]를 입력하여 접근할 수 있다. 인덱스는 배열의 첫 번째 값부터 순서대로 0,1,2,3,... 의 숫자를 갖는다. 
        scores[2] = 30; //인덱스 2 = 배열의 세 번째 값

        for (int i = 0; i < scores.Length; i++) //배열.Length 로 배열의 길이를 가져올 수 있다.
        {
            scores[i] = (i + 1) * 10;
            Debug.Log($"{i + 1}번째 배열(인덱스 score[{i}])의 값 : {scores[i]}");
        }

        foreach (int i in scores)   //배열의 값에 규칙성이 있다면, 반복문을 응용하여 값을 넣어줄 수 있다. 
                                    //foreach(변수형태 변수명 in 배열명) { 실행할 내용 } 으로, 배열에 순서대로 for문을 돌릴 수 있다. 변수명에는 배열 내의 값이 순서대로 들어간다.
        {
            Debug.Log(i);
        }
    }
    
    [Button]
    void TestArray02()
    {
        //배열의 또다른 선언 방법
        int[] scores1 = new int[5] { 10, 20, 30, 40, 50 }; //배열 선언 뒤에 { 인덱스1 값, 인덱스2 값, ... } 식으로 초기값을 넣어준다. 배열 크기와 {} 사이의 값 개수가 다르면 에러가 뜬다.
        int[] scores2 = new int[] { 100, 200, 300, 400, 500 }; //배열의 개수를 생략할수도 있다. 이러면 {} 사이에 값이 몇개든 상관없지만, 배열 개수 역시 보장할 수 없다.
        int[] scores3 = { 1000, 2000, 3000, 4000, 5000 }; //아예 새로운 배열 선언을 생략할수도 있다. 간편하지만 배열임을 직관적으로 알아보기는 어려워 권장되지 않는다.

        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"배열의 값: {scores1[i]}, {scores2[i]}, {scores3[i]}");
        }

        //또한, 배열은 참조 타입임에 유의할 것.
        scores1 = scores2;                                      //스코어1에 스코어2를 대입하면, 스코어1은 스코어2와 같은 배열을 가리킨다.
        scores2[1] = 999;                                       //스코어2가 가리키는 배열의 값을 변경하면...
        Debug.Log($"1번 배열의 두번째 값 : {scores1[1]}");      //스코어 1의 값도 변경된다. 
    }

    [Button]
    void TestArray03()
    {
        //배열 연습하기
        int[] scores = new int[] { 60, 45, 70, 65, 90, 85, 55, 15, 100 };

        string origin = "";
        foreach (var item in scores) origin += $"{item} ";
        Debug.Log($"원본 배열 : {origin}");
        Debug.Log($"배열 내 최대값 : {GetHighestScore(scores)}");
        Debug.Log($"배열 평균값 : {GetAverageScore(scores)}");
        Debug.Log($"85의 인덱스 : {GetIndexOf(scores, 65)}");
        Debug.Log($"25의 인덱스 : {GetIndexOf(scores, 25)}");

        Sort(scores);
        //Sort2(scores);
        string sorted = "";
        foreach (var item in scores) sorted += $"{item} "; //배열은 참조 형식이므로, scores를 함수 내에서 수정해도 (ref를 붙인 것처럼) 외부 변수까지 변경된다.
        Debug.Log($"작은 순 배열 : {sorted}");

    }

    //배열 내의 값은 0 ~ 100 사이의 정수로 가정한다.
    int GetHighestScore(int[] scores) //1) 배열에서 가장 큰 수 찾기 
    {
        int highestNum = 0;
        foreach (var i in scores)
        {
            if (highestNum < i) highestNum = i;
        }

        return highestNum;
    }
    int GetAverageScore(int[] scores) //2) 평균값 찾기
    {
        int totalNum = 0;
        int count = scores.Length;
        if (count == 0) return 0; //0으로 나눌 수 없으므로 안전장치 필요

        foreach (int i in scores) totalNum += i;
        float average = (float)totalNum / (float)count;

        return Mathf.RoundToInt(average);
    }
    int GetIndexOf(int[] scores, int value) //3) 특정 값을 찾아 인덱스 번호를 반환하고, 못 찾으면 -1을 반환하기
    {
        int result = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] == value) result = i;
        }

        return result;
    }
    void Sort(int[] scores) //4) 배열을 작은 수에서 큰 수 순으로 정렬하기
    {
        int[] sortedArray = new int[scores.Length]; //scores와 같은 길이의 임시 배열을 생성한다. 배열의 모든 값은 0이다.

        for (int i = 0; i < scores.Length; i++)             //scores의 값을 하나씩 체크한다.
        {
            int index = scores.Length - 1;                  //인덱스를 배열의 맨 끝으로 설정한다.
            for (int j = 0; j < sortedArray.Length; j++)    //임시 배열을 쭉 훑으며
            {
                if (scores[i] < sortedArray[j])             //scores의 값보다 임시 배열의 수가 크면
                {
                    index = j - 1;                          //해당 지점이 scores[i]의 값이 들어갈 인덱스가 된다.
                    break;
                }
            }
            if(sortedArray[index] != 0)                     //해당 지점이 초기값이 아니면(다른 수가 이미 있으면)
            {
                for (int j = 1; j <= index; j++)            //임시 배열의 (인덱스 0은 어차피 사라질테니)인덱스 1부터 현재 위치까지의 값을
                {
                    sortedArray[j - 1] = sortedArray[j];    //모두 한 칸씩 앞으로 옮겨서 scores[i]가 들어갈 자리를 비워둔다.
                }
            }
            sortedArray[index] = scores[i];                 //해당 인덱스에 scores[i]를 넣는다.
        }

        for (int i = 0; i < sortedArray.Length; i++) scores[i] = sortedArray[i]; //완성된 임시 배열을 scores에 대입한다.
    }

    void Sort2(int[] scores) //참고 답안
    {
        for (int i = 0; i < scores.Length; i++)
        {
            // i부터 scores.Length - 1 중에서 제일 작은 숫자가 있는 인덱스 minIndex를 찾는다.
            int minIndex = i;
            for (int j = i; j < scores.Length; j++)
            {
                if (scores[j] < scores[minIndex]) minIndex = j;
            }
             
            //가장 작은 숫자를 인덱스 i에 있는 숫자와 바꾼다.
            int temp = scores[i];
            scores[i] = scores[minIndex];
            scores[minIndex] = temp;
        }
    }





    //--------------------------------------------------------------------------------------





    // 2. 다차원 배열 (2차원 배열)

    // 아파트에 층과 호수가 있듯이, 배열 내에 배열을 넣어 2차원 이상의 배열을 정의할 수 있다.
    
    [Button]
    void TestMultiArray01() 
    {
        //정의 방법은 일차원 배열과 비슷하게, 3가지 형태로 가능하다.
        int[,] scores = new int[2, 5];                                                          // 1) [,]로 정의 후 [x,y] 좌표로 접근하기
        scores[0, 0] = 20;
        scores[0, 1] = 30;
        for (int i = 0; i < scores.GetLength(0); i++) //다차원 배열[x,y,z,...]의 개수는 GetLength(x)와 같이 받아올 수 있다.
        {
            for (int j = 0; j < scores.GetLength(1); j++)
            {
                scores[i, j] = (i * 5) + ((j + 1) * 10); 
            }
        }
        int[,] scores2 = new int[2, 5] { { 20, 50, 75, 65, 60 }, { 30, 45, 75, 85, 70 } };      // 2) 바로 값 입력하기
        int[,] scores3 = { { 20, 50, 75, 65, 60 }, { 30, 45, 75, 85, 70 } };                    // 3) new 선언 생략

        Debug.Log("45 = " + scores[1, 3]); //45
        Debug.Log("70 = " + scores2[1, 4]); //70
    }


    //2D 탑다운 게임의 타일맵을 제작한다고 가정하자. 1은 벽이고, 0은 빈 공간이다.
    int[,] tiles = //맵의 배열을 저장하는 2차원 배열을 생성한다.
        {
            {1,1,1,1,1 },
            {1,0,0,0,1 },
            {1,0,0,0,1 },
            {1,0,0,0,1 },
            {1,1,1,0,1 }
        };
    public void Render() //2차원 배열에 따라 맵을 생성한다.
    {
        string render = "";
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(0); y++)
            {
                if (tiles[x, y] == 1) render += "■";
                else render += "□";
            }
            render += "\n";
        }
        Debug.Log(render);
    }

    [Button]
    void TestMultiArray02()
    {
        Render();
    }




    //--------------------------------------------------------------------------------------

    // 2.1. 가변 배열

    // 배열의 차원을 바꿀 수 있는 배열(배열들의 배열). 잘 쓰이지는 않는다.
    [Button]
    void TestMultiArray03()
    {
        int[][] map = new int[3][]; //대괄호를 두 개 선언하고, new로 형태를 선언한 뒤 첫번째 괄호에 배열의 개수를 적는다.

        map[0] = new int[2]; //각 배열마다 다시 배열을 선언할 수 있다.
        map[0][0] = 1;
        map[0][1] = 3;
        map[1] = new int[] { 2, 4, 6, 8 };
        //map[2] = { 1, 2, 3 }; new 생략은 불가

        Debug.Log("3 = " + map[0][1]); //3
        Debug.Log("6 = " + map[1][2]); //6
    }

}