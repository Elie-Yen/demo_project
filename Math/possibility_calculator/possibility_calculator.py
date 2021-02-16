'''
Elie Yen
Python 3
possibility calculator(v2)
'''
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
from matplotlib.gridspec import GridSpec
from matplotlib.ticker import MultipleLocator

class Hat:
    def __init__(self, contents, seed=None):
        '''
        contents: list of 5 integer [white, red, yellow, green, blue]
        that indicates the amount of balls of each color
        seed: optional, integer to initialize the random generator for reproducibility,
        default is none
        '''
        if len(contents) != 5:
            raise AttributeError('Invalid input, contents must be a list of 5 integer [white, red, yellow, green, blue]')
        if min(contents) < 0:
            raise AttributeError('Invalid input, the minimum amount of color is 0')
        
        self.contents = contents
        self.amount = 0
        self.ball_choice = []
        for i in range(5):
            if self.contents[i]:
               self.ball_choice.append(i)
            self.amount  += contents[i]
        
        #_ random generator
        self.rng = np.random.default_rng(seed)
    
    def draw(self, num):
        '''
        input type: integer
        return type: list of count(order: white, red, yellow, green, blue)

        This method should remove balls at random from contents.
        The balls should not go back into the hat during the draw,
        similar to an urn experiment without replacement.
        If the number of balls to draw exceeds the available quantity,
        return all the balls.
        '''
        if num <= 0:
            raise AttributeError('Invalid input; num must be an integer > 0')
        if num >= self.amount:
            return self.contents
        
        cnt = self.contents[ : ]
        candidates = self.ball_choice[ : ]
        res = [0] * 5
        for _ in range(num):
            #_ set p for not uniform distribution; the cnt affects the color picked
            p = np.array(list(cnt[i] for i in candidates))
            i = self.rng.choice(candidates, p = p / p.sum() )
            cnt[i] -= 1
            res[i] += 1

            if not cnt[i]:
                candidates.remove(i)

        return res
        
class Dice:
    def __init__(self, sides, seed=None):
        '''
        side: int, side/ face of dice
        seed: optional, integer to initialize the random generator for reproducibility,
        default is none
        '''
        self.sides = sides
        self.rng = np.random.default_rng(seed)

    def rolling(self):
        '''
        output: integer of point
        ''' 
        return self.rng.integers(1, self.sides + 1)

class Experiment:
    def __init__(self):
        pass

    def hat_experiment(self, hat, num_balls_drawn, experiment_time, expected_balls):
        '''
        calculate the possibility of successful prediction (occurrence of color-ball)
        return possibility(float) and print statistic charts of occurrence of each color

        hat: A hat object containing balls that should be copied inside the function
        num_balls_drawn: integer, number of balls to draw out of the hat in each experiment.
        experiment_time: integer, number of experiments to perform.
        expected_balls: list of 5 integer [white, red, yellow, green, blue] that
        indicating the exact group attempt to draw from the hat. 
        '''
        if experiment_time <= 0:
            raise AttributeError('Invalid input, experiment time must be a positive integer')
        if len(expected_balls) != 5:
            raise AttributeError('Invalid input, expected balls must be a list of 5 integer [white, red, yellow, green, blue]')
        
        success = 0
        data = []
        cmap = ['#aaaaaa', '#ff6666', '#e6e600', '#8cff66', '#66e0ff']
        fig, (ax1, ax2) = plt.subplots(2,1)

        #_ calculate possibility while generating visualization chart ax1
        for x in range(experiment_time):
            cnt = hat.draw(num_balls_drawn)
            data.append(cnt)
            flag = True
            padding = 0 # sum of other colors so far 
            for i in range(5):
                if cnt[i] < expected_balls[i]:
                    flag = False 
                ax1.bar(x=x, height=cnt[i], bottom=padding, color=cmap[i])
                padding += cnt[i]
            if flag:
                success += 1

        ax1.set_title('color composement')
        ax1.set_xlabel('experiment no.')
        ax1.set_ylabel('number')

        data = pd.DataFrame(data)
        loc = MultipleLocator(1)

        for c in data:
            #_ freq = frequency of occurrence happens
            #_ freq.index = occurrence of ball per draw
            freq = data[c].value_counts().sort_index()
            ax2.plot(freq.index, freq, color=cmap[c])
            
        ax2.set_title('Compare of occurrence')
        ax2.grid(True)
        ax2.xaxis.set_major_locator(loc)
        ax2.set_xlabel('occurrence in each draw')
        ax2.set_ylabel('frequency')

        fig.tight_layout() #_ avoid label overlapping
        plt.show()

        print(success / experiment_time)
        return success / experiment_time

    def dice_experiment(self, dice, num_dices, experiment_time, expected_sum):
        '''
        Rolling num_dices dices at each time,
        return the possibility(float) of having exactly same sum of points as expected,
        and print chart of occurrence of specific point
        
        dice: Dice object;
        expected_sum/ roll_time / experiment_time: integer
        '''
        if experiment_time <= 0:
            raise AttributeError('Invalid input, experiment time must be a positive integer')
        if num_dices < 0:
            raise AttributeError("Invalid input; num of dice can't less than 0")
        if expected_sum > num_dices * dice.sides or expected_sum < num_dices:
            return 0

        data = []
        sum_points = []
        success = 0

        for _ in range(experiment_time):
            _sum = 0
            cnt = np.zeros(dice.sides, dtype=int)
            for __ in range(num_dices):
                point = dice.rolling()
                _sum += point
                cnt[point - 1] += 1
            data.append(cnt)
            sum_points.append(_sum)
            if _sum == expected_sum:
                success += 1

        #_ create figures
        fig = plt.figure(constrained_layout=True)
        gs = GridSpec(2, 2, figure=fig)
        ax1 = fig.add_subplot(gs[0, 0])
        ax2 = fig.add_subplot(gs[1, 0])
        ax3 = fig.add_subplot(gs[:, 1])

        #_ labels and locator
        bins = np.arange(num_dices, num_dices * dice.sides + 1)
        loc = MultipleLocator(1)
        points = np.arange(1, dice.sides + 1)

        #_ data
        df = pd.DataFrame(data, columns=points)
        occurrence = df.sum()

        ax1.pie(occurrence, labels=occurrence.index, autopct='%1.1f%%')
        ax1.axis('equal')
        ax1.set_title('total occurrence ratio of each point')

        ax2.scatter(x=points, y=df.mean())
        ax2.grid(True)
        ax2.xaxis.set_major_locator(loc)
        ax2.set_xlabel('points')
        ax2.set_ylabel('occurrence')
        ax2.set_title('mean occurrence of each point in each rolling')

        ax3.hist(sum_points, bins=bins)
        ax3.grid(True)
        ax3.set_xlabel('sum')
        ax3.set_ylabel('times')
        ax3.set_title('distribution of sum of points')
        
        plt.show()

        print(success / experiment_time)
        return success / experiment_time

class Test:
    def __init__(self):
        self.hat = Hat([35, 51, 43, 38, 27], 1235) # white/red/yellow/green/blue, seed
        self.hat_r = Hat([35, 51, 43, 38, 27]) # no seed
        self.dice = Dice(6, 456) # sides, seed
        self.dice_r = Dice(6) # no seed
        self.exp = Experiment()

    def test_hat_operation(self):
        self.hat.draw(13)
        self.hat_r.draw(13)

    def test_hat_experiment(self):
        expect = [0,3,2,0,0] # 3 red balls + 2 yellow balls
        #_ take 12 balls in each draw, 100 experiments
        self.exp.hat_experiment(self.hat, 12, 100, expect) # 0.56
        self.exp.hat_experiment(self.hat_r, 12, 100, expect)
    
    def test_dice_operation(self):
        self.dice.rolling()
        self.dice_r.rolling()
    
    def test_dice_experiment(self):
        #_ rolling 3 dice in each experiment, 100 experiments
        #_ expect sum of point = 10
        self.exp.dice_experiment(self.dice, 3, 100, 10) # 0.12
        self.exp.dice_experiment(self.dice_r, 3, 100, 10)
