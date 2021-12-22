p1start = 6 - 1
p2start = 2 - 1

##1
def s(t, s0, r1):
    return (s0 + 9*t**2 + (r1 - 9)*t) % 10

def p(t, s0, r1):
    total = 0
    for i in range(1, t+1):
        total += s(i, s0, r1) + 1
    return total

def rollTil1000(s0, r1):
    total = 0
    i = 0
    while total < 1000:
        i += 1
        total += s(i, s0, r1) + 1
    return i

p1turns = rollTil1000(p1start, 6)   # first roll is 1,2,3 = 6
p2turns = rollTil1000(p2start, 15)  # first roll is 4,5,6 = 15

if p1turns < p2turns:
    losingScore = p(p1turns - 1, p2start, 15)
    rolls = 3 * (p1turns*2 -1)
else:
    losingScore = p(p2turns, p1start, 6)
    rolls = 3 * (p2turns*2)

print(losingScore * rolls)

##2
def getPoints(s0, rolls):
    t = len(rolls)
    p = 0
    s = s0
    for r in rolls:
        s = (s + r) % 10
        p += s + 1
    return p

pmap = {3:1, 4:3, 5:6, 6:7, 7:6, 8:3, 9:1}  # the number of ways to get each value by adding up the three dice
rollRange = range(3, 10)
winRange = range(3, 11)                     # from any starting point, it takes a minimum of 3 and a maximum of 10 turns to cross 21

def get21Counts(s0, rolls):
    p21s = dict.fromkeys(winRange, 0)
    for r in rollRange:
        next = list(rolls)
        next.append(r)
        p = getPoints(s0, next)

        if p >= 21:
            universes = 1
            for r in next:
                universes *= pmap[r]        # a sequence of rolls does not represent 1 universe, since each roll represents multiple permutations
            p21s[len(next)] += universes    # of the 3 dice values
        else:
            branches = get21Counts(s0, next)
            for length in branches:
                p21s[length] += branches[length]
    return p21s

p1_21s = get21Counts(p1start, [])   # dictionaries where the keys are the player's turns, and the values are the number of universes
p2_21s = get21Counts(p2start, [])   # in which the player crosses 21 on that turn

p1_alives = dict.fromkeys(range(2,11), 0)
p2_alives = dict.fromkeys(range(2,11), 0)
p1_alives[2] = 27**2
p2_alives[2] = 27**2
for i in p1_21s:
    p1_alives[i] = p1_alives[i-1]*27 - p1_21s[i]    # each turn the player takes, the number of universes multiplies by 27, but then the branches
    p2_alives[i] = p2_alives[i-1]*27 - p2_21s[i]    # where the player crosses 21 die out and only the remaining branches continue to multiply

p1 = 0                              # the number of universes where p1 wins on their nth turn is the number of universes where p1
p2 = 0                              # crosses 21 on their nth turn * the number of universes where p2 doesn't cross 21 on their (n-1)th turn
for i in p1_21s:                    # same for p2, but since they go second we care if p1 crossed on their nth turn
    p1 += p1_21s[i] * p2_alives[i-1]
    p2 += p2_21s[i] * p1_alives[i]
print(max(p1, p2))