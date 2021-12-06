fishes = [0]*9

with open("input.txt", "r") as input:
    init = input.readline()[:-1].split(",")
    for i in init:
        fishes[int(i)] += 1

def day(days):
    sim = list(fishes)
    i = 0
    while i < days:
        tmp = sim[0]
        for j in range(0, 8):
            sim[j] = sim[j+1]
        sim[8] = tmp
        sim[6] += tmp
        i += 1
    return sim

##1
d80 = day(80)
count = 0
for i in range(0, 9):
    count += d80[i]
print(count)

##2
d256 = day(256)
count = 0
for i in range(0, 9):
    count += d256[i]
print(count)