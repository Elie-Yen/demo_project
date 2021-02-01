"""
Python 3
Elie Yen
budget recorder
"""

import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import matplotlib.dates as mdates
from datetime import date, timedelta
import random
from io import StringIO

###### budgets contains multiple category and further operations #####
class Budgets:
    def __init__(self):
        #_ record overview of categories and spend of that category
        self.categories = dict()

        self.all_asset = 0

        #_ record all transactions
        #_ format: date / monthlydate / spend / deposit / category_name / description
        self.history = []

    def create_category(self, category_name):
        if category_name in self.categories:
            raise ValueError(f"{category_name} has already been created.")
        elif category_name == "deposit":
            raise ValueError(f"{category_name} is invalid, try another.")
        self.categories[category_name] = 0

    def get_balance(self):
        return self.all_asset

    def deposit(self, amount, _date = date.today()):
        '''
        amount: number
        date: date object
        '''
        if amount <= 0:
            raise ValueError("Invalid amount input, try again")
        self.all_asset += amount
        monthlydate = _date.replace(day = 1)
        self.history.append([_date, monthlydate, 0, amount, "deposit", ""])
        
    def spend_record(self, category_name, amount, _date = date.today(), detail = ""):
        if amount <= 0:
            raise ValueError("Invalid amount input, try again")
        if category_name not in self.categories:
            raise ValueError("Invalid category_name input, try again")

        self.categories[category_name] += amount
        monthlydate = _date.replace(day = 1)
        self.history.append([_date, monthlydate, -amount, 0, category_name, detail])
        self.all_asset -= amount

    def __repr__(self):
        #_ provide current status overview

        res = f"*\ncurrent total asset: {self.all_asset}\n\n"
        for c in self.categories:
            res += f"*{c} \n spend:  {self.categories[c]}\n"
        return res

    def get_dataframe(self, detail = True):
        """
        turn into dataframe to export and analyze
        """
        arr = np.array(sorted(self.history))
        date_index = arr[ : , 0]
        
        if detail:
            df = pd.DataFrame(
                arr[ : , 2 : ], columns = ["Spend", "Deposit", "Category", "Detail"],
                index = date_index
                )
        else:
            #_ this is for analyze
            df = pd.DataFrame(
                arr[ : , 1 : -1], columns = ["MonthlyDate", "Spend", "Deposit", "Category"],
                index = date_index
                )
        return df
    
    def export(self, path_or_buf = "C:\\Users\\user\\Desktop\\budget_test.csv"):
        #_ export to a csv file
        df = self.get_dataframe()
        return df.to_csv(path_or_buf)

######## Another Analysis functions ##########
class Analyze(Budgets):
    def __init__(self, Budgets):
        self.budget = Budgets
        self.df = self.budget.get_dataframe(False)

    def get_next_month(self, _date):
        """
        return date object that is start of _date's next month
        """
        m = _date.month + 1
        y = _date.year
        if m > 12:
            y += 1
            m %= 12
        res = _date.replace(year=y, month= m, day= 1)
        return res
    
    def monthly_spend_overview(self, target_date):
        """
        analysis spend of target month (= target_date.month)
        output: none, print dataframe and pie chart
        """
        #_ select data
        start_date = target_date.replace(day=1)
        end_date = self.get_next_month(start_date)
        data = self.df[(self.df.index >= start_date) & (self.df.index < end_date)]
        data = data.drop(columns='Deposit')

        #_ aggregate, filtering data
        spend = data.groupby(["Category"])["Spend"].sum()
        spend = spend.drop(index='deposit') #_ spend of each category(except deposit)
        labels = spend.index #_ name of categories
        
        fig, ax = plt.subplots()
        ax.pie(-spend, labels=labels, autopct='%.1f%%', startangle=90)
        ax.axis('equal')  # Equal aspect ratio ensures that pie is drawn as a circle.
        ax.set_title(f'your monthly spend overview between {start_date} and {end_date}')
        ax.legend(loc='upper left', frameon=False)

        print(spend)
        plt.show()
        
    def history_overview(self, start_date, end_date):
        """
        shows the change of asset in specific period
        only start_date is included
        """
        #_ get length of time by month to ensure at least one month
        start_date = start_date.replace(day = 1)
        end_date = end_date.replace(day = 1)
        n = (end_date.year - start_date.year)* 12 + end_date.month - start_date.month

        if n < 1:
            raise ValueError("Invalid range of time")

        #_ select data
        data = self.df[(self.df.index >= start_date) & (self.df.index < end_date)]

        #_ calculate accumulate asset from start_date
        monthlydata = data.groupby(["MonthlyDate"]).sum()
        accum = np.array(monthlydata["Spend"] + monthlydata["Deposit"])
        X2 = np.array(monthlydata.index)

        for i in range(1, len(accum)):
            accum[i] += accum[i - 1]
        accum = pd.DataFrame(accum, index = X2)
        
        deposit_rec = data["Deposit"]
        spend_rec = data["Spend"]
        
        X = data.index

        #_ get visualize, share same xaxis(date)
        fig, (ax1, ax2) = plt.subplots(nrows=2, ncols=1, sharex=True)

        #_ show accum, spend , deposit rec
        ax1.plot(X2, accum, label = 'accumulated asset')
        ax2.bar(X, spend_rec, facecolor='#ff9999', edgecolor='white', label = 'spend')
        ax2.bar(X, deposit_rec, facecolor='#9999ff', edgecolor='white', label = 'deposit')
        
        #_ set legeng, ticker and format
        ax1.grid(True)
        ax1.legend(loc='upper left', frameon=False)
        ax2.legend(loc='upper left', frameon=False)
        months = mdates.MonthLocator()  # every month
        ax1.xaxis.set_major_locator(months)
        ax1.xaxis.set_ticks_position('bottom')
        ax2.xaxis.set_major_locator(months)
        ax2.xaxis.set_ticks_position('bottom')
        
        ax1.set_title(f'your accumulated asset between {start_date} and {end_date}')
        ax2.set_title(f'your spend/deposit between {start_date} and {end_date}')

        ax2.set_xlabel('date')
        ax1.set_ylabel('money')
        ax2.set_ylabel('money')
        plt.show()


###### test ######
class Test():
    def __init__(self):
        self.bd = Budgets()
        self.bd.create_category("food")
        self.bd.create_category("cloth")
        self.bd.create_category("medicine")
        self.bd.create_category("entertainment")

    def test_operation(self):
        
        self.bd.spend_record("food", 32, date(2021, 4, 21))
        self.bd.spend_record("cloth", 52, date(2021, 4, 21), "for performance")
        self.bd.deposit(1200, date(2021, 3, 15))
        self.bd.spend_record("entertainment", 100, date(2021, 5, 6), "dancing show")
        self.bd.spend_record("cloth", 23, date(2021, 5, 6))
        self.bd.spend_record("food", 200, date(2021, 3, 2))
        self.bd.deposit(1000, date(2021, 3, 7))
        self.bd.spend_record("cloth", 120, date(2021, 1, 5))
        self.bd.spend_record("food", 120, date(2021, 2, 3))
        self.bd.deposit(2000, date(2021, 1, 27))
        self.bd.spend_record("food", 150, date(2021, 3, 6))
        self.bd.spend_record("cloth", 130, date(2021, 2, 6))
        self.bd.spend_record("food", 200, date(2021, 3, 2))

        print(self.bd)
    
    def test_export(self):
        self.bd.export()

    def test_analyze_rnddata(self):
        #_ for produce random data to test(without description)
        category = ["food", "cloth", "medicine", "entertainment"]
        spends = np.random.randint(low=20 , high= 100, size=100)
        for i in range(100):
            cate = random.choice(category)
            d = random.randint(1, 28)
            m = random.randint(1, 12)
            self.bd.spend_record(cate, spends[i], date(2021, m, d))
        
        deposits = np.random.randint(low=100 , high= 1000, size=20)
        for i in range(20):
            d = random.randint(1, 28)
            m = random.randint(1, 12)
            self.bd.deposit(deposits[i], date(2021, m, d))


    def test_analyze_month(self):
        test_mo_date = date(2021, 3, 26)
        analyze = Analyze(self.bd)
        analyze.monthly_spend_overview(test_mo_date)
    
    def test_analyze_asset(self):
        sd = date(2021, 2, 5)
        td = date(2021, 12, 30)
        analyze = Analyze(self.bd)
        analyze.history_overview(sd, td)



    
