'''
demo_project_Python/201115 Tic-tac-toe game
Author: Elie-Yen
Python version: 3.6
'''
import numpy as np
import random as rd
class MyGame:
    def __init__(self):
        #_ initialize and conversations
        self.level = 0
        self.order = [] #_ who's play now
        self.board = np.ones((3,3))
        self.symbol = {2: 'X', 3: 'O'} # 2: user; 3: computer
        self.rec = {'row': [1, 1, 1], 'col': [1, 1, 1], 'dg': [1, 1]}
        self.cnt = {8: 0, 27: 0, 'draw': 0 }
        self.welcome = ('\nWelcome! Please choose game level' +
                        '\n(type : [ end ] to end at anytime)' +
                        '\ntype : [ easy / mid / master ]\n')
        self.order_ask = ('\nYou choose [ {0} ] level' +
                         '\nWould you wanna start first?\ntype : [ y / n ]\n')
        self.order_re = "\nTHX, you're a nice guy! I'll start first!\n\n"
        self.pos_ask = ('\nWhere do you wanna put X?' +
                        '\n(valid number range: 0,1,2 ex: 2, 2 )' +
                        '\ntype : [ row, column ]')
        self.pos_re = {8: ('\nYou put X at ({0}, {1}),\n\n {2}' +
                            "\n\nnow it's my turn!\n"), 
                       27: ("I'm done! I put O at ({0}, {1}),\n\n {2}" +
                            '\n\n' + self.pos_ask)}
        self.error_msg =  ('\nSorry, invalid input: " {0} " ' +
                        'please type again!\ntype : [ {1} ]')
        self.error_pos = ('\nSorry, ({0}, {1}) is occupied!\n' +
                        'please type again!\ntype : [ row, column ]\n')
        self.gameover = 'Play again?\ntype : [ play / end ]\n'
        self.win_msg = {8: ('\nYou put X at ({0}, {1}),\n' +
                            '\n ====Congrats! you won====\n\n{2}' +
                            '\n' + '=' * 25 + '\n' + self.gameover),
                        27: ("I'm done! I put O at ({0}, {1}),\n" +
                            '\n =======Good  Game!=======\n\n{2}' +
                            '\n' + '=' * 25 + '\n' + self.gameover),
                        (8, 'draw'): ('\nYou put X at ({0}, {1}),\n' + 
                            '\n =========A draw!=========\n\n{2}' +
                            '\n' + '=' * 25 + '\n' + self.gameover),
                        (27, 'draw'): ("I'm done! I put O at ({0}, {1}),\n" + 
                            '\n =========A draw!=========\n\n{2}' +
                            '\n' + '=' * 25 + '\n'+ self.gameover)}
        self.end_fuc = ('\n' + '=' * 25 + '\n' +
                         ' End the game\n THX for playing MyGame!!!' +
                         '\n' + '=' * 25 + '\n')
        print(self.welcome)
        self.MainPlay(input())
    
    def MainPlay(self, msg):
        #_ the main process interacting with player
        if msg == 'end':
            if sum(self.cnt.values()):
                result = ('\ntotal: {0}\nPlayer: {1} %' +
                         '\nComputer: {2} %\nDraw: {3} %')
                s = sum(self.cnt.values())
                print(result.format(s,
                        round(100 * self.cnt[8] / s, 2), 
                        round(100 * self.cnt[27] / s, 2),
                        round(100 * self.cnt['draw'] / s, 2)))
            print(self.end_fuc)
            return 

        msg = msg.replace(' ','').lower()

        #_ choose level
        if not self.order:
            if msg in ('easy', 'mid', 'master'):
                self.level = msg
                self.order.append(self.level)
                print(self.order_ask.format(msg))
            else:
                print(self.error_msg.format(msg,'easy / mid / master'))

        #_ ask who play first(order)
        elif self.order == [self.level]:
            if msg == 'y':
                print(self.pos_ask)
                self.order.append(2)
            elif msg == 'n':
                print(self.order_re)
                print(self.ComputerPlay())
            else:
                print(self.error_msg.format(msg, 'y / n'))

        #_ start play    
        elif self.order[-1] != 'end':
            pos = msg.split(',')
            if len(pos) == 2 and set(pos).issubset({'0', '1', '2'}):
                row, col = int(pos[0]), int(pos[1])
                if self.board[row, col] == 1:
                    self.order.append(2)
                    self.Put(row, col)
                else:
                    print(self.error_pos.format(row, col))
            else:
                print(self.error_msg.format(msg, 'row, column'))
        
        #_ game over
        else:
            #_ play again
            if msg == 'play':
                self.board = np.ones((3, 3))
                self.rec = {'row': [1, 1, 1], 'col': [1, 1, 1], 'dg': [1, 1]}
                self.order.clear()
                print(self.welcome)
            else:
                print(self.error_msg.format(msg, 'play, end'))
        
        msg = input()
        self.MainPlay(msg) # continue
    
    def Put(self, row, col):
        val = self.order[-1]
        self.board[row, col] = val
        self.rec['row'][row] *= val
        self.rec['col'][col] *= val

        #_ diagonal update
        tmp = row * 3 + col
        if not tmp % 2:
            self.rec['dg'][(tmp % 4) // 2] *= val
            if tmp == 4:
                self.rec['dg'][1] *= val
        return self.Winner(row, col)
    
    def Winner(self, row, col, res = ''):
        #_ for output board beautifully
        show = np.full((3,3), '_')
        for r in range(3):
            for c in range(3):
                if self.board[r, c] > 1:
                    show[r][c] = self.symbol[self.board[r, c]]
        
        cur = self.order[-1] ** 3
        tmp = [self.rec['row'][row], self.rec['col'][col]]
        
        #_ a draw!
        if 1 not in self.board:
            self.cnt['draw'] += 1
            self.order.append('end')
            print(self.win_msg[(cur, 'draw')].format(row, col, show))
        #_ someone win!
        elif cur in tmp or cur in self.rec['dg']:
            self.cnt[cur] += 1
            self.order.append('end')
            print(self.win_msg[cur].format(row, col, show))
        #_ keep playing
        else:
            print(self.pos_re[cur].format(row, col, show))
            if cur == 8:
                print(self.ComputerPlay()) #_ after respond, computer play
        return ' '

    def ComputerPlay(self):
        self.order.append(3)
        empty, line = [], []
        #_ test if anyone's about to line up in next turn
        for r in range(3):
            for c in range(3):
                if self.board[r, c] == 1:
                    tmp = {self.rec['row'][r], self.rec['col'][c]}
                    i = r * 3 + c
                    #_ add diagonal if it needs
                    if not i % 2:
                        tmp.add(self.rec['dg'][(i % 4) // 2])
                        if i == 4:
                            tmp.add(self.rec['dg'][1])
                    if 9 in tmp:
                        #_ computer gonna win
                        return self.Put(r, c)
                    if 4 in tmp:
                        #_ not return instantly
                        #_ bc computer might line up later
                        line.append((r, c))
                    empty.append((i))
        
        #_ computer does't line up; but player does
        if line:
            r, c = rd.choice(line)
            return self.Put(r, c)
        
        #_ no one is about to line up in next turn
        pos = rd.choice(empty) # default value for easy level

        #_ special strategies
        if self.level in {'mid', 'master'}:
            f = self.board.flatten()
            s1, s2, s3, s4 = set(), set(), set(), set()
            s5, s6, s7, s8 = set(), set(), set(), set()
            d = {0: [1, 3, 6, 2], 2: [1, 5, 8, 0],
                 6: [7, 3, 0, 8], 8: [7, 5, 2, 6]} # adjacent pos
            corner = set(filter(lambda a: f[a] == 1, d))
           
            if len(empty) > 5 :    
                if len(corner) == 4:
                    if 3 not in self.board and 2 not in self.board: # beginning
                        pos = rd.choice([0, 2, 4, 6, 8])
                    elif f[4] > 1:
                        pos = rd.choice([0, 2, 6, 8])
                    return self.Put(pos // 3, pos % 3)

                elif len(corner) == 3: # s0
                    if f[4] == 1:
                        pos = 4
                    else:
                        pos = 8 - (set(d) - corner).pop()
                    return self.Put(pos // 3, pos % 3)
                
                elif f[4] == 3 and {f[0] * f[8], f[2] * f[6]} == {1, 4}:
                    for x in {1, 3, 5, 7}:
                        if x in empty:
                            s1.add(x)
            
            for x in corner:
                for i in {0, 1}:
                    if (f[4] * f[d[x][i]] in {4, 9} and
                        f[8 - x] * f[d[x][i ^ 1 + 2]] == 1):
                        s2.add(x)
                    if (f[d[x][i]] * f[d[x][i + 2]] in {4, 9} and
                        f[d[x][i ^ 1]] * f[d[x][i ^ 1 + 2]] == 1):
                        s3.add(x)
                    if (f[4] * f[d[x][i ^ 1 + 2]] in {4, 9} and
                        f[8 - x] * f[d[x][i]] == 1):
                        s4.add(x)
                    if (f[8 - x] * f[d[x][i ^ 1 + 2]] in {4, 9} and
                        f[4] * f[d[x][i]] == 1):
                        s5.add(x)
                if (f[d[x][0]] * f[d[x][1]] in {4, 9} and
                    f[d[x][2]] * f[d[x][3]] == 1):
                    s6.add(x)
                if (f[d[x][2]] * f[d[x][3]] in {4, 9} and
                    f[d[x][0]] * f[d[x][1]] == 1):
                    s7.add(x)
                if (f[d[x][1]] * f[8 - x] in {4, 9} and
                    f[d[x][2]] * f[4] == 1):
                    s8.add(x)
            
            if (self.level == 'master' and (s1 or s2 or s3 or s6)):
                if s1:
                    pos = rd.choice(list(s1))
                else:
                    candidate = list(s2 | s3 | s6)
                    pos = rd.choice(candidate)
            elif s4 or s5 or s7 or s8:
                candidate = list(s4 | s5 | s7 | s8)
                pos = rd.choice(candidate)
        
        return self.Put(pos // 3, pos % 3)

#_ special strategies
'''
s1(d6)   s2(bsT4)          s3 (sL2)        s4 (bsT2)
P _ _    _ ? _   _ P _     * C _  * _ C    C P _  P ? _
_ C *    C C P   ? C ?     _ ? ?  C ? ?    _ C ?  ? C P
_ _ P    * ? _   * C _     C ? ?  _ ? ?    * ? P  * _ C

s5 (bsT1)       s6(sL3)    s7(bL1)   s8        s0
? ? C  c P C    * C _      C ? ?     _ P C     _ _ C
? _ P  _ _ ?    C ? ?      _ P ?     C _ ?     _ ? _
* _ C  * ? ?    _ ? ?      * _ C     * ? ?     * _ _
'''
