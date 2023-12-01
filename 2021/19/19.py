scanners = []

with open("input.txt", "r") as input:
    scanner = []
    for line in input:
        if line.find("---") != -1:
            scanner = []
        elif line =="\n":
            scanners.append(scanner) #add an extra newline to the end
        else:
            coos = line.split(",")
            x = int(coos[0])
            y = int(coos[1])
            z = int(coos[2][:-1])
            scanner.append((x,y,z))

def rotate(vector, axis):
    if axis == "x":
        return (vector[0], -vector[2], vector[1])
    elif axis == "y":
        return (-vector[2], vector[1], vector[0])
    elif axis == "z":
        return (vector[1], -vector[0], vector[2])

def minus(a,b):
    return (a[0]-b[0], a[1]-b[1], a[2]-b[2])

def plus(a,b):
    return (a[0]+b[0], a[1]+b[1], a[2]+b[2])

def orientBeacons(beacons, flips):
    rotated = []
    for b in beacons:
        rb = b
        for i in range(flips[0]):
            rb = rotate(rb, "x")
        for j in range(flips[1]):
            rb = rotate(rb, "y")
        for k in range(flips[2]):
            rb = rotate(rb, "z")
        rotated.append(rb)
    return rotated

def translateBeacons(beacons, offset):
    translated = []
    for b in beacons:
        tb = plus(b, offset)
        translated.append(tb)
    return translated

def compareTwo(s1, s1True, s2):
    for i in range(4):
        for j in range(4):
            locations = {}
            rotated = orientBeacons(s2, (i, j, 0))
            for b in rotated:
                for b0 in s1:
                    l = plus(minus(b0, b), s1True)
                    locations[l] = locations.get(l, 0) + 1

            inorder = list(locations.keys())
            inorder.sort(key = lambda x: -locations[x])
            trueL = inorder[0]
            if locations[trueL] >= 12:
                return (trueL, rotated)
        
        for k in range(4):
            locations = {}
            rotated = orientBeacons(s2, (i, 0, k))
            for b in rotated:
                for b0 in s1:
                    l = plus(minus(b0, b), s1True)
                    locations[l] = locations.get(l, 0) + 1

            inorder = list(locations.keys())
            inorder.sort(key = lambda x: -locations[x])
            trueL = inorder[0]
            if locations[trueL] >= 12:
                return (trueL, rotated)
    return None

truescannerLs = [None]*len(scanners)
truescannerLs[0] = (0,0,0)

truescannerBs = [None]*len(scanners)
truescannerBs[0] = scanners[0]

def sweepAgainst(knowns):
    next = []
    for k in knowns:
        for i in range(len(scanners)):
            if i == k:
                continue
            if truescannerLs[i] != None:
                continue
            else:
                trueLB = compareTwo(truescannerBs[k], truescannerLs[k], scanners[i])
                if trueLB != None:
                    truescannerLs[i] = trueLB[0]
                    truescannerBs[i] = trueLB[1]
                    next.append(i)
    return next

next = [0]
while len(next) > 0:
    next = sweepAgainst(next)

#just under 15 seconds. i'll take it

##1
beacons = set()
for i, scanner in enumerate(truescannerBs):
    tscanner = translateBeacons(scanner, truescannerLs[i])
    for beacon in tscanner:
        if beacon not in beacons:
            beacons.add(beacon)
print(len(beacons))

##2
maxManh = 0
for i in range(len(truescannerLs)):
    for j in range(i+1, len(truescannerLs)):
        a = truescannerLs[i]
        b = truescannerLs[j]
        manh = abs(a[0] - b[0]) + abs(a[1] - b[1]) + abs(a[2] - b[2])
        maxManh = manh if manh > maxManh else maxManh
print(maxManh)