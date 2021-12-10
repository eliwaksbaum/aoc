lines = []

with open("input.txt", "r") as input:
    lines = input.readlines()

openers = ["(", "[", "{", "<"]
closers = {"(" : ")", "[" : "]", "{" : "}", "<" : ">"}

##1
scoremap = {")" : 3, "]" : 57, "}" : 1197, ">" : 25137}
score = 0

for line in lines:
    stack = []
    for char in line[:-1]:
        if char in openers:
            stack.append(char)
        else:
            lastopen = stack.pop()
            if char != closers[lastopen]:
                score += scoremap[char]
                break
print(score)

##2
scoremap = {")" : 1, "]" : 2, "}" : 3, ">" : 4}
scores = []

for line in lines:
    stack = []
    corrupt = False
    for char in line[:-1]:
        if char in openers:
            stack.append(char)
        else:
            lastopen = stack.pop()
            if char != closers[lastopen]:
                corrupt = True
    if corrupt:
        continue

    score = 0
    while len(stack) > 0:
        dangler = stack.pop()
        score *= 5
        score += scoremap[closers[dangler]]
    scores.append(score)

scores.sort()
print(scores[len(scores)//2])