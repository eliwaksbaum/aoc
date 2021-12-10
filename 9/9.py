data = []

with open("input.txt", "r") as input:
    first = input.readline()
    l = len(first) + 2
    data.append("9"*l)
    data.append("9" + first[:-1] + "9")
    for line in input:
        data.append("9" + line[:-1] + "9")
    data.append("9"*l)

##1
hstream = [0,0,0]
risk = 0

for i, row in enumerate(data):
    for j, c in enumerate(row):
        hstream[0] = hstream[1]
        hstream[1] = hstream[2]
        hstream[2] = int(c)
        if hstream[1] < hstream[0] and hstream[1] < hstream[2]:
            if hstream[1] < int(data[i-1][j-1]) and hstream[1] < int(data[i+1][j-1]):
                risk += hstream[1] + 1
print(risk)

##2
class basincount:
    def __init__(self, size):
        self.size = size
    
    def grow(self, increment):
        self.size += increment

class layer:
    def __init__(self, start:int, end:int, count:basincount):
        self.start = start
        self.end = end
        self.count = count
    
    def overlap(self, other) -> bool:
        if self.start <= other.start and other.start <= self.end:
            return True
        elif self.start <= other.end and other.end <= self.end:
            return True
        else:
            return False

def getLayers(row:str) -> list:
    penDown = False
    layers = []
    start = 0
    for c in range(0, len(row)):
        if not penDown:
            if row[c] != "9":
                penDown = True
                start = c
        elif penDown:
            if row[c] == "9":
                penDown = False
                layers.append(layer(start, c - 1, basincount(c - start)))
    return layers

def glob(top:layer, bottom:layer ) -> layer:
    if bottom.count != top.count:           # if they're equal, the basin is branched and the first branch
        top.count.grow(bottom.count.size)   # already globbed on to the face
    return layer(bottom.start, bottom.end, top.count)

def globLayers(rowfaces:list, globbingrow:list) -> list:
    newfaces = [] #new joining faces
    newactive = [] #new active basincounts

    for layerG in globbingrow:
        newface = layerG
        for i, layerA in enumerate(rowfaces):
            if layerA.overlap(layerG) or layerG.overlap(layerA):
                newface = glob(layerA, newface)
        newfaces.append(newface)
        if not newface.count in newactive:
            newactive.append(newface.count)

    return [newfaces, newactive]

active = []         #list of active basincounts
biggest = [0,0,0]
faces = []          #list of joining faces in the active row

for row in data:
    globfaces = getLayers(row)
    facesactive = globLayers(faces, globfaces)
    faces = facesactive[0]
    newactive = facesactive[1]

    for count in active:
        if count not in newactive:
            i = -1
            l = count.size
            if l >= biggest[0]:
                i = 0
            elif l >= biggest[1]:
                i = 1
            elif l >= biggest[2]:
                i = 2
            if i != -1:
                biggest.insert(i, l)
                biggest.pop()
    active = newactive
print(biggest[0] * biggest[1] * biggest[2])