data = []
with open("input.txt", "r") as input:
    data = input.readlines()

zeros = [0]*12
ones = [0]*12

for line in data:
    for i in range(0, 12):
        if line[i] == "0":
            zeros[-(i-11)] += 1
        else:
            ones[-(i-11)] += 1

##1
gamma = 0
epsilon = 0

for i in range(0, 12):
    if zeros[i] > ones[i]:
        epsilon += 2**i
    else:
        gamma += 2**i
print(gamma * epsilon)

##2
o2 = 0
co2 = 0

data.sort()

def lifesupport(mode):
    start = 0
    end = len(data)
    digit = 0
    while True:
        pivoted = False
        pivot = 0
        zeros = 0
        ones = 0

        for i in range(start, end):
            if data[i][digit] == "0":
                zeros += 1
            else:
                if not pivoted:
                    pivot = i
                    pivoted = True
                ones += 1
        
        if zeros + ones == 1:
            return data[start]

        if zeros > ones:
            if mode == "o":
                end = pivot
            elif mode == "c":
                start = pivot
        else:
            if mode == "o":
                start = pivot
            elif mode == "c":
                end = pivot
        digit += 1

o2 = int(lifesupport("o"),  2)
co2 = int(lifesupport("c"), 2)
print(o2*co2)


## this is an interesting solution to a slightly different problem, where you consider the most common bit for each position
## in all the numbers, without discarding as you go like the problem asks you to. gotta read these closer
row12 = [False] * 2**12

for line in data:
    row12[int(line, 2)] = True

def find(guess, round=0):
    if round == 0:
        if row12[guess]:
            return guess
    else:
        for i in range (guess, guess + 2**(round - 1)):
            if row12[i]:
                return i
    
    treeIndex = 2**12 + guess
    gparent = treeIndex // 2**(round + 1)     #go up another level
    branch = ((treeIndex//2**round) + 1) % 2     #we want to search the branch we didn't already search, adding one gets the other branch
    newguess = (gparent*2 + branch) * 2**round    #go down one level, move horizontally, go down the rest
    return find(newguess - 2**12, round + 1)

o2 = find(gamma)
co2 = find(epsilon)