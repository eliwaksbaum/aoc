import math

class heap:
    array = [0]
    eToIndex = {}

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
            s.eToIndex[p[0]] = i
            s.array[pi] = root
            s.eToIndex[root[0]] = pi
            s.upheap(pi)

    def downheap(s, i):
        root = s.array[i]
        li = s.left(i)
        l = s.array[li] if len(s.array) > li else [0, math.inf]
        ri = s.right(i)
        r = s.array[ri] if len(s.array) > ri else [0, math.inf]

        if l[1] < r[1] and root[1] > l[1]:
            s.array[i] = l
            s.eToIndex[l[0]] = i
            s.array[li] = root
            s.eToIndex[root[0]] = li
            s.downheap(li)
        elif root[1] > r[1]:
            s.array[i] = r
            s.eToIndex[r[0]] = i
            s.array[ri] = root
            s.eToIndex[root[0]] = ri
            s.downheap(ri)

    def put(s, e, priority):
        if e in s.eToIndex:
            s.assign(e, priority)
        else:
            s.array.append([e, priority])
            s.eToIndex[e] = len(s.array) - 1
            s.upheap(len(s.array) - 1)
    
    def remove(s, e):
        i = s.eToIndex.pop(e)
        s.array[i] = s.array.pop()
        s.eToIndex[s.array[i][0]] = i
        s.downheap(i)
        
    def removeMin(s):
        min = s.array[1]
        s.eToIndex.pop(min[0])
        if len(s.array) == 2:
            s.array.pop()
        else:
            s.array[1] = s.array.pop()
            s.eToIndex[s.array[1][0]] = 1
            s.downheap(1)
        return min[0]
    
    def peekMin(s):
        return s.array[1][0]
    
    def assign(s, e, priority):
        i = s.eToIndex[e]
        s.array[i][1] = priority
        s.upheap(i)
        s.downheap(i)
    
    def getCost(s, e):
        return s.array[s.eToIndex[e]][1] if e in s.eToIndex else -1

def fromLists(a, b, c, d, h=None):
    rooms = {"a": a, "b": b, "c": c, "d": d}
    rooms["h"] = [None]*11 if h == None else h
    return rooms

def dictToString(state):
    s = ""
    for r in state:
        s += r
        for p in state[r]:
            s += p if p != None else "N"
    return s

def stringToDict(string):
    depth = (len(string) - 15) // 4
    state = {"a":[None]*depth, "b":[None]*depth, "c":[None]*depth, "d":[None]*depth, "h":[None]*11}
    letters = ["a","b","c","d"]
    for i in range(0, 4):
        for j in range(depth):
            pod = string[i*(depth+1) + j + 1]
            if pod != "N":
                state[letters[i]][j] = pod
    for i in range(-11, 0):
        pod = string[i]
        if pod != "N":
            state["h"][i] = pod
    return state

def cleancopy(state):
    a = list(state["a"])
    b = list(state["b"])
    c = list(state["c"])
    d = list(state["d"])
    h = list(state["h"])
    return fromLists(a, b, c, d, h)

def getConnections(state, weights):
    connections = []

    for i, pod in enumerate(state["h"]):
        if pod != None and isOpen(pod, state):
            start = ("h", i)
            slot = openSlot(pod, state)
            if slot == -1:
                continue
            else:
                end = (pod, slot)
                steps = getPath(start, end)

                allclear = True
                for l, p in steps:
                    if state[l][p] != None:
                        allclear = False
                        break
                if not allclear:
                    continue
                else:
                    weight = weights[(start, end)] * energymap[pod]
                    newrooms = cleancopy(state)
                    newrooms[start[0]][start[1]] = None
                    newrooms[end[0]][end[1]] = pod
                    connections.append((newrooms, weight))

    letters = ["a", "b", "c", "d"]
    for l in letters:
        p = freeAgent(l, state)
        if p == None:
            continue
        pod = state[l][p]
        start = (l, p)
        for i in range(0, 11):
            if i%2 == 1 or i == 0 or i == 10:
                end = ("h", i)
                steps = getPath(start, end)

                allclear = True
                for r, c in steps:
                    if state[r][c] != None:
                        allclear = False
                        break
                if not allclear:
                    continue
                else:
                    weight = weights[(start, end)] * energymap[pod]
                    newrooms = cleancopy(state)
                    newrooms[start[0]][start[1]] = None
                    newrooms[end[0]][end[1]] = pod
                    connections.append((newrooms, weight))
    return connections

def isOpen(r, state):
    return all(map(lambda x: x == r or x == None, state[r]))

def openSlot(r, state):
    for i in range(len(state[r]) - 1, -1, -1):
        if state[r][i] == None:
            return i
    return -1     

def freeAgent(r, state):
    wall = len(state[r])
    for i in range(len(state[r]) - 1, -1, -1):
        if state[r][i] != r:
            break
        else:
            wall = i

    for j in range(wall):
        if state[r][j] != None:
            return j
    
    return None

pathcache = {}
def getPath(start, end):
    if (start, end) in pathcache:
        return pathcache[(start, end)]
    elif (end, start) in pathcache:
        steps = list(pathcache[end, start])
        steps.remove(start)
        steps.append(end)
        pathcache[(start, end)] = steps
        return steps
    else:
        sr = start[0]
        sc = start[1]
        er = end[0]
        ec = end[1]

        steps = []

        if sr != "h":
            for i in range(sc):
                steps.append((sr, i))
            sc = gmap[sr]
        if er != "h":
            for i in range(ec + 1):
                steps.append((er, i))
            ec = gmap[er]
        
        for i in range(min(sc, ec), max(sc, ec) + 1):
            if (i%2 == 1 or i == 0 or i == 10) and (sr != "h" or i != sc):
                steps.append(("h", i))
        
        pathcache[(start, end)] = steps
        return steps

energymap = {"a":1, "b": 10, "c": 100, "d":1000}
gmap = {"a":2, "b":4, "c":6, "d":8}

def makeWeights(depth):
    roomGraph = {}
    weightGraph = {}

    letters = ["a", "b", "c", "d"]
    for l in letters:
        for i in range(0, depth):
            key = (l, i)
            v = []
            
            for j in range(11):
                if j%2 == 1 or j == 0 or j == 10:
                    n = ("h", j)
                    weight = (i + 1) + abs(gmap[l] - j)
                    v.append(n)
                    nedges = roomGraph.setdefault(n, [])
                    nedges.append(key)
                    weightGraph[(key, n)] = weight
                    weightGraph[(n, key)] = weight

            roomGraph[key] = v

    return weightGraph

weights = makeWeights(2)
startState = fromLists(["c", "c"], ["b", "d"], ["a", "a"], ["d", "b"])
endState = fromLists(["a", "a"], ["b", "b"], ["c", "c"], ["d", "d"])
#startState = fromLists(["d"], ["c"], ["b"], ["a"])
#endState = fromLists(["a"], ["b"], ["c"], ["d"])

#room = makeRoom(4)
#startState = fromPositions(["a3", "d3", "c2", "d1"], ["a0", "c0", "b2", "c1"], ["b0", "c3", "b1", "d2"], ["b3", "d0", "a1", "a2"])
#endState = fromPositions(["a0", "a1" "a2", "a3"], ["b0", "b1", "b2", "b3"], ["c0", "c1", "c2", "c3"], ["d1", "d0", "d2", "d3"])

def heuristic(state):
    return 0

def astar(src, dest):
    srcstring = dictToString(src)
    visited = set()
    prev = {srcstring:[]}
    gscore = {srcstring:0}
    fscore = heap()
    fscore.put(srcstring, heuristic(src))

    while dest not in visited:
        cur = fscore.removeMin()

        for nstate, energy in getConnections(stringToDict(cur), weights):
            nstring = dictToString(nstate)
            if not nstring in visited:
                cost = gscore[cur] + energy
                pre = gscore.get(nstring, -1)
                if pre == -1 or cost < pre:
                    gscore[nstring] = cost
                    prev[nstring] = list(prev[cur])
                    prev[nstring].append(nstring)
                    fscore.put(nstring, cost + heuristic(nstate))
        
        if cur == dest:
            print(prev[cur])
            return gscore[cur]
        
        visited.add(cur)

print(getConnections(fromLists(["a", "a"], ["d", "b"], ["c", "c"], ["b","d"]), weights))