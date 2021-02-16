/*---
Elie Yen
C#
Sum of Dices
---*/

public class SumDices{
    public static int[] Distribution(int face_of_dice, int num_of_dice){
        /*--
        return an int array represents the distribution of ways 
        to reach sums of points (sum range from 0 to face_of_dice * num_of_dice)
        (it will used as same amount of dices as num_of_dice)
        each permutation seems as 1 way, for example: (1, 2) != (2, 1)
        both face_of_dice and num_of_dice must be positive integers
        and less than 10 to avoid overflow (ways > 2 ^31)
        --*/
        if (face_of_dice <= 0 || num_of_dice <= 0 || face_of_dice > 10 || num_of_dice > 10){
            throw new ArgumentException("Error, only positive integer of 1-10 is available");
        }

        // dynamic programming
        int max_sum = face_of_dice * num_of_dice;
        int[ , ] dp = new int[num_of_dice + 1, max_sum + 1];
        
        // initialize, for 1 dice, each point has 1 way to achieve it
        for (int p = 1; p < face_of_dice + 1; p ++){
            dp[1, p] = 1;
        }
        
        // since 1 dice has already been calculated, start from 2 dices
        for (int n = 2; n < num_of_dice + 1; n++){
            
            // the possible sums of n dices range from 1 * n to face_of_dice * n
            for (int sum = n; sum < face_of_dice * n + 1; sum++){
                
                // ensure at least 1 point of each n - 1 dice
                // that means sum - p >= n - 1; p <= sum - n + 1
                int maxpoint = Math.Min(sum - n + 2, face_of_dice + 1);
                for (int p = 1; p < maxpoint; p++){
                    dp[n, sum] += dp[n - 1, sum - p];
                }
            }
        }

        // we need all results of num_of_dice dices
        int[] ways = new int[max_sum + 1];
        for (int i = 0; i < max_sum + 1; i++){
            ways[i] = dp[num_of_dice, i];
        }

        return ways;
    }
    public static int Ways(int sum, int face_of_dice, int num_of_dice){
        /*--
        return an int represents the ways to reach sum of points
        (it will used as same amount of dices as num_of_dice)
        each permutation seems as 1 way, for example: (1, 2) != (2, 1)
        both face_of_dice and num_of_dice must be positive integers
        and less than 10 to avoid overflow (ways > 2 ^31)
        --*/
        if (sum < num_of_dice || sum > face_of_dice * num_of_dice){
            return 0;
        }
        int[] ways = Distribution(face_of_dice, num_of_dice);
        return ways[sum];
    }
}

class Test {
    static void Main() {
        int face = 6;
        int num = 3;
        int[] distribution = SumDices.Distribution(face, num);
        for (int i = 0; i < distribution.Length; i++){
            Console.WriteLine($"{distribution[i]} ways to reach sum = {i}");
        }

        int sum = 12;
        int ways = SumDices.Ways(sum, face, num);
        Console.WriteLine($"{ways} ways to reach sum = {sum} by {face}-faced dice * {num}");
        /*--
        output:     
            0 ways to reach sum = 0
            0 ways to reach sum = 1
            0 ways to reach sum = 2
            1 ways to reach sum = 3
            3 ways to reach sum = 4
            6 ways to reach sum = 5
            10 ways to reach sum = 6
            15 ways to reach sum = 7
            21 ways to reach sum = 8
            25 ways to reach sum = 9
            27 ways to reach sum = 10
            27 ways to reach sum = 11
            25 ways to reach sum = 12
            21 ways to reach sum = 13
            15 ways to reach sum = 14
            10 ways to reach sum = 15
            6 ways to reach sum = 16
            3 ways to reach sum = 17
            1 ways to reach sum = 18
            
            25 ways to reach sum = 12 by 6-faced dice * 3
            
        --*/
    }
}
