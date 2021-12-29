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

energymap = {"a":1, "b": 10, "c": 100, "d":1000}
gmap = {"a":2, "b":4, "c":6, "d":8}

def fromPositions(a, b, c, d):
    amphipods = {}
    for pos in a:
        amphipods[pos] = ("a")
    for pos in b:
        amphipods[pos] = ("b")
    for pos in c:
        amphipods[pos] = ("c")
    for pos in d:
        amphipods[pos] = ("d")
    return(fromDictToString(amphipods))

def fromStringToDict(s):
    dic = {}
    for i in range(0, len(s), 3):
        dic[s[i:i+2]] = s[i+2]
    return dic

def fromDictToString(dic):
    keys = list(dic.keys())
    keys.sort()
    s = ""
    for k in keys:
        s += k + dic[k][0]
    return s

cache = {}
def getPath(start, end, room):
    if (start, end) in cache:
        return cache[(start, end)]
    elif (end, start) in cache:
        steps = list(cache[end, start])
        steps.remove(start)
        steps.append(end)
        cache[(start, end)] = steps
        return steps
    else:
        sr = start[0]
        sc = 10 if start[1] == "X" else int(start[1])
        er = end[0]
        ec = 10 if end[1] == "X" else int(end[1])

        steps = []

        if sr != "h":
            for i in range(1, sc):
                steps.append(sr + str(i))
            sc = gmap[sr]
        if er != "h":
            for i in range(1, ec + 1):
                steps.append(er + str(i))
            ec = gmap[er]
        
        for i in range(min(sc, ec), max(sc, ec) + 1):
            step = "hX" if i == 10 else "h" + str(i)
            if step in room and step != start:
                steps.append(step)
        
        cache[(start, end)] = steps
        return steps

def getConnections(pos, posToSpecies, room):
    connections = []
    species = posToSpecies[pos]
    curroom = pos[0]
    energy = energymap[species]
    cleandict = dict(posToSpecies)
    cleandict.pop(pos)

    for node, weight in room[pos]:
        if curroom == "h" and node[0] != species:
            continue

        allclear = True
        for step in getPath(pos, node, room):
            if step in cleandict:
                allclear = False
                break

        if not allclear:
            continue

        newstate = dict(cleandict)
        newstate[node] = species
        connections.append((fromDictToString(newstate), weight * energy))
    
    return connections

def heuristic(state, room):
    total = 0
    amphipods = fromStringToDict(state)
    for pos in amphipods:
        species = amphipods[pos]
        energy = energymap[species]
        dest = species + "1"

        if pos[0] == species:
            continue
        else:
            for node, weight in room[pos]:
                if node == dest:
                    total += energy * weight
            
            # for step in getPath(pos, dest, room):
            #     if step in amphipods:
            #         total += 3 * energymap[amphipods[step]]

    return total

def makeRoom(depth):
    roomGraph = {}

    letters = ["a", "b", "c", "d"]
    for l in letters:
        for i in range(1, depth+1):
            key = l + str(i)
            v = []
            
            for j in range(11):
                if j%2 == 1 or j == 0 or j == 10:
                    n = "h" + str(j) if j < 10 else "hX"
                    weight = i + abs(gmap[l] - j)
                    v.append((n, weight))
                    nedges = roomGraph.setdefault(n, [])
                    nedges.append((key, weight))

            roomGraph[key] = v

    return roomGraph

room = makeRoom(2)
#room = makeRoom(1)
print(room)
startState = fromPositions(["c1", "c2"], ["b1", "d2"], ["a1", "a2"], ["b2", "d1"])
endState = fromPositions(["a1", "a2"], ["b1", "b2"], ["c1", "c2"], ["d1", "d2"])
#startState = fromPositions(["d1"], ["c1"], ["b1"], ["a1"])
#endState = fromPositions(["a1"], ["b1"], ["c1"], ["d1"])

#room = makeRoom(4)
#startState = fromPositions(["a3", "d3", "c2", "d1"], ["a0", "c0", "b2", "c1"], ["b0", "c3", "b1", "d2"], ["b3", "d0", "a1", "a2"])
#endState = fromPositions(["a0", "a1" "a2", "a3"], ["b0", "b1", "b2", "b3"], ["c0", "c1", "c2", "c3"], ["d1", "d0", "d2", "d3"])

def astar(src, dest):
    visited = set()
    prev = {src:[]}
    gscore = {src:0}
    fscore = heap()
    fscore.put(src, heuristic(src, room))

    while dest not in visited:
        cur = fscore.removeMin()
        pass

        amphipods = fromStringToDict(cur)
        for pod in amphipods:
            for nstate, energy in getConnections(pod, amphipods, room):
                if not nstate in visited:
                    cost = gscore[cur] + energy
                    pre = gscore.get(nstate, -1)
                    if pre == -1 or cost < pre:
                        gscore[nstate] = cost
                        prev[nstate] = list(prev[cur])
                        prev[nstate].append(nstate)
                        fscore.put(nstate, cost)# + heuristic(nstate, room))
        
        if cur == dest:
            print(prev[cur])
            return gscore[cur]
        
        visited.add(cur)

print(heuristic(startState, room))
print(getPath("a2", "d2", room))
print(getConnections("a1", fromStringToDict(startState), room))
print(astar(startState, endState))
