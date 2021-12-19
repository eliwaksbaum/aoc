import math

class node:
    def __init__(self, weight, connections) -> None:
        self.weight = weight
        self.connections = connections

# I know heapq exists but I just did an assignment implementing a minheap in Java, so why not
class heap:
    array = [0]
    pointToIndex = {}

    def __init__(s) -> None:
        pass

    def parent(s, i):
        return i//2
    def left(s, i):
        return 2*i
    def right(s, i):
        return 2*i + 1

    def upheap(s, i):
        root = s.array[i]
        pi = s.parent(i)
        if pi < 1:
            return
        p = s.array[pi]
        if root[1] < p[1]:
            s.array[i] = p
            s.pointToIndex[p[0]] = i
            s.array[pi] = root
            s.pointToIndex[root[0]] = pi
            s.upheap(pi)

    def downheap(s, i):
        root = s.array[i]
        li = s.left(i)
        l = s.array[li] if len(s.array) > li else [0, math.inf]
        ri = s.right(i)
        r = s.array[ri] if len(s.array) > ri else [0, math.inf]

        if l[1] < r[1] and root[1] > l[1]:
            s.array[i] = l
            s.pointToIndex[l[0]] = i
            s.array[li] = root
            s.pointToIndex[root[0]] = li
            s.downheap(li)
        elif root[1] > r[1]:
            s.array[i] = r
            s.pointToIndex[r[0]] = i
            s.array[ri] = root
            s.pointToIndex[root[0]] = ri
            s.downheap(ri)

    def put(s, point, priority):
        if point in s.pointToIndex:
            s.assign(point, priority)
        else:
            s.array.append([point, priority])
            s.pointToIndex[point] = len(s.array) - 1
            s.upheap(len(s.array) - 1)
    
    def remove(s, point):
        i = s.pointToIndex.pop(point)
        s.array[i] = s.array.pop()
        s.pointToIndex[s.array[i][0]] = i
        s.downheap(i)
        
    def removeMinPoint(s):
        min = s.array[1]
        s.pointToIndex.pop(min[0])
        if len(s.array) == 2:
            s.array.pop()
        else:
            s.array[1] = s.array.pop()
            s.pointToIndex[s.array[1][0]] = 1
            s.downheap(1)
        return min[0]
    
    def peekMin(s):
        return s.array[1][0]
    
    def assign(s, point, priority):
        i = s.pointToIndex[point]
        s.array[i][1] = priority
        s.upheap(i)
        s.downheap(i)
    
    def getCost(s, point):
        return s.array[s.pointToIndex[point]][1] if point in s.pointToIndex else -1

def djikstra(map, src, dest):
    visited = set()
    assigned = heap()
    assigned.put(src, 0)

    cur = src
    while dest not in visited:
        for neighbor in map[cur].connections:
            if not neighbor in visited:
                cost = assigned.getCost(cur) + map[neighbor].weight
                pre = assigned.getCost(neighbor)
                if pre == -1 or cost < pre:
                    if neighbor == (0,3):
                        pass
                    assigned.put(neighbor, cost)
        
        if cur == dest:
            return assigned.getCost(dest)

        assigned.removeMinPoint()
        visited.add(cur)
        cur = assigned.peekMin()

##1
lines = []

with open("input.txt", "r") as input:
    for line in input:
        lines.append(line[:-1])

def buildgraph(lines):
    graph = {}
    cl = range(len(lines))
    rl = range(len(lines[0]))
    for j, line in enumerate(lines):
        for i, num in enumerate(line):
            key = (i,j)
            weight = int(num)
            connections = []

            up = j-1 in rl
            down = j+1 in rl
            left = i-1 in cl
            right = i+1 in cl
            if up:
                connections.append((i, j-1))
            if down:
                connections.append((i, j+1))
            if left:
                connections.append((i-1, j))
            if right:
                connections.append((i+1, j))
        
            graph[key] = node(weight, connections)
    return graph

graph = buildgraph(lines)
print(djikstra(graph, (0,0), (len(lines[0]) - 1, len(lines) - 1)))

##2
def maketiles(base):
    tiles = [base]
    for i in range (1, 9):
        tile = []
        for row in base:
            newrow = ""
            for c in row:
                newnum = (int(c) + i)
                newnum = (newnum + 1) % 10 if newnum >= 10 else newnum
                newrow += str(newnum)
            tile.append(newrow)
        tiles.append(tile)
    return tiles

def merge(tiles, start):
    band = []
    for i in range(len(tiles[0])):
        row = ""
        for j in range(start, start + 5):
            row += tiles[j][i]
        band.append(row)
    return band

def fiveXfive(tiles):
    entire = []
    for i in range(5):
        entire.extend(merge(tiles, i))
    return entire

tiles = maketiles(lines)
whole = fiveXfive(tiles)
graph = buildgraph(whole)
print(djikstra(graph, (0,0), (len(whole[0]) - 1, len(whole) - 1))) #give it a good 6 seconds