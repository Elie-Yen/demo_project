def solveSudoku(self, board: List[List[str]]):
        box = dict(((i // 3, i % 3), []) for i in range(9))
        cols = dict((i, []) for i in range(9))
        unknown = []
        
        for row in range(9):
            for col in range(9):
                if board[row][col] == '.':
                    unknown.append((row, col))
                cols[col].append(board[row][col])
                box[(row // 3, col // 3)].append(board[row][col])

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
            if not unknown:
                return True
            
            row, col = unknown[-1]
            for num in '123456789':
                if valid(row, col, num):
                    renew(row, col, num)
                    unknown.pop()
                    
                    #_ no unknown, the num is correct 
                    if backtrack(): 
                        return True 
                    
                    #_ still unknown remained, remove and try next num
                    else:
                        renew(row, col, '.')
                        unknown.append((row, col))
            return
        
        backtrack()
