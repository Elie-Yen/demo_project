---

### Hamming Number Generator and Validator

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
