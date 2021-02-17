/*---
Elie Yen
C#
Coin combinations
---*/

public class Coin{
    private static void Calc(int sum, Dictionary<int, int> coin_and_count, ref int[] ways){
        /*--
        The main function to calculate how many ways to reach the target sum
        by different coins (usage will less than coin's count)
        return none (in-place modify) 
        --*/
        
        /*--
        dynamic programming does't work, since we don't know the composition of
        each combination (it might use more coins than the coin's count)
        --*/

        sum ++;
        HashSet<int> vals = new HashSet<int>();
        vals.Add(0);
        foreach (KeyValuePair<int, int> kvp in coin_and_count){
            int coin = kvp.Key;
            HashSet<int> tmp = new HashSet<int>();
            for (int cnt = 1; cnt < kvp.Value + 1; cnt++){
                int c = coin * cnt;
                if (c > sum){
                    break;
                }
                foreach (int psum in vals){
                   if (c + psum < sum){
                       ways[c + psum] ++;
                       tmp.Add(c + psum);
                   }
                }
                
            }
            vals.UnionWith(tmp);
        }
    }
    private static int MaxsumAndValid(int sum, Dictionary<int, int> coin_and_count){
        /*--
        to ensure each coin and cnt are valid integers
        and will not cause overflow in Calc function
        return the maxsum the coin_and_count could achieve
        --*/
        if (coin_and_count.Count > 5){
            throw new ArgumentException("Error, only 1-5 kinds of coins is acceptable");
        }
        int maxsum = 0; 
        foreach (KeyValuePair<int, int> kvp in coin_and_count){
            // check positive integer and will not cause overflow
            if (kvp.Key <= 0 || kvp.Value <= 0 || kvp.Value > 50){
                throw new ArgumentException(
                    "Error, coin_and_count is not valid (coin_and_count must meet that " +
                    "1 < Max(cnt) < 100 and both coin and cnt are possitive integers)"
                );
            }

            // check sum overflow
            if (kvp.Key > int.MaxValue / kvp.Value){
                throw new ArgumentException("Error, sum(coin * cnt) must less than 2^31");
            }
            int tmp = kvp.Key * kvp.Value;
            if (int.MaxValue - maxsum < tmp){
                throw new ArgumentException("Error, sum(coin * cnt) must less than 2^31");
            }
            maxsum += tmp;

        }
        return maxsum;
    }
    public static int WayofSum(int sum, Dictionary<int, int> coin_and_count){
        /*--
        count how many ways to reach sum by different coins with restrictions of amount of each coin
        the result might not used all coins or as same amount as the count of coins
        sum: 0-2^31
        coin_and_count must meet that 1 < Max(cnt) < 51
        coin_and_count.Count <= 5
        and both coin and cnt are possitive integers
        --*/
        if (sum < 0){
            throw new ArgumentException("Error, sum can't be less than 0");
        }
        int maxsum = MaxsumAndValid(sum, coin_and_count);
        if (sum == 0 || sum == maxsum){
            return 1;
        }
        if (sum > maxsum){
            // args are valid, nut no way to achieve sum
            return 0;
        }
        int[] ways = new int[sum + 1];
        Calc(sum, coin_and_count, ref ways);
        return ways[sum];
    }
    public static int WayofSum(int sum, params int[] coins){
        /*--
        count how many ways to reach sum by different coins
        (no limit in usage of each coin)
        the result might not used all coins or as same amount as the count of coins
        sum: 0-2^31
        coin: possitive integers
        --*/
        if (sum < 0){
            throw new ArgumentException("Error, sum can't be less than 0");
        }
        if (sum == 0){
            return 1;
        }
        sum ++;
        // dynamic programming
        int[] ways = new int[sum];
        ways[0] = 1;

        // remove repetition of coin
        HashSet<int> coinset = new HashSet<int>(coins);
        foreach (int coin in coinset){
            if (coin <= 0){
                throw new ArgumentException("Error, coin must be possitive integers");
            }
            for (int psum = coin; psum < sum; psum++){
                ways[psum] += ways[psum - coin];
            }
        }
        return ways[sum - 1];
    }
    public static List<int> BuyWithChange(Dictionary<int, int> coin_and_count){
        /*--
        return a List of prices that are less than the sum of coin * cnt 
        but cannot be bought without change
        coin_and_count must meet that 1 < Max(cnt) < 51
        coin_and_count.Count <= 5
        and both coin and cnt are possitive integers
        --*/

        int sum = MaxsumAndValid(0, coin_and_count);
        
        int[] ways = new int[sum + 1];
        Calc(sum, coin_and_count, ref ways);
        List<int> res = new List<int>();
        for (int i = 1; i < sum; i++){
            if (ways[i] == 0){
                res.Add(i);
            }
        }
        return res;
    }
}

class Test {
    static void Main() {
        Dictionary<int, int> coins = new Dictionary<int, int>{
            [1]=5, [10]=2, [50]=4, [100]=3
        };
        // count the way to achieve sum = 112
        int WayWithConstraints = Coin.WayofSum(112, coins);
        int WayNoConstraints = Coin.WayofSum(112, 1, 10, 50, 100);
        Console.WriteLine(WayWithConstraints);
        /*--
        2 ways:
        1 * 2 + 10 * 1 + 100 * 1
        1 * 2 + 10 * 1 + 50 * 2
        --*/
        Console.WriteLine(WayNoConstraints);
        /*--
        23 ways:
        1 * 2 + 10 * 1 + 100 * 1
        1 * 2 + 10 * 1 + 50 * 2
        1 * 2 + 10 * 6 + 50 * 1
        1 * 2 + 10 * 11
        1 * 12 + 100 * 1
        1 * 12 + 50 * 2
        1 * 12 + 10 * 5 + 50 * 1
        1 * 22 + 10 * 4 + 50 * 1
        1 * 32 + 10 * 3 + 50 * 1
        1 * 42 + 10 * 2 + 50 * 1
        1 * 52 + 10 * 1 + 50 * 1
        1 * 62 + 50
        1 * 12 + 10 * 10
        1 * 22 + 10 * 9
        1 * 32 + 10 * 8
        1 * 42 + 10 * 7
        1 * 52 + 10 * 6
        1 * 62 + 10 * 5
        1 * 72 + 10 * 4
        1 * 82 + 10 * 3
        1 * 92 + 10 * 2
        1 * 102 + 10 * 1
        1 * 112
        --*/
        
        Dictionary<int, int> coins2 = new Dictionary<int, int>{
            [1]=5, [10]=2, [50]=2
        };
        var prices = Coin.BuyWithChange(coins2);
        foreach (int price in prices){
            Console.WriteLine(price);
        }
        /*--
        6, 7, 8, 9, 16, 17, 18, 19, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39,
        40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 56, 57, 58, 59, 66, 67, 68, 69, 76, 77, 78, 79,
        80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 106, 107,
        108, 109, 116, 117, 118, 119
        --*/
    }
}