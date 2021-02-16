### Sum of Dices
  
* Purpose:   
  This program could calculate the distribution of and ways to achieve different sum of points when rolling n dices(m faces).
  The result could be used in real-world experiments. 

* Concepts:   
  Both the relationships between [sumA, sumB] and between [sum, number of dices] are recursive.   
  Thus the program uses **dynamic programming** to calculate the result.
  A 2-dimensional array is used to record the intermediate results in each calculation, dp[n][sumX] stores how many ways to achieve sumX by n dices.   
  (each permutation is recognized as one way, eg: (1, 2) != (2, 1))    
  
  The most tricky part is *we must use all dices in every calculation*.      
  That means, the largest point we can use in sumX is equal to *[sumX - 1 * number_of_dices]* , which might be different every time.   
  
  Besides, for each pointP,    
  the possible sum that could contain pointP is ranged from *[1 * number_of_dices + pointP]* to *[number_of_dices * pointP]*, which might also be different every time.   

* Preview:
```
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
 ```
