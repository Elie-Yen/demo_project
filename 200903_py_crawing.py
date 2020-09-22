# -*- coding: utf-8 -*-
"""
Created on Sat Sep  5 16:13:09 2020

@author: bb
"""

import re
import bs4
import urllib.request as req
        
def StockTest(industry):
    #get industry's url
    stock_header = {
        'user-agent': 'Mozilla/5.0 (iPad; CPU OS 11_0 like Mac OS X) AppleWebKit/604.1.34 (KHTML, like Gecko) Version/11.0 Mobile/15A5341f Safari/604.1'
        }
    industry_log = 'https://www.cnyes.com/twstock/stock_astock.aspx?ga=nav'
    industry_req = req.Request(industry_log, headers=stock_header)
    industry_d = dict()
    with req.urlopen(industry_req) as industry_rp:
        data = industry_rp.read().decode('utf-8')
        root = bs4.BeautifulSoup(data, 'html.parser')
        industry_urls = root.find_all(href = re.compile('index2real.aspx\?'))
        for industry_url in industry_urls:
            industry_d.update({industry_url.string: industry_url['href']})

    #lookup industry info    
    src = 'https://www.cnyes.com/twstock/{0}'.format(industry_d[industry])
    request = req.Request(src, headers=stock_header)
    with req.urlopen(request) as response:
        data = response.read().decode('utf-8')
        root = bs4.BeautifulSoup(data, 'html.parser')
        table = root.find_all('div', 'TableBox')
        print(table)

StockTest('水泥')

