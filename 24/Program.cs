string[] input = File.ReadAllLines("input.txt");

BlizzardAlley[] rows = new BlizzardAlley[input.Length - 2];
BlizzardAlley[] columns = new BlizzardAlley[input[0].Length - 2];
for (int i = 0; i < rows.Length; i++)
{
    rows[i] = new BlizzardAlley(input[0].Length - 2);
}
for (int i = 0; i < columns.Length; i++)
{
    columns[i] = new BlizzardAlley(input.Length - 2);
}

for (int y = 0; y < rows.Length; y++)
{
    string row = input[y+1];
    for (int x = 0; x < columns.Length; x++)
    {
        switch (row[x+1])
        {
            case '>':
                rows[y].AddForward(x);
                break;
            case '<':
                rows[y].AddBackward(x);
                break;
            case 'v':
                columns[x].AddForward(y);
                break;
            case '^':
                columns[x].AddBackward(y);
                break;
            default:
                break;
        }
    }
}

Point start = new(0, -1, 0);
Point goal = new(columns.Length - 1, rows.Length, 0);

//1
int there = Dodge(start, columns.Length - 1, rows.Length -1);
Console.WriteLine(there);

//2
int back = Dodge(goal with {t = there}, 0, 0);
int there_again = Dodge(start with {t = back}, columns.Length - 1, rows.Length -1);
Console.WriteLine(there_again);


int Dodge(Point init, int endX, int endY)
{
    HashSet<Point> reached = new();
    Queue<Point> q = new();
    q.Enqueue(init);

    while(true)
    {
        Point cur = q.Dequeue();
        foreach (Point n in GetNeighbors(cur))
        {
            if (n.x == endX && n.y == endY)
            {
                return n.t + 1;
            }
            else if (!reached.Contains(n))
            {
                q.Enqueue(n);
                reached.Add(n);
            }
        }
    }
}

List<Point> GetNeighbors(Point cur)
{
    Point[] possibles;
    List<Point> neighbors;
    //start
    if (cur.y == -1)
    {
        possibles = new Point[] {new(0, 0, cur.t + 1)};
        neighbors = new List<Point> {cur with {t = cur.t + 1}};
    }
    //end
    else if (cur.y == rows.Length)
    {
        possibles = new Point[] {new(columns.Length - 1, rows.Length - 1, cur.t + 1)};
        neighbors = new List<Point> {cur with {t = cur.t + 1}};
    }
    //regular
    else
    {
        possibles = new Point[] {
            cur with {t = cur.t + 1},
            cur with {x = cur.x + 1, t = cur.t + 1},
            cur with {x = cur.x - 1, t = cur.t + 1},
            cur with {y = cur.y + 1, t = cur.t + 1},
            cur with {y = cur.y - 1, t = cur.t + 1},
        };
        neighbors = new();
    }
    
    foreach (Point n in possibles)
    {
        if (!(n.x < 0 || n.y < 0 || n.x >= columns.Length || n.y >= rows.Length))
        {
            if (!columns[n.x].GetBlizzardPositions(n.t).Contains(n.y)
                && !rows[n.y].GetBlizzardPositions(n.t).Contains(n.x))
            {
                neighbors.Add(n);
            }
        }
    }
    return neighbors;
}

readonly record struct Point(int x, int y, int t);

class BlizzardAlley
{
    readonly int period;
    readonly List<int> init_forward = new();
    readonly List<int> init_backward = new();

    public BlizzardAlley(int length)
    {
        period = length;
    }

    public void AddForward(int b)
    {
        init_forward.Add(b);
    }
    public void AddBackward(int b)
    {
        init_backward.Add(b);
    }

    public HashSet<int> GetBlizzardPositions(int minute)
    {
        HashSet<int> blizzards = new();
        foreach(int f in init_forward)
        {
            blizzards.Add((f + minute) % period);
        }
        foreach(int b in init_backward)
        {
            blizzards.Add((b + (minute * (period - 1))) % period);
        }
        return blizzards;
    }
}