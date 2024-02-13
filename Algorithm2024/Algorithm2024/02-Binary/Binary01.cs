namespace Algorithm2024; 

public class Binary01 : ISolutionExample {
    int MissingNumber(List<int> nums) {
        // 추가적인 컨테이너 할당을 피하기 위해, n까지의 합계 (n(n+1) / 2) 에서 nums 원소 합계를 뺀다.
        int maxNum = nums.Count;
        int sumIncludeResult = (maxNum * (maxNum + 1)) / 2;
        int sumOfNums = nums.Sum(n => n);

        return sumIncludeResult - sumOfNums;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(MissingNumber(new List<int>() { 3, 0, 1 }));
        ConsoleUtility.WriteLine(MissingNumber(new List<int>() { 0, 1 }));
        ConsoleUtility.WriteLine(MissingNumber(new List<int>() { 9, 6, 4, 2, 3, 5, 7, 0, 1 }));
    }
}