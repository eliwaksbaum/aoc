data = []
with open("input.txt", "r") as input:
    data = input.readlines()

##1
count = 0
prev = 10000000000000

for line in data:
    cur = int(line)
    if (cur > prev):
        count += 1
    prev = cur
print(count)

##2
count = 0
prev = 1000000000000

for i in range(0, len(data) - 2):
    cur = int(data[i]) + int(data[i+1]) + int(data[i+2])
    if (cur > prev):
        count += 1
    prev = cur
print (count)