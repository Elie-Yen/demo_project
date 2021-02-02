## Math

This category includes projects that written with some math concepts or could be used for math-related purpose.

---

### password generator(c#)

* Purpose:   
In order to meet complexity requirements of different websites, this program is designed to produce a password that has a customized length(6 - 100) and made by required character sets (lowercases, uppercases, digits, space, symbols(non-alphanumeric) and customized characters(unicode)).  
You can also set a number of permutation time to make it more difficult to reproduce the password. 

* Concepts:  
Consider the security of password, this program adopted *System.Security.Cryptography.RandomNumberGenerator* class - which is an abstract class of cryptographic random number generators (rather than *Random class*, which produces a sequence of numbers that meet certain statistical requirements for randomness)(cite from .NET API Reference version 5.0).   
By using factoradic representation, this program takes nearly a liner time to generate kth permutation in lexicographical order (regard to the original one; doesn't consider repetition).

* Preview:  
```
// using uppercases, lowercases, digits, symbol and customized characters.
var ex = new Password();
ex.length = 25;
ex.symbol = true;
ex.other = "➌✿";

var res = ex.Generate();
Console.WriteLine(res); // Ub➌6yH1a@I0f9'4➌K.;➌➌O✿*h

res = ex.Permutation(res, 3628799); 
Console.WriteLine(res); // Ub➌6yH1a@I0f9'4h*✿O➌➌➌;.K
```
---

### Hamming Number Generator

* Purpose:   
Regular numbers are the numbers whose only prime divisors are 2, 3, and 5, In computer science, regular numbers are often called Hamming numbers. (cite from wiki)
A program contains functions of generate the kth hamming number (or list), validation of hamming number, and
hamming number decomposition (n = 2^i * 3^j * 5^k)  (in a specific field that the hamming number < 2 ^ 31).

* Concepts:  
Use similar approach of Edsger Dijkstra to calculate all hamming number from (1 - k) in ascending order. Validation and decomposition are achieved by modulo operation with modulus: 2, 3, 5.

* Preview:   
```
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
```

---

### probability calculator

* Purpose:
A program to determine the approximate probability of drawing certain balls randomly from a hat.
*Hat* calss takes a variable number of arguments that specify the number of balls of each color that are in the hat; and has a *draw* method which  picks specific number of balls at random and returns those balls as a list of strings.
Another *experiment* function return a probability of making correct predictions of picked balls during specific number of experiments.

* Concepts:
Import *random* module to pick balls randomly.
Use *collection.Counter* to speed up the process of getting the result of every *draw*.

* Preview:
```
>> hat = prob_calculator.Hat(red=3,blue=2)
>> hat.contents
["red","red","red","blue","blue"]

>> hat = prob_calculator.Hat(blue=3,red=2,green=6)
>> probability = prob_calculator.experiment(
                hat=hat, expected_balls={"blue":2,"green":1},
                num_balls_drawn=4, num_experiments=1000)
>> probability
0.272
```
---
