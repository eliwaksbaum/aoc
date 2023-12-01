class cucumber:
    def __init__(self, direction, cur):
        self.dir = direction
        self.cur = cur

        if direction == "south":
            self.next = [cur[0] + 1, cur[1]]
        elif direction == "east":
            self.next = [cur[0], cur[1] + 1]
    
    def readyset(self, map, readies):
        if self.next[0] >= len(map): #row
            self.next[0] = 0
        if self.next[1] >= len(map[0]):
            self.next[1] = 0

        if not map[self.next[0]][self.next[1]]:
            readies.append(self)
    
    def go(self, map):
        map[self.cur[0]][self.cur[1]] = False
        map[self.next[0]][self.next[1]] = True

        self.cur = list(self.next)
        if self.dir == "south":
            self.next[0] += 1
        elif self.dir == "east":
            self.next[1] += 1

easts = []
souths = []
seafloor = []

with open("input.txt", "r") as input:
    for r, line in enumerate(input):
        row = []
        for c, char in enumerate(line[:-1]):
            if char == ".":
                row.append(False)
            else:
                row.append(True)
                if char == ">":
                    easts.append(cucumber("east", [r, c]))
                elif char == "v":
                    souths.append(cucumber("south", [r, c]))
        seafloor.append(row)

def step(map):
    done = True

    readies = []
    for deut in easts:
        deut.readyset(map, readies)
    for deut in readies:
        deut.go(map)
    done = done and len(readies) == 0

    readies = []
    for deut in souths:
        deut.readyset(map, readies)
    for deut in readies:
        deut.go(map)
    done = done and len(readies) == 0
       
    return done

done = False
steps = 0
while not done:
    done = step(seafloor)
    steps += 1
print(steps)