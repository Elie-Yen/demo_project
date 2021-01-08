## Other

This category includes projects used different techs eg. string formatting, dynamic programming to produce the result.
Some of projects here are assignments of online courses with specific requirement.

---

### arithmetic formatter

* Purpose: 
A function that receives a list of strings that are arithmetic problems and returns the problems arranged vertically and side-by-side.
* Concepts: 
Scan every char of problems and split strings into *operand operater operand* format while check if any char violates the rules (eg: value isn't numerical). Use *ljust* *rjust* to ensure the alignment of each line.
* Preview:
    ```
   >> arithmetic_arranger(["32 - 698", "1 - 3801", "45 + 43", "123 + 49"], True)
       32         1      45      123
    - 698    - 3801    + 43    +  49
    -----    ------    ----    -----
     -666     -3800      88      172
    ```
---
### time calculator (without using built-in *date* ) 

* Purpose:  
Takes in two required parameters and one optional parameter: a start time in the 12-hour clock format (ending in AM or PM), a duration time that indicates the number of hours and minutes, (optional) a starting day of the week (case insensitive).
The function should add the duration time to the start time and return the result.
* Concepts: 
Transform the time into 24-hour clock format in order to compute the overflow (by % mods). After the calculation, formatting the result for output.
* Preview:
```
>>  add_time("8:16 PM", "466:02", "tUeSday")
6:18 AM, Monday (20 days later)
```
---
### budget app

* Purpose: 
A *Category* class which is able to instantiate objects based on different budget categories with *ledger* instance. This class supports multiple methods: *deposit, withdraw, get_balance, transfer, check_funds*. Besides budget object is required to have specific format when being print.
Another function called *create_spend_chart* that takes a list of categories as an argument. It should return a string that is a bar chart.
* Concepts:
Use list to store the info of each transaction (a dictionary of amount: description). Override ```__repr__``` to meet the format requirement. Use list to arrange the names of categories vertically. 
* Preview:
```
>> food = budget.Category("Food")
>> food.deposit(900, "deposit")
>> food.withdraw(45.67, "milk, cereal, eggs, bacon, bread")
>> food.transfer(20, self.entertainment)
>> print(food)
*************Food*************
deposit                 900.00
milk, cereal, eggs, bac -45.67
Transfer to Entertainme -20.00

Total: 834.33

>> print(create_spend_chart([food, clothing, Auto]))
Percentage spent by category
100|  
 90|          
 80|          
 70|          
 60| o        
 50| o        
 40| o        
 30| o        
 20| o  o     
 10| o  o  o  
  0| o  o  o  
    ----------
     F  C  A 
     o  l  u 
     o  o  t 
     d  t  o 
        h    
        i    
        n    
        g    
```
---
### Chrismas tree builder

* Purpose:  
This program could produce a html code that presents a customized christmas tree with random decorations on it. Just paste on note and save it into .html, or paste on anywhere on your html code. And you will see your beautiful christmas tree.

* Concepts:  
Create several lines with ascending width and random color decorations by random module. Then concatenate each line and formatting it to produce html code.
You can customize the height(lines of the tree body), color of the tree/star/decorations, and text on it.

* Preview (it's modified to present the appearance of result):  
```
◆♥⊛✿❅ Mery Christmas ❅✿⊛♥◆

             ★
            *.*
            /|\
           *.*.*
           /♥|\\
          *.*.*.*
         /⊛/⊛\\\
         *.*.*.*.*
        ///❅|◆\\\
        *.*.*.*.*.*
       /◆///|\⊛\\\
       *.*.*.*.*.*.*
     /⊛/❅//|✿\❅\\\
             ■
```
---
