'''
Elie Yen
Python 3
'''

class Category:

  def __init__(self, category):
    self.name = category
    self.ledger = []
    self.money = 0
    self.spend = 0
    

  def deposit(self, amount, detail = ""):
    '''
    A deposit method that accepts an amount and description. If no description is given, it should default to an empty string. The method should append an object to the ledger list in the form of {"amount": amount, "description": description}.
    '''
    self.ledger.append({"amount": amount, "description": detail})
    self.money += amount

  def withdraw(self, amount, detail = ""):
    '''
    A withdraw method that is similar to the deposit method, but the amount passed in should be stored in the ledger as a negative number. If there are not enough funds, nothing should be added to the ledger. This method should return True if the withdrawal took place, and False otherwise.
    '''
    if self.check_funds(amount):
      self.ledger.append({"amount": -amount, "description": detail})
      self.money -= amount
      self.spend += amount
      return True
    return False
  
  def get_balance(self):
    '''
    A get_balance method that returns the current balance of the budget category based on the deposits and withdrawals that have occurred.
    '''
    return self.money
  
  def transfer(self, amount, category):
    '''
    A transfer method that accepts an amount and another budget category as arguments. The method should add a withdrawal with the amount and the description "Transfer to [Destination Budget Category]". The method should then add a deposit to the other budget category with the amount and the description "Transfer from [Source Budget Category]". If there are not enough funds, nothing should be added to either ledgers. This method should return True if the transfer took place, and False otherwise.
    '''
    if self.check_funds(amount):
      self.withdraw(amount, "Transfer to {0}".format(category.name))
      category.deposit(amount, "Transfer from {0}".format(self.name))
      return True
    return False

  def check_funds(self, amount):
    '''
    A check_funds method that accepts an amount as an argument. It returns False if the amount is greater than the balance of the budget category and returns True otherwise. This method should be used by both the withdraw method and transfer method.
    '''
    return amount <= self.money
  
  def snum(self, num):
      '''
      for convenience of translating numbers into strings
      '''
      if num == int(num):
          return str(num) + '.00'
      return (str(round(num, 2)) if round(num, 2) == num
              else str(num)[ :7])


  def __repr__(self):
    '''
    A title line of 30 characters where the name of the category is centered in a line of * characters.A list of the items in the ledger. Each line should show the description and amount. The first 23 characters of the description should be displayed, then the amount. The amount should be right aligned, contain two decimal places, and display a maximum of 7 characters.A line displaying the category total.
    '''
    #_ title
    n = (30 - len(self.name)) // 2
    res = ('*' * n + self.name + '*' * n).ljust(30, '*') + '\n'

    #_ content
    for i in range(len(self.ledger)):
      num = self.snum(self.ledger[i]["amount"])
      des = str(self.ledger[i]["description"])[ :23]
      res += des + ' ' + (num.rjust(29 - len(des))) + '\n'
    res += "Total: " + self.snum(self.money)
    return res

#_ another function
def create_spend_chart(categories):
    '''
    takes a list of categories as an argument. It should return a string that is a bar chart.
    The chart should show the percentage spent in each category passed in to the function. The percentage spent should be calculated only with withdrawals and not with deposits. Down the left side of the chart should be labels 0 - 100. The "bars" in the bar chart should be made out of the "o" character. The height of each bar should be rounded down to the nearest 10. The horizontal line below the bars should go two spaces past the final bar. Each category name should be vertacally below the bar. There should be a title at the top that says "Percentage spent by category".
    Percentage spent by category
    '''
    longest = 0 # longest name 
    _sum = 0
    for c in categories:
      longest = max(longest, len(c.name))
      _sum += c.spend

    #_ seems everything in chart must be fixed width filled with ' '
    x_axis = list('    ' for i in range(longest))
    y_axis = list('' for i in range(11))
    y_axis[10] += ' ' #_ idk, according to the answer
    for c in categories:
        #_ handle the chart
        percent = int(100 * c.spend / _sum) + 1
        for score in range(10):
            y_axis[score] += ' o ' if score * 10 <= percent else '   '

        #_ handle the vertial category 
        for i in range(longest):
            x_axis[i] += ' ' + c.name[i] + ' ' if i < len(c.name) else '   '
    
    #_ print the chart
    chart = "Percentage spent by category\n"
    width = 2 * (len(categories) + 2)
    #_ reversed order, 
    for i in range(10, -1, -1):
        chart += str(i * 10).rjust(3) + '|' + y_axis[i] + ' \n'
    
    chart += '    ' + '-' * width + '\n'
    
    for i in range(len(x_axis)):
        chart += x_axis[i] + '\n'
    
    return chart
    
