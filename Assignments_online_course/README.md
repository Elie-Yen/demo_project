## Assignments of online courses
---

### arithmetic formatter

* Purpose: a function that receives a list of strings that are arithmetic problems and returns the problems arranged vertically and side-by-side.
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
---
### polygon area calculator
---
### probability calculator
---
