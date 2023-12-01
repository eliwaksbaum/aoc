//string gas = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
string gas = File.ReadAllText("input.txt");

Console.WriteLine((decimal) 32355000 % 50455);

//1
Chamber chamber = new(0, new());
int blow = 0;
for (int i = 0; i < 2022; i++)
{
    if (i % 5 == 0)
    {
        Console.WriteLine(chamber.maxCache);
    }
    Drop(i);
}
Console.WriteLine(chamber.maxCache);

//2
// chamber = new(0, new());
// blow = 0;
// for (int i = 0; i < gas.Length * 10; i++)
// {
//     Drop(i);
// }
// Console.WriteLine(chamber.maxCache);


void Drop(int i)
{
    Rock cur = Next(i);
    while (!cur.Settled)
    {
        char dir = gas[blow%gas.Length];
        if (dir == '>')
        {
            cur.Push(1);
        }
        else
        {
            cur.Push(-1);
        }
        blow++;
        cur.Fall();
    }
}

Rock Next(int turn)
{
    Point drop = new(3, chamber.maxCache + 4);

    return (turn % 5) switch
    {
        0 => new Flat(drop, chamber),
        1 => new Cross(drop, chamber),
        2 => new Ell(drop, chamber),
        3 => new Line(drop, chamber),
        4 => new Square(drop, chamber)
    };
}

class Square : Rock
{
    public Square(Point bl, Chamber chamber) : base(bl, chamber) {}

    protected override List<Point> Generate(Point bl)
    {
        return new()
        {
            bl,
            bl with {y = bl.y + 1},
            bl with {x = bl.x + 1},
            new(bl.x + 1, bl.y + 1)
        };
    }
}

class Line : Rock
{
    public Line(Point bottom, Chamber chamber) : base(bottom, chamber) {}

    protected override List<Point> Generate(Point bottom)
    {
        return new()
        {
            bottom,
            bottom with {y = bottom.y + 1},
            bottom with {y = bottom.y + 2},
            bottom with {y = bottom.y + 3},
        };
    }
}

class Ell : Rock
{
    public Ell(Point bl, Chamber chamber) : base(bl, chamber) {}

    protected override List<Point> Generate(Point bl)
    {
        return new()
        {
            bl,
            bl with {x = bl.x + 1},
            bl with {x = bl.x + 2},
            new(bl.x + 2, bl.y + 1),
            new(bl.x + 2, bl.y + 2)
        };
    }
}

class Cross : Rock
{
    public Cross(Point bl, Chamber chamber) : base(bl, chamber) {}

    protected override List<Point> Generate(Point bl)
    {
        return new()
        {
            bl with {x = bl.x + 1},
            bl with {y = bl.y + 1},
            new(bl.x + 1, bl.y + 1),
            new(bl.x + 1, bl.y + 2),
            new(bl.x + 2, bl.y + 1)
        };
    }
}

class Flat : Rock
{
    public Flat(Point left, Chamber chamber) : base(left, chamber) {}

    protected override List<Point> Generate(Point left)
    {
        return new()
        {
            left,
            left with {x = left.x + 1},
            left with {x = left.x + 2},
            left with {x = left.x + 3}
        };
    }
}

readonly record struct Point(int x, int y);

abstract class Rock
{
    bool settled = false;
    public bool Settled => settled;
    readonly Chamber chamber;
    List<Point> points;

    public Rock(Point bottomLeft, Chamber chamber)
    {
        this.chamber = chamber;
        this.points = Generate(bottomLeft);
    }

    protected abstract List<Point> Generate(Point bl);

    public void Fall()
    {
        foreach (Point p in points)
        {
            int next_y = p.y - 1;
            if (next_y <= 0 || chamber.settledPoints.Contains(p with {y = next_y}))
            {
                Settle();
                return;
            }
        }
        Move(0, -1);
    }

    //dir = 1 or -1
    public void Push(int dir)
    {
        foreach (Point p in points)
        {
            int next_x = p.x + dir;
            if (next_x > 7 || next_x < 1 || chamber.settledPoints.Contains(p with {x = next_x}))
            {
                return;
            }
        }
        Move(dir, 0);
    }

    void Move(int h, int v)
    {
        List<Point> next = new();
        foreach (Point p in points)
        {
            next.Add(new(p.x + h, p.y + v));
        }
        points = next;
    }

    void Settle()
    {
        foreach (Point p in points)
        {
            chamber.settledPoints.Add(p);
            chamber.maxCache = int.Max(chamber.maxCache, p.y);
        }
        settled = true;
    }
}

class Chamber
{
    public readonly HashSet<Point> settledPoints;
    public int maxCache;

    public Chamber(int maxCache, HashSet<Point> settledPoints)
    {
        this.maxCache = maxCache;
        this.settledPoints = settledPoints;
    }
}