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
pmap = {3:1, 4:3, 5:6, 6:7, 7:6, 8:3, 9:1}  # the number of ways to get each value by adding up the three dice
rollRange = range(3, 10)
winRange = range(3, 11)                     # from any starting point, it takes a minimum of 3 and a maximum of 10 turns to cross 21

def get21Counts(s0, points, turn):
    p21s = dict.fromkeys(winRange, 0)
    for r in rollRange:
        next = (s0 + r) % 10
        p = points + next + 1
        t = turn + 1

        if p >= 21:
            p21s[t] += pmap[r]
        else:
            branches = get21Counts(next, p, t)
            for length in branches:
                p21s[length] += branches[length] * pmap[r]
    return p21s

p1_21s = get21Counts(p1start, 0, 0)   # dictionaries where the keys are the player's turns, and the values are the number of universes
p2_21s = get21Counts(p2start, 0, 0)   # in which the player crosses 21 on that turn

p1_alives = dict.fromkeys(range(2,11), 0)   # dictionaries where the keys are the player's turns and the values are the number of universe
p2_alives = dict.fromkeys(range(2,11), 0)   # in which the player hasn't crossed 21 yet
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


# After I finished my first solution I checked out the subreddit and saw people talking about memoization. I looked it up and thought I'd
# give it a shot. My first stab actually looked a lot like this, but without the cache it didn't run in a reasonable amount of time. I'm 
# glad that I wasn't initially familiar with the strategy though, since it forced me to come up with the solution above, which I really like.

pmap = {3:1, 4:3, 5:6, 6:7, 7:6, 8:3, 9:1}
cache = {}

def getWins(positions, points, turn):
    wins = [0,0]
    for r in rollRange:
        branchwins = [0,0]
        if positions in cache and points in cache[positions] and turn%2 in cache[positions][points] and r in cache[positions][points][turn%2]:
            memowins = cache[positions][points][turn%2][r]
            branchwins[0] = memowins[0]
            branchwins[1] = memowins[1]
        else:
            if turn%2 == 0:
                nextpos = ((positions[0] + r) % 10, positions[1])
                nextpts = (points[0] + nextpos[0] + 1, points[1])
            else:
                nextpos = (positions[0], (positions[1] + r) % 10)
                nextpts = (points[0], points[1] + nextpos[1] + 1)
            
            if nextpts[turn%2] >= 21:
                branchwins[turn%2] = 1
            else:
                branchwins = getWins(nextpos, nextpts, turn + 1)
            cache.setdefault(positions, {}).setdefault(points, {}).setdefault(turn%2, {})[r] = branchwins
        
        wins[0] += branchwins[0] * pmap[r]
        wins[1] += branchwins[1] * pmap[r]
    return wins

wins = getWins((p1start, p2start), (0,0), 0)
print(max(wins))