## Game projects

Welcome! Here are some game-related projects written by Python and use some libs like Numpy.

---

  ### Tic-tac-toe Game
  * Description:
    An interactive game that the player can challenge the computer with 3 levels: easy, mid and master,
    also available to choose who plays first and end the game at any time.
    It would automatically compute who wins the game this time and restart the game if the player wants to play again.
    Once the player ends it, a statistic of how many games the player has won/ lose/ draw would show up on the screen. 
    This could be used in further data analysis.
  * Concept:
    By setting player = 2, computer = 3 and empty = 1, it could be very useful to check if someone is about to get 3 in a row
    and decide where to put.
  * libs:   
    Numpy (3rd-party):  Speed up and calculate complex algorithm.   
    Random (Built-in):  Increase unpredictable moves in easy level and some of mid & master level.   
    time (Built-in): Produce asynchronous outputs for better experience.   
  * Preview:
      * start play
     ```
      Welcome! Please choose game level
      (type : [ end ] to end at anytime)
      type : [ easy / mid / master ]
      
      >> no
      
      Sorry, invalid input: " no " please type again!
      type : [ easy / mid / master ]
       
      >> master
       
      You choose [ master ] level
      Would you wanna start first?
      type : [ y / n ]
      >> n

      THX, you're a nice guy! I'll start first!
      I'm done! I put O at (2, 2),

       [['_' '_' '_']
       ['_' '_' '_']
       ['_' '_' 'O']]
    ```
    * end of the game
    ```
    I'm done! I put O at (1, 0),

     =======Good  Game!=======

    [['O' 'X' '_']
     ['O' 'X' '_']
     ['O' '_' 'X']]
    =========================
    Play again?
    type : [ play / end ]
     end

    total: 2
    Player: 0.0 %
    Computer: 50.0 %
    Draw: 50.0 %

    =========================
     End the game
     THX for playing MyGame!!!
    =========================

    <__main__.MyGame at 0x53540a0>
    ```
---

### Sudoku Solver
Sudoku is a 2-D array. A valid solution of Sudoku must has unique numbers in each row, column and 3x3 boxes.
Use backtracking to try different combinations (in-place modify) until reach the answer. (leetcode problem)
* preview
  * input:
    ```
    [["5","3",".",".","7",".",".",".","."],
    ["6",".",".","1","9","5",".",".","."],
    [".","9","8",".",".",".",".","6","."],
    ["8",".",".",".","6",".",".",".","3"],
    ["4",".",".","8",".","3",".",".","1"],
    ["7",".",".",".","2",".",".",".","6"],
    [".","6",".",".",".",".","2","8","."],
    [".",".",".","4","1","9",".",".","5"],
    [".",".",".",".","8",".",".","7","9"]]
    ```
  * Output:
    ```
    [["5","3","4","6","7","8","9","1","2"],
    ["6","7","2","1","9","5","3","4","8"],
    ["1","9","8","3","4","2","5","6","7"],
    ["8","5","9","7","6","1","4","2","3"],
    ["4","2","6","8","5","3","7","9","1"],
    ["7","1","3","9","2","4","8","5","6"],
    ["9","6","1","5","3","7","2","8","4"],
    ["2","8","7","4","1","9","6","3","5"],
    ["3","4","5","2","8","6","1","7","9"]]
    ```
