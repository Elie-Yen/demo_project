'''
Elie Yen
Python version: 3.6
Conway's Game of life
'''
import numpy
import math
def get_generation(cells, generations):

    #_ the direction of adjacent cells
    adj = ((-2, -2), (-2, -1), (-2, 0), (-1, -2), (-1, 0),
           (0, -2), (0, -1), (0, 0))

    def status(cells, cur):

        print("\ngeneration{0}\n".format(cur), cells)
        if not generations or len(cells) < 1 or cur == generations:
            return cells
        
        #_ expand 1 cells in each border
        #_ 1 for live cells, -1 for dead cells
        h, w = len(cells), len(cells[0])
        next_cells = numpy.full((h + 2, w + 2), -1, dtype = numpy.int8)
        next_cells[1: -1, 1: -1] = cells[:]

        #_ new height, width of next generation
        nh, nw = -math.inf, -math.inf
        min_h, min_w = math.inf, math.inf
        
        for row in range(len(next_cells)):
            for col in range(len(next_cells[0])):
                 #_ calculate how many adj live cells
                 #_ next_cells[i + 1][j + 1] = cells[i][j]
                for r, c in adj:
                    if (-1 < row + r < h and -1 < col + c < w and
                        cells[row + r, col + c]):
                        next_cells[row, col] *= 2
                        #_ cells that have 3+ live neighbors will die
                        if next_cells[row, col] in (16, -16):
                            next_cells[row, col] = 0 
                            break

                #_ check next status of cell by its value
                #_ update range of width, height after trim empty row/ col
                if next_cells[row, col] in (4, 8, -8):
                    nh, min_h = max(nh, row), min(min_h, row)
                    nw, min_w = max(nw, col), min(min_w, col)
                    next_cells[row, col] = 1
                else:
                    next_cells[row, col] = 0 

        #_ if no live cells, cells = []
        #_ else trim the empty rows/ cols of next generation
        cells = ([] if min_h == min_w == -nh == -nw == math.inf
                else next_cells[min_h: nh + 1, min_w: nw + 1])
        status(cells, cur + 1)

    return status(cells, 0) 




#_ test
cells = numpy.random.randint(2, size=(3, 5))
get_generation(cells, 5)
