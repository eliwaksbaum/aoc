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

def getConnections(pods, room):
    pods = fromStringToDict(pods)
    connections = []
    for pod in pods:
        cleanstate = dict(pods)
        cleanstate.pop(pod)
        species = pods[pod]
        out = pod[0] == "h"
        energy = energymap[species]
        for npos in room[pod]:
            if npos not in pods:
                if not out or out and (npos[0] == "h" or npos[0] == species):
                    newstate = dict(cleanstate)
                    newstate[npos] = species
                    connections.append((fromDictToString(newstate), energy))

    return connections

def heuristic(state):
    depth = 2                               ### CHANGE WITH DEPTH
    
    gmap = {"a":2, "b":4, "c":6, "d":8}
    pods = fromStringToDict(state)
    total = 0
    for pod in pods:
        species = pods[pod]
        room = pod[0]
        if species == room:
            c = int(pod[1])
            for i in range(c + 1, depth):
                if pods.get(species + str(i), species) != species:
                    total += (c + 4) * energymap[species]
                    break
            continue
        elif room == "h":
            c = 10 if pod[1] == "X" else int(pod[1])
            g = gmap[species]
            if c == g:
                for i in range(depth):
                    r = pods.get(species + str(i), species)
                    if r != species:
                        total += 3 * energymap[species]
                        break
            else:
                total += (abs(g-c) + depth) * energymap[species]
            continue
        else:
            c = int(pod[1])
            cg = gmap[pod[0]]
            tg = gmap[species]
            total += (c+1 + abs(cg-tg) + depth) * energymap[species]
            continue
    return total


def makeRoom(depth):
    roomGraph = {}
    for i in range(11):
        key = "h" + str(i) if i < 10 else "hX"
        v = []
        if i-1 >= 0:
            v.append("h"+str(i-1))
        if i+1 < 11:
            v.append("h"+str(i+1)) if i + 1 < 10 else v.append("hX")

        roomGraph[key] = v

    letters = ["a", "b", "c", "d"]
    for g, l in enumerate(letters):
        for i in range(depth):
            key = l + str(i)
            v = []
            if i+1 < depth:
                v.append(l+str(i+1))

            if i == 0:
                v.append("h"+str(2*(g+1)))
                roomGraph["h"+str(2*(g+1))].append(l+str(0))
            else:
                v.append(l+str(i-1))

            roomGraph[key] = v

    return roomGraph

room = makeRoom(2)
#room = makeRoom(1)
startState = fromPositions(["a1", "d1"], ["a0", "c0"], ["b0", "c1"], ["b1", "d0"])
endState = fromPositions(["a0", "a1"], ["b0", "b1"], ["c0", "c1"], ["d1", "d0"])
#startState = fromPositions(["d0"], ["c0"], ["b0"], ["a0"])
#endState = fromPositions(["a0"], ["b0"], ["c0"], ["d0"])

#room = makeRoom(4)
#startState = fromPositions(["a3", "d3", "c2", "d1"], ["a0", "c0", "b2", "c1"], ["b0", "c3", "b1", "d2"], ["b3", "d0", "a1", "a2"])
#endState = fromPositions(["a0", "a1" "a2", "a3"], ["b0", "b1", "b2", "b3"], ["c0", "c1", "c2", "c3"], ["d1", "d0", "d2", "d3"])

def astar(src, dest):
    visited = set()
    prev = {src:[]}
    gscore = {src:0}
    fscore = heap()
    fscore.put(src, heuristic(src))

    while dest not in visited:
        cur = fscore.removeMin()

        for nstate, energy in getConnections(cur, room):
            if not nstate in visited:
                cost = gscore[cur] + energy
                pre = gscore.get(nstate, -1)
                if pre == -1 or cost < pre:
                    gscore[nstate] = cost
                    prev[nstate] = list(prev[cur])
                    prev[nstate].append(nstate)
                    fscore.put(nstate, + heuristic(nstate))
        
        if cur == dest:
            print(prev[cur])
            return gscore[cur]
        
        visited.add(cur)

print(astar(startState, endState))
print(heuristic(startState))