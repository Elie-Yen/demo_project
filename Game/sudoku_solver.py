"""
Elie Yen
Python 3
Sudoku solver v2
"""
import numpy as np

class Sudoku:

    @classmethod
    def __backtracking__ (cls, board, cols, box, places, mode = False):
        '''
        core program to create/ solve a sudoku by in-place modify
        cols and box: dict, record used numbers in cols and 3 * 3 boxes
        places: list of tuple, empty (unknow) place to be filled
        mode: bool, True for creator, False for solver
        return: none (in-place modify)
        '''
        rng = np.random.default_rng() # random generator
        nums = np.arange(1, 10)

        #_ assistant sub functions
        def renew(row, col, num):
            board[row][col] = num
            cols[col][row] = num
            box[(row // 3, col // 3)][row % 3 + (col % 3) * 3] = num
            
        def valid(row, col, num):
            if (num in board[row] or num in cols[col] or
               num in box[(row // 3, col // 3)]):
                return False
            return True
        
        def backtrack():
            #_ reach the end
            if not places:
                return True
            
            #_ create mode, change the order to generate random board
            if mode:
                rng.shuffle(nums)

            row, col = places[-1]
            for num in nums:
                if valid(row, col, num):
                    renew(row, col, num)
                    places.pop()
                    
                    #_ everywhere is filled, the num is correct 
                    if backtrack():
                        return True
                    
                    #_ impossible combination, remove and try next num
                    else:
                        renew(row, col, 0)
                        places.append((row, col))
            
            return False
        
        #_ implement
        backtrack()
        return 

    @classmethod
    def __get_cols_box_place_valid_helper__(cls, board, mode = False):
        '''
        For validation of questions and solution
        board: List[ List[ integer] ]
        mode: bool, True for solution, False for question
        return: tuple( cols, box, places, valid )
        cols and box: dict, record used numbers in cols and 3 * 3 boxes
        places: list of tuple, empty (unknow) place to be filled
        valid: bool indicate whether the board is valid
        (only contains appropriate numbers, no repetitions in box, cols, rows ...)
        '''
        box = dict()
        cols = dict()
        places = []
        valid = False

        #_ ensure it's 9 * 9
        if len(board) != 9:
            return cols, box, places, valid
        for i in range(9):
            if len(board[i]) != 9:
                return cols, box, places, valid

        
        nums = np.arange(1, 10) if mode else np.arange(10)
        
        for r in range(9):
            key = (r // 3, r % 3)
            box[key] = [0] * 9
            cols[r] = [0] * 9

        valid = True
        for r in range(9):
            cnt = [0] * 10 # cnt non-zero numbers in row

            for c in range(9):
                if board[r][c] not in nums:
                    valid = False
                    return cols, box, places, valid

                key = (r // 3, c // 3)
                if not board[r][c]:
                    places.append((r, c))
                
                #_ find repetition
                elif (board[r][c] in cols[c] or
                      board[r][c] in box[key] or
                      cnt[board[r][c]]):
                    valid = False
                    return cols, box, places, valid

                cnt[board[r][c]] += 1
                cols[c][r] = board[r][c]
                box[key][r % 3 + (c % 3) * 3] = board[r][c]
 
        #_ if it's a question, check there's a solution or not
        if not mode:
            nums = set(range(1, 10))
            for r, c in places:
                key = (r // 3, c // 3)
                possible = nums - set(board[r]) - set(cols[c]) - set(box[key])
                if not possible:
                    valid = False
                    break

        return cols, box, places, valid

    @staticmethod
    def Valid(board, mode):
        '''
        check the board is a valid question/solution or not
        board:  List[ List[interger] ]
        mode: bool, True for solution, False for question
        rtype: bool that indicates the board is a valid sudoku or not
        '''
        return Sudoku.__get_cols_box_place_valid_helper__(board, mode)[3]
        
    @staticmethod
    def Creator(level=1):
        '''
        randomly produce a sudoku question and a solution by different level
        (there might be more than 1 solution for the question)
        level: integer range from 1 to 3 (inclusive), 1 for easy and 3 for hard
        rtype: a tuple consists of 2 boards (np.array, List[ List[integer] ] )
        tuple[0] is the question, tuple[1] is the solution
        len(board) = len(board[0]) = 9 (2D array, 9 * 9)
        and the integers range from 0-9(inclusive), 0 is unknown place to be filled
        '''
        if level not in (1, 2, 3):
            raise AttributeError(f"Invalid input: '{level}', level could only be 1, 2 or 3")

        solution = np.zeros((9, 9), dtype=int)
        rng = np.random.default_rng() # random generator

        places = []
        box = dict()
        cols = dict()
        
        for r in range(9):
            key = (r // 3, r % 3)
            box[key] = [0] * 9
            cols[r] = [0] * 9
            for c in range(9):
                places.append((r, c))

        #_ the amounts of empty place with number by different level
        level_num = {1: (25, 35), 2: (35, 45), 3: (45, 55)}
        empty_amount = rng.integers(low=level_num[level][0], high=level_num[level][1])
        empty_pos = rng.choice(places, size=empty_amount, replace=False)

        #_ the function changes the passing args
        #_ so we put if after empty_pos has been done (since it needs places) 
        Sudoku.__backtracking__(solution, cols, box, places, True)
        
        #_ the same board as solution, but with empty places
        question = np.array(solution)
        
        for pos in empty_pos:
            question[pos[0]][pos[1]] = 0

        print("* question : ")
        print(question)
        print("* a solution for question : ")
        print(solution)
        return question, solution

    @staticmethod
    def Solver(board):
        '''
        input and return type: np.array or list, List[ List[interger] ]
        '''
        #_ check the question is valid, and generate cols, box, places for calculation
        cols, box, unknown, valid = Sudoku.__get_cols_box_place_valid_helper__(board)
        if not valid:
            print("Error, invalid question, check the input again")
            return board
        
        Sudoku.__backtracking__(board, cols, box, unknown)
        print(board)
        return board


class Test:
    @staticmethod
    def test_create():
        Sudoku.Creator(1)
        Sudoku.Creator(2)
        Sudoku.Creator(3)

        # AttributeError: Invalid input: '5', level could only be 1, 2 or 3
        Sudoku.Creator(5) 

    @staticmethod
    def test_solver():
        question, solution = Sudoku.Creator(2)
        
        print("solution by solver")
        res = Sudoku.Solver(question)
        
        print("compare both solutions")
        print(res == solution) # might slightly be different

    @staticmethod
    def test_valid():
        valid_solution = [
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
        invalid_solution = [
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
        print(Sudoku.Valid(valid_solution, True)) # True
        print(Sudoku.Valid(invalid_solution, True)) # False

        valid_question = [
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
        invalid_question = [
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
        print(Sudoku.Valid(valid_question, False)) # True
        print(Sudoku.Valid(invalid_question, False)) # False
