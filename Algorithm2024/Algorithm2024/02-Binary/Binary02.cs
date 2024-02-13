namespace Algorithm2024; 

public class Binary02 : ISolutionExample {
    List<int> ArrayOfBitCount(int n) {
        int[] result = new int[n + 1];

        for (int i = 0; i < n + 1; i++) {
            // (result[i / 2] << 1) 을 하고 1의 자리만 추가하면 result[i]가 된다. 
            result[i] = result[i / 2] + (i & 1);
        }
        
        // Hamming Weight Algorithm
        //  i & (i - 1) 을 하면 i 에서 제일 오른쪽 비트가 1 -> 0이 된다.
        //  이걸 써서 맨 오른쪽 비트를 제거한 수 k를 기준으로 result[i] = result[k] + 1 과 같이 구해도 됨. 

        return result.ToList();
    }

    public void PrintExample() {
        ConsoleUtility.WriteLine(ArrayOfBitCount(2));
        ConsoleUtility.WriteLine(ArrayOfBitCount(5));
        ConsoleUtility.WriteLine(ArrayOfBitCount(64));
    }
}