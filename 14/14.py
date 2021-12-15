template = ""
outwards = {}
nullcounts = {}

with open("input.txt", "r") as input:
    template = input.readline()[:-1]
    input.readline()
    for line in input:
        inout = line.split()
        pair = inout[0]
        insert = inout[2]

        outward = (pair[0] + insert, insert + pair[1])
        outwards[pair] = outward
        nullcounts[pair] = 0

def evolve(outmap, curcounts):
    newcounts = dict(nullcounts)
    for pair in curcounts:
        for out in outmap[pair]:
            newcounts[out] += curcounts[pair]
    return newcounts

def evolveNTimes(outmap, startcounts, num):
    curcounts = startcounts
    i = 0
    while i < num:
        curcounts = evolve(outmap, curcounts)
        i += 1
    return curcounts

def getLetterCounts(paircounts):
    lettercounts = {}
    for pair in paircounts:
        for l in pair:
            if l in lettercounts:
                lettercounts[l] += paircounts[pair]/2
            else:
                lettercounts[l] = paircounts[pair]/2

    lettercounts[template[0]] += .5
    lettercounts[template[-1]] += .5
    return lettercounts

def extremeDif(lc):
    lclist = []
    for x in lc.items():
        lclist.append(x)

    lclist.sort(key = lambda x: x[1])
    min = lclist[0][1]
    lclist.sort(key = lambda x: -x[1])
    max = lclist[0][1]

    return max - min

basecounts = dict(nullcounts)
for i in range(0, len(template) - 1):
    pair = template[i:i+2]
    basecounts[pair] += 1

##1
ten = evolveNTimes(outwards, basecounts, 10)
lcs = getLetterCounts(ten)
dif = extremeDif(lcs)
print(dif)

##2
forty = evolveNTimes(outwards, basecounts, 40)
lcs = getLetterCounts(forty)
dif = extremeDif(lcs)
print(dif)



## ran fine for part one, but turns out you can't just iterate your way through a 4-trillion character long string

# pairmap = {}
# template = ""
# letters = []

# with open("input.txt", "r") as input:
#     template = input.readline()[:-1]
#     input.readline()
#     for line in input:
#         inout = line.split()
#         pair = inout[0]
#         insert = inout[2]

#         pairmap[pair] = insert
#         if not insert in letters:
#             letters.append(insert)

# def evolve(base, map):
#     result = base[0]
#     for i in range(0, len(base) - 1):
#         pair = base[i:i+2]
#         if pair in map:
#             result += map[pair] + base[i+1]
#     return result

# def evolveNTimes(base, map, num):
#     curbase = base
#     i = 0
#     while i < num:
#         curbase = evolve(curbase, map)
#         i += 1
#     return curbase

# def extremeDif(string, letters):
#     most = 0
#     least = len(string)
#     for char in letters:
#         count = string.count(char)
#         if count > most:
#             most = count
#         elif count < least:
#             least = count
#     return most - least

# ten = evolveNTimes(template, pairmap, 10)
# print(extremeDif(ten, letters))