def arithmetic_arranger(problems, answer = False):
  if not problems:
    return
  if len(problems) > 5:
    return "Error: Too many problems."
   
  #_ split into lines while decide the width
  lines = {0: "" ,1: "", 2: "", 3: ""}
  for p in problems:
    ex = []
    num = ""
    p = p.replace(" ", "")
    #_ check each char and catch errors
    #_ EOF'#' for add the last operand
    for char in p + '#':
      if char.isdigit():
        num += char
      else:
        if len(num) > 4:
          return "Error: Numbers cannot be more than four digits."
        ex.extend([num, char])
        num = ""
    ex.pop() #_ remove the EOF"#"
    if len(ex) > 3 or ex[1] not in ('+', '-', '*', '/'):
      return "Error: Numbers must only contain digits."

    if ex[1] in ('*', '/'):
      return "Error: Operator must be '+' or '-'."
        
    
    result = (int(ex[0]) + int(ex[2]) if ex[1] == '+'
          else int(ex[0]) - int(ex[2])) 
    result = str(result)
    #_ 1 space between longest operand and operator
    width = max(len(ex[0]), len(ex[2])) + 2
    lines[0] += (ex[0].rjust(width)) + '    '
    lines[1] += ex[1] + ' ' + (ex[2].rjust(width - 2)) + '    '
    lines[2] += ("-" * width) + '    '
    lines[3] += result.rjust(width) + '    '


  #_ show answer or not
  show = 4 if answer else 3
  return '\n'.join(list(lines[i][ : -4] for i in range(show)))
