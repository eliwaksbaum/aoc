packet = ""

with open("input.txt", "r") as input:
    hex = input.readline()[:-1]
    pre = bin(int(hex, 16))[2:]
    missing0s = len(hex) * 4 - len(pre) ##turning the hex into a number and back into binary will remove leading 0s
    packet = "0" * missing0s + pre

def getVersion(packet, head) -> int:
    return int(packet[head:head+3], 2)

def parseLiteral(packet, head):
    end = 0
    value = ""
    for i in range(head, len(packet), 5):
        chunk = packet[i+1 : i+5]
        value += chunk
        if packet[i] == "0":
            end = i+5
            break
    return (end, int(value, 2))

def parseLID0(packet, op, head):
    L = packet[head : head + 15]
    subpacketbits = int(L, 2)
    rhead = head + 15

    subvalues = []
    end = rhead + subpacketbits
    while rhead < end:
        hv = parsePacket(packet, rhead)
        rhead = hv[0]
        subvalues.append(hv[1])
    
    return (rhead, operation(op, subvalues))

def parseLID1(packet, op, head):
    L = packet[head : head + 11]
    subpacketnum = int(L, 2)
    rhead = head + 11

    subvalues = []
    for i in range(subpacketnum):
        hv = parsePacket(packet, rhead)
        rhead = hv[0]
        subvalues.append(hv[1])
    
    return (rhead, operation(op, subvalues))

def operation(opID, subvalues):
    if opID == 0: #sum
        sum = 0
        for x in subvalues:
            sum += x
        return sum
    if opID == 1: #product
        prod = 1
        for x in subvalues:
            prod *= x
        return prod
    if opID == 2: #minimum
        return min(subvalues)
    if opID == 3: #maximum
        return max(subvalues)
    if opID == 5: #>
        return 1 if subvalues[0] > subvalues[1] else 0
    if opID == 6: #<
        return 1 if subvalues[0] < subvalues[1] else 0     
    if opID == 7: #==
        return 1 if subvalues[0] == subvalues[1] else 0

versioncount = 0

def parsePacket(packet, head):
    global versioncount
    versioncount += getVersion(packet, head)

    T = int(packet[head + 3 : head + 6], 2)
    if T == 4:
        return parseLiteral(packet, head + 6)
    else:
        I = packet[head + 6]
        if I == "0":
            return parseLID0(packet, T, head + 7)
        else:
            return parseLID1(packet, T, head + 7)

hv = parsePacket(packet, 0) #(head, value)
##1
print(versioncount)
##2
print(hv[1])