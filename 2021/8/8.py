displays = []

with open("input.txt", "r") as input:
    for line in input:
        display = {}
        parts = line.split("|")
        display["digits"] = parts[0].split()
        display["output"] = parts[1].split()
        displays.append(display)

##1
count = 0
for d in displays:
    for o in d["output"]:
        i = len(o)
        if i == 2 or i == 3 or i == 4 or i == 7:
            count += 1
print(count)

##2
count = 0
all = ["a","b","c","d","e","f","g"]
codes = {63:0, 6:1, 91:2, 79:3, 102:4, 109:5, 125:6, 7:7, 127:8, 111:9}
                                        #    0a
for d in displays:                      # 5f    1b
    signals = {}                        #    6g
    one = []                            # 4e    2c
    seven = []                          #    3d
    twothreefive = []
    zerosixnine = []
    for dig in d["digits"]:
        if len(dig) == 2:
            one = dig
        elif len(dig) == 3:
            seven = dig
        elif len(dig) == 5:
            twothreefive.extend(dig)
        elif len(dig) == 6:
            zerosixnine.extend(dig)
    
    #get the signals
    for l in all:
        if l in seven and not l in one:         # a
            signals[l] = 0
    for l in all:
        if twothreefive.count(l) == 3:          # d or g
            if l in signals and signals[l] == 0:
                continue
            elif zerosixnine.count(l) == 2:
                signals[l] = 6
            else:
                signals[l] = 3
        elif twothreefive.count(l) == 2:        # b or c
            if zerosixnine.count(l) == 2:
                signals[l] = 1
            else:
                signals[l] = 2
        elif twothreefive.count(l) == 1:        # e or f
            if zerosixnine.count(l) == 2:
                signals[l] = 4
            else:
                signals[l] = 5
    
    #interpret the output
    value = 0
    for i, digit in enumerate(d["output"]):
        code = 0
        for letter in digit:
            code += 2**signals[letter]
        value += codes[code] * 10**(3-i)
    count += value
print(count)