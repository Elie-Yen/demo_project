### Coin Combination

![demo coin combination](https://github.com/Ellie-Yen/demo_project/blob/master/Math/coin_combination/demo_coin_combination.png?raw=true)
  
 * Purpose:   
  This program consists of 2 functions: WayofSum and BuyWithChange.
  *WayofSum* could calculate the ways to achieve target sum and is an overloaded method with 2 input forms: one has constraints of the amount of each coin, the other does not.
  Based on the kinds of coins and amount of them, *BuyWithChange* would tell you the prices which are affordable but you will get change.

 * Concepts:   
  When no restriction in usage of each coin, **dynamic programming (DP)** is the main process to calculate the ways of sum.   
  The recursive relationship between sumX and coins is: ```ways[sumX] = sum(ways[sumX - coinA] + ways[sumX - coinB] ...)```   
  which we can understand by this way:   
    ```
    For example, we have coins: 1, 5 and we want to know how many ways that the sum of combination is 7.
    We first choose coin1 as the largest coin we use, and decompose 7 into combinations of coin1 and sum:
    
    Way(coin1, sum7) = Way(coin1 + sum6) = Way(sum6)
    --------------------------------------------------------------------
    
    and we continueously do the same thing to the sum, until we cannot decompose it:

    Way(coin1, sum6) = Way(coin1 + sum5) = Way(sum5)
    ...
    Way(coin1, sum1) = Way(coin1 + sum0) = Way(sum0) <- we cannot decompose it
    --------------------------------------------------------------------
    
    we know there is only 1 way to achieve sum0 (use 0 coins), thus we can calculate the result based on this:
    
    way(sum0) = 1
    --> way(coin1, sum1) = Way(coin1 + sum0) = Way(sum0) = 1
    --> way(coin1, sum2) = Way(coin1 + sum1) = Way(sum1) = 1
    ...
    --> way(coin1, sum7) = Way(coin1 + sum6) = Way(sum6) = 1
    --------------------------------------------------------------------
    
    Then we choose next coin, coin5, and add up the results above:
    
    way(sum0) = 1 
    --> way(sum1) += way(coin5, sum1) (= way(coin5 + sum -4) = 0) --> way(sum1) = 1
    ...
    --> way(sum5) += way(coin5, sum5) (= way(coin5 + sum0) = Way(sum0) = 1) --> way(sum5) = 2
    --> way(sum6) += way(coin5, sum6) (= way(coin5 + sum1) = Way(sum1) = 1) --> way(sum6) = 2
    --> way(sum7) += way(coin5, sum7) (= way(coin5 + sum2) = Way(sum2) = 1) --> way(sum7) = 2
    ```
    On the other hand, this technique is not available when there's a restriction in usage of each coin,
    because we don't know the composition of each sum.
    For example: target sum = 7,  coin_count = { [1]=5, [5]=1 }
    if we use the similar principle and consider the amount of each coin:
      ```
      way(coin1, sum0) = 1
      --> way(coin1, sum1) = 1
      --> way(coin1, sum2) = 1
      ...
      --> way(coin1, sum6) = 0 <-- we only have 5 coins
      --> way(coin1, sum7) = 0 <-- we only have 5 coins

      way(coin5, sum0) = 1
      --> way(coin5, sum1) = 0
      ...
      --> way(coin5, sum5) = 1
      --> way(coin5, sum6) = 0 <-- we only have 1 coin
      --> way(coin5, sum7) = 0 <-- we only have 1 coin

      way(sum1) = way(coin1, sum1) + way(coin5, sum1) 1 + 0 = 1
      way(sum2) = way(coin1, sum2) + way(coin5, sum2) 1 + 0 = 1
      ...
      way(sum5) = way(coin1, sum5) + way(coin5, sum5) 1 + 1 = 2
      way(sum6) = way(coin1, sum6) + way(coin5, sum6) 0 + 0 = 0
      way(sum7) = way(coin1, sum7) + way(coin5, sum7) 0 + 0 = 0

      ```
    We'll get a result of way(sum7) = 0, which is obviously wrong.
    If we take a closer look, we'll find out the only achievable way is way(coin5, sum5) * way(coin1, sum2) = 1 (two events are independent),
    which means we need to combine different coin and sum to get that answer.
    Furthermore, if we have more kinds of coins, for example  coin_count = {[1] = 5, [2] = 2, [3] = 1, [5] = 1}, we need to calculate:

      ```
      way(coin5 sum5) * way(coin1 sum2)
      + way(coin5 sum5) * way(coin2 sum2)
      + way(coin3 sum3) * way(coin1 sum4)
      + way(coin3 sum3) * way(coin2 sum4)
      + way(coin3 sum3) * way(coin2 sum2) * way(coin1 sum2)
      + way(coin2 sum2) * way(coin1 sum5)
      ```
    Unlike the situation without restriction, things get more complex because we need to separate the way of sum by different coin for multiplication.
    Thus, the restricted one use **breadth-first search (BFS)** to make the process more simple:
      ```
      target sum = 7, coin_count = {[1] = 5, [2] = 2, [3] = 1, [5] = 1}
      First, calculate the possible results when we only use coin1 (use 0  1 ... 5 coins):
      achievable results = {0, 1, 2, 3, 4, 5} 
      --------------------------------------------------------------------
      
      Next, we expand the results by use the second coin2
      (use 1 coin: (0 + 2 * 1, 1 + 2 * 1, 3 + 2 * 1, 4 + 2 * 1, 5 + 2 * 1)
       use 2 coin: (0 + 2 * 2, 1 + 2 * 2, 3 + 2 * 2, 4 + 2 * 2, 5 + 2 * 2)
      ):
      achievable results = {0, 1, 2, 3, 4, 5, 6 ,7, 8, 9} 
      (The repetition results are omitted but will still be recorded)
      --------------------------------------------------------------------
      
      And we continuously repeat the process until there's no coin left.
      ```

* Preview:
  ```
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
  ```
