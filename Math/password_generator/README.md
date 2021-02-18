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
