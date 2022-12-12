string[] input = File.ReadAllLines("input.txt");
Point[] instructions = input.SelectMany(l => Extract(l)).ToArray();

//1
Rope rope = new Rope(0);
foreach (Point vec in instructions)
{
    rope.Move(vec);
}
Console.WriteLine(rope.TailCount);

//2
rope = new Rope(8);
foreach (Point vec in instructions)
{
    rope.Move(vec);
}
Console.WriteLine(rope.TailCount);


List<Point> Extract(string line)
{
    List<Point> repeats = new();
    int value = int.Parse(line[2..^0]);
    Point vec = DirToVec(line[0]);

    for (int i = 0; i < value; i++)
    {
        repeats.Add(vec);
    }
    return repeats;
}

Point DirToVec(char dir) => dir switch
{
    'U' => new Point(0, 1),
    'D' => new Point(0, -1),
    'R' => new Point(1, 0),
    'L' => new Point(-1, 0),
    _ => throw new Exception()
};

readonly record struct Point(int x, int y);

class Rope
{
    Head head;
    Tail tail;
    public int TailCount => tail.Count;

    public Rope(int bodyLength)
    {
        head = new Head();

        Knot prev = head;
        for (int i = 0; i < bodyLength; i++)
        {
            Knot knot = new Body(prev);
            prev = knot;
        }
        tail = new Tail(prev);
    }

    public void Move(Point vec)
    {
        head.Move(vec);
    }
}

class Knot
{
    protected Point location = new Point(0, 0);
    protected int X => location.x;
    protected int Y => location.y;
    
    public event Action<Point> Moved = x => {};
    protected void Move() {Moved(location);}
}

class Head : Knot
{
    public void Move(Point vec)
    {
        location = location with {x = X + vec.x, y = Y + vec.y};
        base.Move();
    }
}

class Body : Knot
{
    public Body(Knot leader)
    {
        leader.Moved += Follow;
    }

    protected virtual void Follow(Point lead_loc)
    {
        if (!Touching(lead_loc))
        {
            Tug(lead_loc);
            base.Move();
        }
    }

    bool Touching(Point head_loc)
    {
        return (Math.Abs(head_loc.x - location.x) <= 1) && (Math.Abs(head_loc.y - location.y) <= 1);
    }

    void Tug(Point head_loc)
    {
        int vert = Math.Sign(head_loc.y - Y);
        int hoz = Math.Sign(head_loc.x - X);
        location = location with {y = Y + vert, x = X + hoz};
    }
}

class Tail : Body
{
    HashSet<Point> visited = new();
    public int Count => visited.Count;

    public Tail(Knot leader) : base(leader)
    {
        visited.Add(location);
    }

    protected override void Follow(Point head_loc)
    {
        base.Follow(head_loc);
        visited.Add(location);
    }
}