## Other

This category includes projects used different techs eg. string formatting, dynamic programming to produce the result.
Some of projects here are assignments of online courses with specific requirement.

---

### BST builder, serializer and deserializer (C#)

* Purpose: 
This program support building a valid BST by an int array or a specific formatted string, common methods including <i>insertion, deletion, validation, contains(value), in/pre/post/level-order traversal(output by string or array)</i> and height/ count properties.
Furthermore, in order to optimized searching and traversal, it's build as an AVL.
* Concepts: 
Use bottom-up recursion(iteration) to build the BST, since height of every node is crucial to maintain AVL.
With the help of *SortedSet* in *System.Collections.Generic* namespace, this program could quickly rebuild after its validation failed.
* Preview:
```
>> int[] values = new int[]{1,2,3,4,5,6,7,8,9};
>> var ex = new BSTbuilder(values);
>> Console.WriteLine(ex); 
[5 | 3, 8 | 2, 4, 7, 9 | 1, null, null, null, 6, null, null, null]

>> var ex2 = new BSTbuilder("[5|5,99|42,4,78,9|1,null,null,null,6,null,null,null]"); // not valid, rebuild
>> Console.WriteLine(ex2); 
[9 | 5, 78 | 4, 6, 42, 99 | 1, null, null, null, null, null, null, null]

>> ex2.Delete(11); // this would do nothing since 11 is not in root
>> ex2.Delete(4);
>> Console.WriteLine(ex2.Insert(5));
false  // since 5 already in root

>> ex2.Insert(55);
>> Console.WriteLine(ex2);
[9 | 5, 78 | 1, 6, 42, 99 | null, null, null, null, null, 55, null, null]

>> Console.WriteLine(ex2.OrderVisualize("preorder"));
preorder : [ 9, 5, 1, 6, 78, 42, 55, 99 ] , height: 4, non-null elements: 8
```

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

