grid = {}
lines = []
dangers = 0

with open("input.txt", "r") as input:
    for line in input:
        points = line[:-1].split(" -> ")
        nums = [x.split(",") for x in points]
        lines.append([(int(p[0]), int(p[1])) for p in nums])
    
##1
for line in lines:
    start = line[0]
    end = line[1]
    if start[0] == end[0]:
        s = min(start[1], end[1])
        e = max(start[1], end[1])
        for y in range(s, e + 1):
            point = (start[0], y)
            if grid.get(point):
                if grid[point] == 1:
                    dangers += 1
                grid[point] += 1
            else:
                grid[point] = 1
    elif start[1] == end[1]:
        s = min(start[0], end[0])
        e = max(start[0], end[0])
        for x in range(s, e + 1):
            point = (x, start[1])
            if grid.get(point):
                if grid[point] == 1:
                    dangers += 1
                grid[point] += 1
            else:
                grid[point] = 1
print(dangers)

##2
for line in lines:
    start = line[0]
    end = line[1]
    m = (end[1] - start[1]) / (end[0] - start[0]) if end[0] - start[0] != 0 else 0
    s = min(start, end, key = lambda x: x[0])
    e = max(start, end, key = lambda x: x[0])
    if m == 1:
        for x in range(s[0], e[0] + 1):
            point = (x, s[1] + x - s[0])
            if grid.get(point):
                if grid[point] == 1:
                    dangers += 1
                grid[point] += 1
            else:
                grid[point] = 1
    elif m == -1:
        for x in range(s[0], e[0] + 1):
            point = (x, s[1] - (x - s[0]))
            if grid.get(point):
                if grid[point] == 1:
                    dangers += 1
                grid[point] += 1
            else:
                grid[point] = 1
print(dangers)