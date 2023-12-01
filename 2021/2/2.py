data = []
with open("input.txt", "r") as input:
    data = input.readlines()

##1
x = 0
y = 0

for line in data:
    words = line.split(" ")
    dir = words[0]
    value = int(words[1])

    if (dir == "forward"):
        x += value
    elif (dir == "down"):
        y += value
    elif (dir == "up"):
        y -= value
    else:
        print("houston, we have a problem")
print(x*y)

##2
x = 0
y = 0
aim = 0

for line in data:
    words = line.split(" ")
    dir = words[0]
    value = int(words[1])

    if (dir == "forward"):
        x += value
        y += aim * value
    elif (dir == "down"):
        aim += value
    elif (dir == "up"):
        aim -= value
    else:
        print("houston, we have a problem")
print(x*y)