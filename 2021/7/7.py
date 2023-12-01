crabs = []

with open("input.txt", "r") as input:
    data = input.readline().split(",")
    for x in data:
        crabs.append(int(x))

##1
crabs.sort()
mid = crabs[len(crabs)//2]
fuel = 0
for crab in crabs:
    fuel += abs(crab - mid)
print(mid, fuel)

##2
sum = 0
for crab in crabs:
    sum += crab
mean = round(sum/(len(crabs)+1))
fuel = 0
for crab in crabs:
    fuel += (abs(crab - mean)*(abs(crab - mean) + 1))/2
print(mean, fuel)