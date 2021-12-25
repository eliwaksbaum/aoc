instructions = []

with open("input.txt", "r") as input:
    instructions = input.readlines()

class region():
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z
    
    def contains(self, other):
        x = self.x[0] < other.x[0] and self.x[1] > other.x[1]
        y = self.y[0] < other.y[0] and self.y[1] > other.y[1]
        z = self.z[0] < other.z[0] and self.z[1] > other.z[1]
    
    def size(self):
        return (self.x[1] - self.x[0]) * (self.y[1] - self.y[0]) * (self.z[1] - self.z[0])
    
    def dim(self):
        return (self.x, self.y, self.z)

def overlaps1(span1, span2):
    return span1[0] <= span2[1] and span1[1] >= span2[0]

def overlaps3(c1, c2):
    return overlaps1(c1.x, c2.x) and overlaps1(c1.y, c2.y) and overlaps1(c1.z, c2.z)

def discreteDifference(old, new):
    blocks = []
    if old.x[0] < new.x[0]:
        r = region((old.x[0], new.x[0]), old.y, old.z)
        blocks.append(r)
    if old.x[1] > new.x[1]:
        r = region((new.x[1], old.x[1]), old.y, old.z)
        blocks.append(r)
    if old.y[0] < new.y[0]:
        r = region((max(old.x[0], new.x[0]), min(old.x[1], new.x[1])), (old.y[0], new.y[0]), old.z)
        blocks.append(r)
    if old.y[1] > new.y[1]:
        r = region((max(old.x[0], new.x[0]), min(old.x[1], new.x[1])), (new.y[1], old.y[1]), old.z)
        blocks.append(r)
    if old.z[0] < new.z[0]:
        r = region((max(old.x[0], new.x[0]), min(old.x[1], new.x[1])), (max(old.y[0], new.y[0]), min(old.y[1], new.y[1])), (old.z[0], new.z[0]))
        blocks.append(r)
    if old.z[1] > new.z[1]:
        r = region((max(old.x[0], new.x[0]), min(old.x[1], new.x[1])), (max(old.y[0], new.y[0]), min(old.y[1], new.y[1])), (new.z[1], old.z[1]))
        blocks.append(r)
    return blocks

actives = []

def on(new):
    blocks = []
    olds = []
    for r in actives:
        if r.contains(new):
            return
    for r in actives:
        if overlaps3(r, new):
            olds.append(r)
            blocks.extend(discreteDifference(r, new))
    for r in olds:
        actives.remove(r)
    actives.append(new)
    actives.extend(blocks)

def off(cover):
    blocks = []
    olds = []
    for r in actives:
        if overlaps3(r, cover):
            olds.append(r)
            if cover.contains(r):
                continue
            blocks.extend(discreteDifference(r, cover))
    for r in olds:
        actives.remove(r)
    actives.extend(blocks)

def flipSwitch(inst, p1):
    xs = inst.find("=") + 1
    xe = inst.find(",")
    x = inst[xs : xe].split("..")

    if abs(int(x[0])) > 50 and p1:
        return

    ys = inst.find("=", xs) + 1
    ye = inst.find(",", xe + 1)
    y = inst[ys : ye].split("..")

    zs = inst.find("=", ys) + 1
    z = inst[zs:-1].split("..")

    r = region((int(x[0])-.5, int(x[1])+.5), (int(y[0])-.5, int(y[1])+.5), (int(z[0])-.5, int(z[1])+.5))

    if inst.split()[0] == "on":
        on(r)
    else:
        off(r)

##1
for inst in instructions:
    flipSwitch(inst, True)

count = 0
for r in actives:
    count += r.size()
print(count)

##2
actives = []
for inst in instructions:
    flipSwitch(inst, False)

count = 0
for r in actives:
    count += r.size()
print(count)