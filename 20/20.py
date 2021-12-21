lookup = ""
image = []

with open("input.txt", "r") as input:
    lookup = input.readline()[:-1]
    input.readline()
    for line in input:
        image.append(line.split()[0])

def getValue(i, j, ir, jr, image, n):
    pixel = ""
    if i in ir and j in jr:
        pixel = image[i][j]
    else:
        if lookup[0] == ".":
            pixel = "."
        else:
            pixel = lookup[0] if n%2 == 1 else lookup[2**9 - 1]
    return 1 if pixel == "#" else 0

def enhance(image, n):
    enhanced = []
    ir = range(len(image))
    jr = range(len(image[0]))

    for i in range(-1, len(image) + 1):
        row = ""
        for j in range(-1, len(image[0]) + 1):
            ninebit = 0
            power = 8
            for r in range(i-1, i+2):
                for c in range(j-1, j+2):
                    ninebit += 2**power * getValue(r, c, ir, jr, image, n)
                    power -= 1
            row += lookup[ninebit]
        enhanced.append(row)
    return enhanced

def printImage(image):
    for line in image:
        print(line)

def enhanceNTimes(image, num):
    cur = image
    for i in range(num):
        cur = enhance(cur, i)
    return cur

two = enhanceNTimes(image, 2)
count = 0
for line in two:
    for char in line:
        count += 1 if char == "#" else 0
print(count)

fifty = enhanceNTimes(image, 50)
count = 0
for line in fifty:
    for char in line:
        count += 1 if char == "#" else 0
print(count)