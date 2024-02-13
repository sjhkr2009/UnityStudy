namespace Algorithm2024; 

public class Binary05 : ISolutionExample {
    int SumWithoutPlus(int a, int b) {
        int sum = a;

        while (b != 0) {
            int upper = a & b; // 자릿수를 올릴 비트들
            sum = a ^ b; // 그 외의 비트들을 더한다

            a = sum;
            b = upper << 1;
        }

        return sum;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(SumWithoutPlus(1, 2));
        ConsoleUtility.WriteLine(SumWithoutPlus(2, 3));
        ConsoleUtility.WriteLine(SumWithoutPlus(27, 33));
        ConsoleUtility.WriteLine(SumWithoutPlus(13543, 14242));
    }
}