HashSet<Point> beacons = new();

List<Sensor> sensors = File.ReadAllLines("input.txt")
    .Select(line => line.Split(' ')
        .Where(word => word[1] == '=')
        .Select(num => int.Parse(num[2..^1]))
        .ToList()
    )
    .Select(l => {
        beacons.Add(new(l[2], l[3]));
        int r = Math.Abs(l[0] - l[2]) + Math.Abs(l[1] - l[3]);
        return new Sensor(l[0], l[1], r);
    })
    .ToList();

//1
int start = sensors
    .Where(s => Remaining(s, 2000000) >= 0)
    .Select(s => s.x - Remaining(s, 2000000))
    .Min();

int end = sensors
    .Where(s => Remaining(s, 2000000) >= 0)
    .Select(s => s.x + Remaining(s, 2000000))
    .Max();

int count = 0;
for (int x = start; x < end + 1; x++)
{
    foreach (Sensor s in sensors)
    {
        int distance = Math.Abs(s.x - x) + Math.Abs(s.y - 2000000);
        if (distance <= s.radius && !beacons.Contains(new(x, 2000000)))
        {
            count++;
            break;
        }
    }
}
Console.WriteLine(count);

int Remaining(Sensor s, int level)
{
    int down = Math.Abs(s.y - level);
    return s.radius - down;
}

//2
HashSet<Point> intersections = new();

for (int i = 0; i < sensors.Count - 1; i++)
{
    Sensor cur = sensors[i];
    for (int j = i+1; j < sensors.Count; j++)
    {
        Sensor other = sensors[j];
        intersections.UnionWith(cur.GetIntersections(other));
    }
}
intersections.RemoveWhere(p => (p.x - p.y)/2 < 0 | (p.x - p.y)/2 > 4000000 | (p.x + p.y)/2 < 0 | (p.x + p.y)/2 > 4000000);
foreach (Sensor s in sensors)
{
    intersections.RemoveWhere(p => s.IsPointStrictlyWithin(p));
}

Point beacon = Search();
decimal frequency = (decimal) beacon.x * 4000000 + beacon.y;
Console.WriteLine(frequency);

Point Search()
{
    foreach (Point p in intersections)
    {
        int twos = 0;
        foreach (Point q in intersections)
        {
            if (Math.Abs(p.x - q.x) + Math.Abs(p.y - q.y) == 2)
            {
                twos++;
            }
        }
        if (twos == 2)
        {
            List<Point> diagonals = new()
            {
                new(p.x + 1, p.y + 1),
                new(p.x + 1, p.y - 1),
                new(p.x - 1, p.y + 1),
                new(p.x - 1, p.y - 1)
            };
            foreach(Point d in diagonals)
            {
                if (IsBlindSpot(d))
                {
                    return new Point((d.x - d.y)/2, (d.x + d.y)/2);
                }
            }
        }
    }
    return new(0,0);
}

bool IsBlindSpot(Point p)
{
    foreach (Sensor s in sensors)
    {
        if (s.IsPointWithin(p)) { return false; }
    }
    return true;
}



readonly record struct Point(int x, int y);

struct Sensor
{
    public readonly int x;
    public readonly int y;
    public readonly int radius;

    public int u1 => y+x - radius;
    public int u2 => y+x + radius;
    public int v1 => y-x - radius;
    public int v2 => y-x + radius;
    public int sidelength => Math.Abs(u1 - u2);

    public Sensor(int x, int y, int radius)
    {
        this.x = x;
        this.y = y;
        this.radius = radius;
    }

    public bool IsPointStrictlyWithin(Point p)
    {
        return (u1 < p.x && p.x < u2) && (v1 < p.y && p.y < v2);
    }

    public bool IsPointWithin(int u, int v)
    {
        return (u1 <= u && u <= u2) && (v1 <= v && v <= v2);
    }
    public bool IsPointWithin(Point p)
    {
        return (u1 <= p.x && p.x <= u2) && (v1 <= p.y && p.y <= v2);
    }

    public HashSet<Point> GetIntersections(Sensor other)
    {
        if (other.sidelength > sidelength)
        {
            return other.GetIntersections(this);
        }

        HashSet<Point> intersections = new();
        bool ur, ul, lr, ll;
        ur = ul = lr = ll = false;

        int corners_inside = 0;
        if (IsPointWithin(other.u1, other.v1)) { corners_inside++; ll = true; }
        if (IsPointWithin(other.u1, other.v2)) { corners_inside++; ul = true; }
        if (IsPointWithin(other.u2, other.v1)) { corners_inside++; lr = true; }
        if (IsPointWithin(other.u2, other.v2)) { corners_inside++; ur = true; }

        if (corners_inside == 1)
        {
            if (ur)
            {
                intersections.Add(new(u1, other.v2));
                intersections.Add(new(other.u2, v1));
            }
            else if (ul)
            {
                intersections.Add(new(other.u1, v1));
                intersections.Add(new(u2, other.v2));
            }
            else if (lr)
            {
                intersections.Add(new(u1, other.v1));
                intersections.Add(new(other.u2, v2));
            }
            else if (ll)
            {
                intersections.Add(new(other.u1, v2));
                intersections.Add(new(u2, other.v1));
            }
        }
        else if (corners_inside == 2)
        {
            if (ul && ll)
            {
                intersections.Add(new(u2, other.v1));
                intersections.Add(new(u2, other.v2));
            }
            else if (ur && lr)
            {
                intersections.Add(new(u1, other.v1));
                intersections.Add(new(u1, other.v2));
            }
            else if (lr && ll)
            {
                intersections.Add(new(other.u1, v2));
                intersections.Add(new(other.u2, v2));
            }
            else if (ur && ul)
            {
                intersections.Add(new(other.u1, v1));
                intersections.Add(new(other.u2, v1));
            }
        }
        return intersections;
    }
}