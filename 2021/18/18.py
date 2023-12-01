import math

class snailfishnum:
    parent = None
    left = None
    right = None

    def __init__(num, left, right) -> None:
        num.left = left
        num.right = right
    
    def toString(num):
        string = ""
        string += "[" + num.left.toString() if type(num.left) != type(0) else ""

        if type(num.left) == type(0):
            string += "[" + str(num.left)
        string += ","
        if type(num.right) == type(0):
            string += str(num.right) + "]"
        
        string += num.right.toString() + "]" if type(num.right) != type(0) else ""
        return string

    def height(num):
        if num.parent == None:
            return 0
        else:
            return num.parent.height() + 1

    def isLeft(num):
        return num.parent.left == num
    
    def upRight(num):
        if num.parent == None:
            return None
        if num.isLeft():
            return num.parent
        else:
            return num.parent.upRight()

    def shootRight(num, r):
        turn = num.upRight()
        if turn == None:
            return

        focus = turn.right
        if type(focus) == type(0):
            turn.right += r
        else:
            while type(focus.left) != type(0):
                focus = focus.left
            focus.left += r
    
    def upLeft(num):
        if num.parent == None:
            return None
        elif not num.isLeft():
            return num.parent
        else:
            return num.parent.upLeft()

    def shootLeft(num, l):
        turn = num.upLeft()
        if turn == None:
            return

        focus = turn.left
        if type(focus) == type(0):
            turn.left += l
        else:
            while type(focus.right) != type(0):
                focus = focus.right
            focus.right += l

    def explode(num):
        num.shootRight(num.right)
        num.shootLeft(num.left)
        
        if num.isLeft():
            num.parent.left = 0
        else:
            num.parent.right = 0

    def checkExplode(num):
        if num.height() >= 4:
            num.explode()
            return False
        return True

    def rcheckExplode(num):
        good = num.left.rcheckExplode() if type(num.left) != type(0) else True
        if not good:
            return False
        good = num.checkExplode()
        if not good:
            return False
        good = num.right.rcheckExplode() if type(num.right) != type(0) else True
        if not good:
            return False
        return True
    
    def split(num, child):
        if child == "left":
            new = snailfishnum(math.floor(num.left/2), math.ceil(num.left/2))
            num.left = new
            new.parent = num
        else:
            new = snailfishnum(math.floor(num.right/2), math.ceil(num.right/2))
            num.right = new
            new.parent = num
    
    def checkSplit(num):
        if type(num.left) == type(0) and num.left >= 10:
            num.split("left")
            return False
        elif type(num.right) == type(0) and num.right >= 10:
            num.split("right")
            return False
        return True
    
    def rcheckSplit(num):
        good = num.left.rcheckSplit() if type(num.left) != type(0) else True
        if not good:
            return False
        good = num.checkSplit()
        if not good:
            return False
        good = num.right.rcheckSplit() if type(num.right) != type(0) else True
        if not good:
            return False
        return True
    
    def comb(num):
        good = num.rcheckExplode()
        if not good:
            return False
        good = num.rcheckSplit()
        if not good:
            return False
        return True

    def reduce(num):
        done = num.comb()
        if not done:
            num.reduce()
    
    def magnitude(num):
        l = num.left if type(num.left) == type(0) else num.left.magnitude()
        r = num.right if type(num.right) == type(0) else num.right.magnitude()
        return 3*l + 2*r

    def clone(num):
        l = num.left if type(num.left) == type(0) else num.left.clone()
        r = num.right if type(num.right) == type(0) else num.right.clone()

        new = snailfishnum(l, r)
        if type(l) != type(0):
            l.parent = new
        if type(r) != type(0):
            r.parent = new
        return new
    
    @staticmethod
    def add(num1, num2):
        a = num1.clone() if type(num1) != type(0) else num1
        b = num2.clone() if type(num2) != type(0) else num2
        sum = snailfishnum(a, b)
        if type(a) != type(0):
            a.parent = sum
        if type(b) != type(0):
            b.parent = sum
        sum.reduce()
        return sum
    
    @staticmethod
    def parse(text):
        stack = []
        for char in text:
            if char == "[" or char == ",":
                continue
            elif char == "]":
                right = stack.pop()
                left = stack.pop()
                stack.append(snailfishnum.add(left, right))
            else:
                stack.append(int(char))
        return stack.pop()

numbers = []

with open("input.txt", "r") as input:
    for line in input:
        numbers.insert(0, snailfishnum.parse(line[:-1]))

##1
p1 = list(numbers)
for i in range(len(numbers) - 1):
    a = p1.pop()
    b = p1.pop()
    p1.append(snailfishnum.add(a,b))
print(p1.pop().magnitude())

##2
max = 0
for a in numbers:
    for b in numbers:
        if a == b:
            continue
        else:
            mag = snailfishnum.add(a,b).magnitude()
            max = mag if mag > max else max
print(max)