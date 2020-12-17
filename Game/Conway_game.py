'''
Elie Yen
Conway's Game of life
'''
import numpy
import math
import timeit
def get_generation(cells, generations):
    if len(cells) < 1:
        return []

    #_ the direction of adjacent cells
    adj = ((-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1),
           (1, -1), (1, 0), (1, 1))
    #_ store how many live neighbors, > 0: current living cells
    adj_live = dict(((r, c), 2 * cells[r][c] - 1)
                for c in range(len(cells[0])) for r in range(len(cells)))
    print(cells)
    for _ in range(generations):

        #_ calculate how many adj live cells
        for row in range(len(cells)):
            for col in range(len(cells[0])):
                if not cells[row][col]:
                    continue
                for r, c in adj:
                    if (row + r, col + c) in adj_live:
                        adj_live[(row + r, col + c)] *= 2
                        #_ live/ dead cells that has 3+ live neighbors will die
                        if adj_live[(row + r, col + c)] in (16, -16):
                            adj_live[(row + r, col + c)] = 0
                        
                    #_ live cell is on boarder, consider expand bc
                    #_ the original dead cell might become alive
                    else:
                        adj_live[(row + r, col + c)] = -2
        
        #_ check the status of next generation
        #_ next generation's range of width, height after trim empty row/ col
        min_h, min_w = math.inf, math.inf
        h, w = -math.inf, -math.inf
        new_live = set()
        for row, col in adj_live:
            #_ live cell with 2/3 live neighbors/ dead cell with 3 neighbors
            if adj_live[row, col] in (4, 8, -8):
                h, min_h = max(h, row), min(min_h, row)
                w, min_w = max(w, col), min(min_w, col)
                new_live.add((row, col))

        #_ no live cells in next generation
        if not new_live:
            print([])
            return []

        #_ update the adj_live of next generation
        cells = []
        adj_live = dict()
        for r in range(h - min_h + 1):
            tmprow = []
            for c in range(w - min_w + 1):
                Val = int((r + min_h, c + min_w) in new_live)
                adj_live[((r, c))] = Val * 2 - 1
                tmprow.append(Val)
            cells.append(tmprow)

        print(cells)
    
    return cells
                    





cells = numpy.random.randint(2, size=(3, 3))
get_generation(cells, 2)

