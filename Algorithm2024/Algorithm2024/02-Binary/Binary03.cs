namespace Algorithm2024; 

public class Binary03 : ISolutionExample {
    // Hamming Weight Algorithm: i & (i - 1) 을 하면 i 에서 제일 오른쪽 비트가 1 -> 0이 된다.
    int NumberOfOneBit(uint n) {
        int count = 0;
        while (n > 0) {
            count++;
            n &= (n - 1);
        }

        return count;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(NumberOfOneBit(0b1011));
        ConsoleUtility.WriteLine(NumberOfOneBit(0b10000000));
        ConsoleUtility.WriteLine(NumberOfOneBit(0b11111111111111111111111111111101));
    }
}