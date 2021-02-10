### Sudoku Solver

* Purpose:   
*Sudoku is a logic-based,  combinatorial number-placement puzzle. In classic sudoku, the objective is to fill a 9×9 grid with digits so that each column, each row, and each of the nine 3×3 subgrids that compose the grid contain all of the digits from 1 to 9. (cite: [Wiki](https://en.wikipedia.org/wiki/Sudoku))*
The program contains Solver, Creator and Validator of Sudoku.
Use backtracking to try different combinations (in-place modify) until reaching the answer.

* Concepts:   
Both Creator and Solver use *backtracking* to try different combinations of numbers. The only difference is the shuffle of number candidates to generate random board in Creator.   
In Creator, this program generates a valid Soduku solution then selecting random positions of deletion by *choice* in Numpy. However, the selection might cause the question having non-singly-solution.   
In Validator, one of the steps in the validation is finding if any number occurs more than once in any column/ row/ box. This process is accomplished by recording data (data will also be used in Solver).

* Preview
  ```
  >> question, solution = Sudoku.Creator(2)
  >> Sudoku.Solver(question)
  
  # question
  [[1 0 0 0 5 6 4 7 0]
   [0 0 3 0 0 1 9 6 2]
   [7 6 0 0 9 0 0 8 5]
   [0 0 6 5 1 8 7 3 0]
   [0 7 8 0 2 9 5 4 0]
   [9 0 0 3 7 0 6 0 0]
   [0 3 0 0 6 0 0 0 0]
   [2 0 0 0 3 7 8 5 6]
   [6 0 0 8 4 2 3 1 0]]
  
  # a solution by creator :   # solution by solver : 
  [[1 8 9 2 5 6 4 7 3]        [[1 8 9 2 5 6 4 7 3]
   [5 4 3 7 8 1 9 6 2]         [5 4 3 7 8 1 9 6 2]
   [7 6 2 4 9 3 1 8 5]         [7 6 2 4 9 3 1 8 5]
   [4 2 6 5 1 8 7 3 9]         [4 2 6 5 1 8 7 3 9]
   [3 7 8 6 2 9 5 4 1]         [3 7 8 6 2 9 5 4 1]
   [9 5 1 3 7 4 6 2 8]         [9 5 1 3 7 4 6 2 8]
   [8 3 7 1 6 5 2 9 4]         [8 3 7 1 6 5 2 9 4]
   [2 1 4 9 3 7 8 5 6]         [2 1 4 9 3 7 8 5 6]
   [6 9 5 8 4 2 3 1 7]]        [6 9 5 8 4 2 3 1 7]]
   
  ## validation
  >> valid_solution = [
            [1, 6, 3, 2, 8, 7, 4, 9, 5],
            [9, 4, 2, 1, 5, 3, 8, 6, 7],
            [7, 5, 8, 4, 6, 9, 1, 2, 3],
            [8, 7, 6, 3, 4, 1, 2, 5, 9],
            [5, 2, 4, 6, 9, 8, 3, 7, 1],
            [3, 9, 1, 7, 2, 5, 6, 4, 8],
            [6, 3, 5, 8, 7, 2, 9, 1, 4],
            [2, 1, 7, 9, 3, 4, 5, 8, 6],
            [4, 8, 9, 5, 1, 6, 7, 3, 2]
        ]
  >> invalid_solution = [
            [6, 1, 3, 2, 8, 7, 4, 9, 5],
            [9, 4, 9, 1, 5, 3, 8, 6, 7],
            [7, 5, 8, 4, 6, 9, 1, 2, 3],
            [8, 7, 6, 3, 5, 1, 2, 5, 9],
            [5, 2, 7, 6, 9, 8, 3, 7, 1],
            [3, 9, 1, 7, 3, 5, 6, 4, 8],
            [6, 3, 5, 8, 7, 2, 9, 1, 4],
            [2, 1, 7, 9, 3, 4, 5, 8, 6],
            [4, 8, 9, 5, 1, 6, 7, 3, 2]
        ]
  >> print(Sudoku.Valid(valid_solution, True)) # True
  >> print(Sudoku.Valid(invalid_solution, True)) # False

  >> valid_question = [
            [8, 5, 4, 0, 9, 0, 0, 7, 0],
            [6, 0, 0, 0, 4, 0, 0, 9, 0],
            [0, 9, 1, 7, 0, 0, 4, 6, 5],
            [0, 3, 5, 0, 0, 7, 2, 4, 0],
            [1, 7, 8, 4, 0, 0, 9, 0, 0],
            [0, 4, 6, 5, 0, 0, 0, 8, 3],
            [0, 1, 9, 6, 5, 0, 8, 2, 0],
            [5, 8, 3, 0, 7, 4, 6, 1, 0],
            [0, 0, 0, 0, 0, 0, 5, 0, 7]
        ]
  >> invalid_question = [
            [8, 5, 4, 0, 9, 0, 0, 7, 0],
            [6, 0, 2, 0, 4, 0, 0, 9, 0],
            [0, 9, 1, 7, 0, 0, 4, 6, 5],
            [0, 3, 5, 0, 0, 7, 9, 4, 0],
            [1, 7, 8, 4, 0, 0, 2, 0, 0],
            [0, 4, 6, 5, 0, 0, 0, 8, 3],
            [0, 1, 9, 6, 5, 0, 8, 2, 0],
            [5, 8, 3, 0, 7, 4, 6, 1, 0],
            [0, 0, 0, 0, 0, 0, 5, 0, 7]
        ]
  >> print(Sudoku.Valid(valid_question, False)) # True
  >> print(Sudoku.Valid(invalid_question, False)) # False
  ```
