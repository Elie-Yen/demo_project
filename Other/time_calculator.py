'''
Elie Yen
Python 3
'''

def add_time(start, duration, weekday = None):
  '''
  12-hour clock format
  start: string, hh:mm AM 
  duration: string, hh:mm
  weekday: string
  '''
  #_ transform string into int and 24-hour clock format
  cur = start[:-3].split(':')
  cur_h = (int(cur[0]) if 'AM' in start else 
          int(cur[0]) + 12)
  cur_m = int(cur[1])
  d_hm = duration.split(':')

  #_ add the minutes overflow on hour
  cur_m += int(d_hm[1])
  cur_h += int(d_hm[0]) + cur_m // 60
  cur_m %= 60

  day_info = ""
  weekday_info = (", " + weekday.capitalize() if weekday
                else "")
  
  #_ handle hour overflow
  if cur_h >= 24:
    n = cur_h // 24
    cur_h %= 24
    day_info += (" ({0} days later)".format(n) if n > 1
                else " (next day)")
    if weekday_info:
      ref = {0: ", Monday", 1: ", Tuesday", 
          2: ", Wednesday", 3: ", Thursday", 4: ", Friday",
          5: ", Saturday", 6: ", Sunday"}
      for i in ref:
        if ref[i] == weekday_info:
          weekday_info = ref[(i + n) % 7]
          break
  
  #_ 'Expected period to change from AM to PM at 12:00'
  res = "{0}:{1}"
  res += ' AM' if cur_h < 12 else " PM"
  res += weekday_info + day_info

  #_ handle for 12-hour clock format
  if cur_h > 12:
    cur_h -= 12
  elif not cur_h:
    cur_h = 12

  #_ it seems that minutes must have 2 digits
  cur_m = str(cur_m).zfill(2)
  
  return res.format(cur_h, cur_m)
