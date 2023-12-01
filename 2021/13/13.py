points = []
folds = []

with open("input.txt", "r") as input:
    mode = "point"
    for line in input:
        if line == "\n":
            mode = "fold"
            continue
        elif mode == "point":
            coordinates = line[:-1].split(",")
            points.append((int(coordinates[0]), int(coordinates[1])))
        elif mode == "fold":
            eq = line.split()[-1]
            pair = eq.split("=")
            folds.append((pair[0], int(pair[1])))

def fold(points, line):
    points = list(points)
    newpoints = []
    div = line[1]

    if line[0] == "x":
        for p in points:
            newp = (0,0)

            if p[0] < div:
                newp = p
            else:
                newp = (div - (p[0] - div), p[1])

            if not newp in newpoints:
                newpoints.append(newp)

    elif line[0] == "y":
        for p in points:
            newp = (0, 0)

            if p[1] < div:
                newp = p
            else:
                newp = (p[0], div - (p[1] - div))
            
            if not newp in newpoints:
                newpoints.append(newp)

    return newpoints

##1
print(len(fold(points, folds[0])))

##2
def manyfolds(foldlist, startpoints):
    curpoints = startpoints
    for line in foldlist:
        curpoints = fold(curpoints, line)
    return curpoints

dots = manyfolds(folds, points)

dots.sort(key = lambda x: -x[0])
maxX = dots[0][0]
dots.sort(key = lambda x: -x[1])
maxY = dots[0][1]

def makedisplay(x, y):
    display = []
    row = [" "] * (x + 1)
    i = 0
    while i < (y + 1):
        display.append(list(row))
        i += 1
    return display
display = makedisplay(maxX, maxY)

for dot in dots:
    display[dot[1]][dot[0]] = "|"

with open("output.txt", "w") as output:
    for row in display:
        line = ""
        for c in row:
            line += c
        output.write(line)
        output.write("\n")