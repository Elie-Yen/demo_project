/*------
C#
Elie yen
hamming number generator
------*/

public class HammingNumber{
    // field
    public static int MaxK { get => 1691; }
    public static int MinK { get => 1; }
    public static int MaxHammingNumber{ get => 2125764000; }
    public static int MinHammingNumber{ get => 1; }
    
    // internal assistance programs
    private static int MinofThree(int n1, int n2, int n3){
        if (n1 <= n2 && n1 < n3){
            return n1;
        }
        if (n2 <= n1 && n2 < n3){
            return n2;
        }
        return n3;
    }
    private static int[] DecomposeHelper(int n){
        // this is for validation, decomposition
        // each int in res stands for: power of 2/3/5 and n mods
        // if n is not a Hamming number, the last int is not 1
        int[] res = new int[]{0, 0, 0, 0};
        while (n % 5 == 0){
            res[2] ++;
            n /= 5;
        }
        while (n % 3 == 0){
            res[1] ++;
            n /= 3;
        }
        while (n % 2 == 0){
            res[0] ++;
            n /= 2;
        }
        res[3] = n;
        return res;
    }
    public static int[] GenerateKthList(int k){
        // this is also for generate kth hamming number
        if (k > MaxK || k < MinK){
            throw new ArgumentOutOfRangeException("Error, k is out of range(1 ~ 1691)");
        }
         
        int hm = 1;
        List<int> history = new List<int>();
        history.Add(hm);

        int i2 = 0;
        int i3 = 0;
        int i5 = 0; 

        for (int i = 1; i < k; i++){
            hm = MinofThree(history[i2] * 2, history[i3] * 3, history[i5] * 5);
            history.Add(hm);
            if (history[i2] * 2 == hm){
                i2 ++;
            }
            if (history[i3] * 3 == hm){
                i3 ++;
            }
            if (history[i5] * 5 == hm){
                i5 ++;
            }
        }
        return history.ToArray();
    }
    public static int GenerateKthNum(int k){
        int[] res = GenerateKthList(k);
        return res[k - 1];
    }
    public static bool Valid(int n){
        int[] powers = DecomposeHelper(n);
        return powers[3] == 1;
    }
    public static int[] Decompose(int n){
        int[] powers = DecomposeHelper(n);
        if (powers[3] != 1){
            Console.WriteLine($"Error, {n} is not a valid Hamming number.");
            powers = Array.Empty<int>(); // return an empty array
        }
        else {
            Console.WriteLine($"{n} = 2 ^ {powers[0]} * 3 ^ {powers[1]} * 5 ^ {powers[2]}");
            Array.Resize(ref powers, 3);
        }
        return powers;
    }
}

class Test {
    static void Main() {
        int ex = HammingNumber.GenerateKthNum(1500);
        Console.WriteLine(ex); // 859963392
        HammingNumber.Decompose(ex);
        // return int array: {17, 8, 5}
        // print: 859963392 = 2 ^ 17 * 3 ^ 8 * 5 ^ 0

        ex = HammingNumber.GenerateKthNum(13);
        Console.WriteLine(HammingNumber.Valid(ex * 10)); // true

        int[] first_hamming_50 = HammingNumber.GenerateKthList(50);
        foreach (int hm in first_hamming_50){
             Console.WriteLine(hm);
        }
        /* -----------
         [1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 16, 18, 20, 24, 25, 27,
          30, 32, 36, 40, 45, 48, 50, 54, 60, 64, 72, 75, 80, 81, 90,
          96, 100, 108, 120, 125, 128, 135, 144, 150, 160, 162, 180,
          192, 200, 216, 225, 240, 243]
        -------*/
    }
}
