program = []

with open("input.txt", "r") as input:
    program = input.readlines()

submodules = []

isDiv = False
x = 0
y = 0
for i, line in enumerate(program):
    if i % 18 == 4:                         # div z 1 or div z 26
        isDiv = line.split()[-1] == "26"
    elif i % 18 == 5:                       # add x a
        x = int(line.split()[-1])
    elif i % 18 == 15:                      # add y a
        y = int(line.split()[-1])
        submodules.append((isDiv, x, y))
    else:
        continue

##1
def getMaxPair(sum):
    if sum >= 0:
        return (9-sum, 9)
    else:
        return (9, 9+sum)

digits = [0]*14
stack = []
for i, (isDiv, x, y) in enumerate(submodules):
    if not isDiv:
        stack.append((i, y))
    else:
        mult = stack.pop()
        digits[mult[0]], digits[i] = getMaxPair(mult[1] + x)

print(digits)

##2
def getMinPair(sum):
    if sum >= 0:
        return (1, 1+sum)
    else:
        return (1-sum, 1) 

digits = [0]*14
stack = []
for i, (isDiv, x, y) in enumerate(submodules):
    if not isDiv:
        stack.append((i, y))
    else:
        mult = stack.pop()
        digits[mult[0]], digits[i] = getMinPair(mult[1] + x)

print(digits)