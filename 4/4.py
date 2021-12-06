draws = []
boards = []

with open("input.txt", "r") as input:
    for i, line in enumerate(input):
        if i == 0:
            draws = line[:-1].split(",")
        elif (i-1) % 6 == 0:
            boards.append([])
        else:
            boards[(i-1)//6].append(line[:-1].split())

def score(board, drawn):
    score = 0
    for i in range(0, 5):
        for j in range(0, 5):
            score += int(board[i][j]) if not board[i][j] in drawn else 0
    return score * int(drawn[-1])

##1
def winbingo():
    for i in range(6, len(draws)):
        drawn = draws[:i]
        for j in range(0, len(boards)):
            board = boards[j]
            for k in range(0, 5):
                row = board[k]
                column = [board[r][k] for r in range(0, 5)]
                if all(x in drawn for x in row):
                    return score(board, drawn)
                elif all(x in drawn for x in column):
                    return score(board, drawn)
print(winbingo())

##2
def losebingo():
    for i in range(1, len(draws)):
        drawn = draws[:-i]
        for j in range(0, len(boards)):
            board = boards[j]
            won = False
            for k in range(0, 5):
                row = board[k]
                column = [board[r][k] for r in range(0, 5)]
                if all(x in drawn for x in row):
                    won = True
                    break
                elif all(x in drawn for x in column):
                    won = True
                    break
            if not won:
                return score(board, draws[:-(i-1)]) if i > 1 else score(board, draws)
print(losebingo())