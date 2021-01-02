'''
Elie Yen
Python 3
'''
from random import random, choice, shuffle
def PasswordGenerate(length = 16, flag_upper = 1, flag_special = 0):
    if length < 6:
        return "Type bigger length for your security"
    digits = str(random())[2: ] + '0123456789'
    lower = 'abcdefghijklmnopqrstuvwxyz'
    upper = lower.upper()
    special = '!"#$%&*+-,.@?^~'
    chars = lower + digits
    res = [choice(lower), choice(digits)]
    length -= 2

    #_ ensure at least 1 upper and special if need
    if flag_special:
        res.append(choice(special))
        chars += special
        length -= 1
    if flag_upper:
        res.append(choice(upper))
        chars += upper
        length -= 1
    
    for _ in range(length):
        res.append(choice(chars))
    
    #_ shuffle
    shuffle(res)
    return ''.join(res)

PasswordGenerate(16, 1, 1)   

    