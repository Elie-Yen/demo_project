# demo_project_Python
including several projects using data types,  
string parsing, (dynamic) algorithm,  regular expression, built-in functions, Numpy module.., etc.

  ## Tic-tac-toe Game
  * Description:
    An interactive game that the player can challenge the computer with 3 levels: easy, mid and master,
    also available to choose who plays first and end the game at any time.
    It would automatically compute who wins the game this time and restart the game if the player wants to play again.
    Once the player ends it, a statistic of how many games the player has won/ lose/ draw would show up on the screen. 
    This could be used in further data analysis.
  * Concept:
    By setting player = 2, computer = 3 and empty = 1, it could be very useful to check if someone is about to get 3 in a row
    and decide where to put.
  * Modules used:
    Numpy (3rd-party):  Speed up and calculate complex algorithm.
    Random (Built-in):  Increase unpredictable moves in easy level and some of mid & master level.
  * Preview:
      * start play
     ```
      Welcome! Please choose game level
      (type : [ end ] to end at anytime)
      type : [ easy / mid / master ]
       master # user's answer

      You choose [ master ] level
      Would you wanna start first?
      type : [ y / n ]
       n

      THX, you're a nice guy! I'll start first!
      I'm done! I put O at (2, 2),

       [['_' '_' '_']
       ['_' '_' '_']
       ['_' '_' 'O']]
    ```
    
    * error message
    ```
    Welcome! Please choose game level
    (type : [ end ] to end at anytime)
    type : [ easy / mid / master ]
     no
    Sorry, invalid input: " no " please type again!
    type : [ easy / mid / master ]
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
## Design a singly linked list
Linked list is the most common data structure and is widely used in queue, stacks, hash table ... etc.
But how it works is not straightforward enough to a lot of beginners.
Therefore I write a guideline to explain and use a list to show how it would look like at each stage of operation.
