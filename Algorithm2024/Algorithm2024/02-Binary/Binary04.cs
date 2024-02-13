namespace Algorithm2024; 

public class Binary04 : ISolutionExample {
    uint ReverseBits(uint n) {
        uint result = n & 1;
        for (int i = 0; i < 31; i++) {
            result <<= 1;
            n >>= 1;
            result += n & 1;

            // 1줄로 하면
            // result |= (n >> i & 1) << (31 - i);
        }

        return result;
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(ReverseBits(0b00000010100101000001111010011100));
        ConsoleUtility.WriteLine(ReverseBits(0b11111111111111111111111111111101));
    }
}