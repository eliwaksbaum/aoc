string[] input = File.ReadAllLines("input.txt");

List<List<Point>> rockpaths = input
    .Select(line => line.Split("->")
        .Select(point => {
            string[] nums = point.Split(',');
            return new Point(int.Parse(nums[0]), int.Parse(nums[1]));
        })
        .ToList()
    )
    .ToList();

bool[][] GetCave()
{
    bool[][] cave = new bool[800][];
    for (int i = 0; i < 800; i++)
    {
        cave[i] = new bool[200];
    }

    foreach (List<Point> rockpath in rockpaths)
    {
        for (int i = 1; i < rockpath.Count; i++)
        {
            Point left = rockpath[i-1];
            Point right = rockpath[i];

            if (left.x == right.x)
            {
                Point start = (left.y < right.y)? left : right;
                Point end = (start == right)? left : right;

                for (int j = start.y; j < end.y + 1; j++)
                {
                    cave[left.x][j] = true;
                }
            }

            if (left.y == right.y)
            {
                Point start = (left.x < right.x)? left : right;
                Point end = (start == right)? left : right;

                for (int j = start.x; j < end.x + 1; j++)
                {
                    cave[j][left.y] = true;
                }
            }
        }
    }
    return cave;
}

//1
bool[][] cave = GetCave();
int count = 0;
while (TripleDrop(cave, 500, 0) != -1)
{
    count++;
}
Console.WriteLine(count);

//2
int floor = rockpaths.SelectMany(path => path.Select(point => point.y)).Max() + 2;
rockpaths.Add(new() { new Point(0, floor), new Point(799, floor) });
cave = GetCave();
count = 0;
while (!cave[500][0])
{
    TripleDrop(cave, 500, 0);
    count++;
}
Console.WriteLine(count);


int TripleDrop(bool[][] cave, int mid, int start)
{
    int mid_rest = Drop(cave[mid], start);
    if (mid_rest == -1)
    {
        return -1;
    }
    
    int left_rest = Drop(cave[mid-1], mid_rest);
    if (left_rest == -1)
    {
        return -1;
    }
    if (left_rest != mid_rest)
    {
        return TripleDrop(cave, mid-1, mid_rest);
    }
    
    int right_rest = Drop(cave[mid+1], mid_rest);
    if (right_rest == -1)
    {
        return -1;
    }
    if (right_rest != mid_rest)
    {
        return TripleDrop(cave, mid+1, mid_rest);
    }

    cave[mid][mid_rest] = true;
    return mid_rest;
}

int Drop(bool[] column, int start)
{
    for (int i = start; i < 199; i++)
    {
        if (column[i+1] == true)
        {
            return i;
        }
    }
    return -1;
}

readonly record struct Point(int x, int y);