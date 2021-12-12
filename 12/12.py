caves = {}

with open("input.txt", "r") as input:
    for line in input:
        connection = line.split()[0].split("-")
        c1 = connection[0]
        c2 = connection[1]

        if not c1 in caves:
            caves[c1] = []
        caves[c1].append(c2)
        if not c2 in caves:
            caves[c2] = []
        caves[c2].append(c1)

def isSmall(cave):
    return cave != cave.upper()

##1
def paths(root, smallsvisited):
    if isSmall(root):
        smallsvisited.append(root)
    count = 0
    neighbors = caves[root]
    for cave in neighbors:
        if cave == "end":
            count += 1
        elif cave in smallsvisited:
            continue
        else:
            count += paths(cave, list(smallsvisited))
    return count

print(paths("start", []))

##2
def paths(root, smallsvisited, special, specialcount):
    if isSmall(root):
        smallsvisited.append(root)
    count = 0
    neighbors = caves[root]
    for cave in neighbors:
        if cave == "end":
            count += 1
        elif cave == special:
            if specialcount < 2:
                count += paths(cave, list(smallsvisited), special, specialcount + 1)
            else:
                continue
        elif cave in smallsvisited:
            continue
        else:
            count += paths(cave, list(smallsvisited), special, specialcount)
    return count

basecount = paths("start", [], "", 0)
total = basecount

for cave in caves:
    if isSmall(cave) and cave != "start" and cave != "end":
        total += paths("start", [], cave, 0) - basecount
print(total)