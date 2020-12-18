import random
from collections import Counter

class Hat:
    def __init__(self, **color_num):
        #_ color_num: dict
        #_ contentss should be a list of strings containing one item for each ball in the hat. 
        self.contents = []
        for color in color_num:
            for _ in range(color_num[color]):
                self.contents.append(color)
        if not self.contents:
            print('nothing in the hat, are you sure?')
        
        #_ for reset every time....
        self.reset_data = self.contents[:]
    
    def draw(self, num):
        '''
        This method should remove balls at random from contentss
        and return those balls as a list of strings.
        The balls should not go back into the hat during the draw,
        similar to an urn experiment without replacement.
        If the number of balls to draw exceeds the available quantity,
        return all the balls.
        '''
        '''
        Expected hat draw to reduce number of items in contents.
        For reproducibility, reset to the original state every time.
        '''
        self.contents = self.reset_data[:]
        if not num:
            return self.contents
        if num >= len(self.contents):
            res = self.contents[:]
            self.contents = []
            return res

        res = []
        for __ in range(num):
            pick = random.randint(0, len(self.contents) - 1)
            res.append(self.contents.pop(pick))
        return res

def experiment(
    hat, expected_balls, num_balls_drawn,
    num_experiments):
    '''
    hat: A hat object containing balls that should be copied inside the function
    expected_balls: dict(Counter), An object indicating the exact group of balls to attempt to draw from the hat for the experiment.
    num_balls_drawn: The number of balls to draw out of the hat in each experiment.
    num_experiments: The number of experiments to perform.
    The experiment function should return a probability.
    '''
    if not num_experiments:
        return 0
    
    success = 0
    for _ in range(num_experiments):
        cnt = Counter(hat.draw(num_balls_drawn))
        result = 0
        for color in expected_balls:
            if (color not in cnt or
                cnt[color] < expected_balls[color]):
                break
            result += 1
        if result == len(expected_balls):
            success += 1
    
    return success / num_experiments
