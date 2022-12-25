List<Point> cubes = File.ReadAllLines("input.txt")
    .Select(line => line.Split(','))
    .Select(split => new Point(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])))
    .ToList();

//1
HashSet<Point> scan = new();
HashSet<Point> airSkin = new();
int sides = 0;
foreach (Point p in cubes)
{
    foreach (Point n in GetSixNeighbors(p))
    {
        if (scan.Contains(n))
        {
            sides -= 2;
        }
        else
        {
            airSkin.Add(n);
        }
    }
    sides += 6;
    scan.Add(p);
    airSkin.Remove(p);
}
Console.WriteLine(sides);

//2
var x = scan.Select(p => p.x);
var y = scan.Select(p => p.y);
var z = scan.Select(p => p.z);
int maxX = x.Max();
int minX = x.Min();
int maxY = y.Max();
int minY = y.Min();
int maxZ = z.Max();
int minZ = z.Min();

Point outside_the_box = new(maxX + 1, maxY, maxZ);
HashSet<Point> reached = new() {outside_the_box};
Queue<Point> q = new(9000);
q.Enqueue(outside_the_box);

while(q.Count != 0)
{
    Point cur = q.Dequeue();
    foreach (Point n in GetSixNeighbors(cur))
    {
        if (!scan.Contains(n)
            && minX - 1 <= n.x && n.x <= maxX + 1
                && minY - 1 <= n.y && n.y <= maxY + 1
                    && minZ - 1 <= n.z && n.z <= maxZ + 1)
        {
            if (!reached.Contains(n))
            {
                q.Enqueue(n);
                reached.Add(n);
            }
        }
    }
}

airSkin.ExceptWith(reached);
foreach (Point p in airSkin)
{
    foreach (Point n in GetSixNeighbors(p))
    {
        if (scan.Contains(n))
        {
            sides -= 1;
        }
    }
}
Console.WriteLine(sides);


Point[] GetSixNeighbors(Point p)
{
    Point[] six = new Point[6];
    six[0] = p with {x = p.x + 1};
    six[1] = p with {x = p.x - 1};
    six[2] = p with {y = p.y + 1};
    six[3] = p with {y = p.y - 1};
    six[4] = p with {z = p.z + 1};
    six[5] = p with {z = p.z - 1};
    return six;
}

readonly record struct Point(int x, int y, int z);