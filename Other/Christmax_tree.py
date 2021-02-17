"""
Elie Yen
Christmas tree builder for html
Python 3
"""
import random
def ChristmasTree(height = 10, text = "&#9670;&#9829;&#8859;&#10047;&#10053; Mery Christmas &#10053;&#10047;&#8859;&#9829;&#9670;", font_size = 10,
                 tree_color = "DarkOliveGreen", star_color = "Orange",
                 color = ["LightCoral", "LightSkyBlue", "MediumPurple", "LightSeaGreen", "LightSalmon"]):
    
    #_ star initialize
    tree = f'''<div class = "ChristmasTreeBody" style = "display: block">
            <p class = "ChristmasTreeBody" style = "line-height: {font_size + 1}px;
            color : {tree_color}; font-family: monospace; text-align : center;">
            <span style = "color : {star_color}; font-size : {font_size * 2}px;">&#9733;\n</span><br>'''
    
    decos = ["&#10053;", "&#10047;", "&#8859;", "&#9829;", "&#9670;"]
    w = 1
    for _ in range(height // 2):
        leaf = ["/"] * w + ["|"] + ["\\"] * w
        num_deco = (random.randint(2, w) if w > 1 else 0)
        for _ in range(num_deco):
            i = random.randrange(1, w * 2 - 1, 2)
            c = random.randint(0, 4)
            deco = random.choice(decos)
            leaf[i] = f'<span style = "color : {color[c]};">{deco}</span>'
        #_ add belt
        c = random.randint(0, 4)
        tree += f'<span style = "color : {color[c]};">{"*." * w + "*<br>"}</span>'
        tree +=  "".join(leaf) + "<br>"
        w += 1
    
    #_ the root
    tree += "&#9632;</p></div>"
    res = f'''<div class = "christmasTree" style = "display: block; padding: 5px;
            font-size: {font_size}px; text-align: center;"><p>{text}<br><br></p>{tree}</div>'''
    print(res)
    return res
    

ChristmasTree(20)  
