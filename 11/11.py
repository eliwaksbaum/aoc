import copy

octopuses = []

class octopus:
    connections = []
    flashed = False

    def __init__(self, energy):
        self.energy = energy
    
    def flash(self):
        for octo in self.connections:
            octo.energy += 1

    def step(self):
        if self.energy >= 9 and not self.flashed:
            self.flash()
            self.flashed = True
            return True
        return False
    
    def clear(self):
        if self.energy >= 9:
            self.energy = 0
            self.flashed = False
        else:
            self.energy += 1

def buildgraph(octoarray):
    graph = []
    rl = range(0, len(octoarray))
    cl = range(0, len(octoarray[0]))
    for i in rl:
        for j in cl:
            connections = []
            up = i-1 in rl
            down = i+1 in rl
            left = j-1 in cl
            right = j+1 in cl
            if up:
                connections.append(octoarray[i-1][j])
            if down:
                connections.append(octoarray[i+1][j])
            if right:
                connections.append(octoarray[i][j+1])
            if left:
                connections.append(octoarray[i][j-1])
            if up and right:
                connections.append(octoarray[i-1][j+1])
            if up and left:
                connections.append(octoarray[i-1][j-1])
            if down and right:
                connections.append(octoarray[i+1][j+1])
            if down and left:
                connections.append(octoarray[i+1][j-1])
            
            octo = octoarray[i][j]
            octo.connections = connections
            graph.append(octo)
    return graph


with open("input.txt", "r") as input:
    array = []
    for row in input:
        i = []
        array.append(i)
        for c in row[:-1]:
            i.append(octopus(int(c)))
    octopuses = buildgraph(array)

def step(octos):
    flashes = 0
    stepdone = False

    while not stepdone:
        oneflash = False
        for octo in octos:
            if octo.step():
                flashes += 1
                oneflash = True
        stepdone = not oneflash

    for octo in octos:
        octo.clear()

    return flashes

##1
def stepNTimes(num):
    octos = copy.deepcopy(octopuses)
    flashes = 0
    steps = 0

    while steps < num:
        flashes += step(octos)
        steps += 1
    return flashes

print(stepNTimes(100))

##2
def stepTilSync():
    octos = copy.deepcopy(octopuses)
    steps = 0
    flashes = 0

    while flashes < len(octos):
        flashes = step(octos)
        steps += 1
    return steps

print(stepTilSync())