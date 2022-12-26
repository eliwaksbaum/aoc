string[] map = File.ReadAllLines("map.txt");
string path = File.ReadAllText("path.txt");

Dictionary<int, MinMax> colBounds = new();
Dictionary<int, MinMax> rowBounds = new();

//rowBounds
for (int i = 0; i < map.Length; i++)
{
    string row = map[i];
    int min = -1;
    for (int j = 0; j < row.Length; j++)
    {
        if (row[j] == '.' || row[j] == '#')
        {
            min = j;
            break;
        }
    }
    rowBounds.Add(i, new(min, row.Length - 1));
}

//colBounds
int maxCol = rowBounds.Values.Select(m => m.max).Max();
for (int col = 0; col <= maxCol; col++)
{
    int min = -1;
    int max = -1;
    for (int i = 0; i < map.Length; i++)
    {
        if (map[i].Length <= col)
        {
            if (min == -1)
            {
                continue;
            }
            else
            {
                max = i - 1;
                break;
            }
        }
        else if ((map[i][col] == '.' || map[i][col] == '#'))
        {
            if (min == -1)
            {
                min = i;
            }
        }
        else if (min != -1 && max == -1)
        {
            max = i - 1;
            break;
        }
    }
    if (max == -1)
    {
        max = map.Length - 1;
    }
    colBounds.Add(col, new(min, max));
}

//1
for (int y = 0; y < map.Length; y++)
{
    string row = map[y];
    MinMax rowBound = rowBounds[y];
    for (int x = rowBound.min; x <= rowBound.max; x++)
    {
        if (row[x] == '.')
        {
            MinMax colBound = colBounds[x];
            Tile cur = Tile.Get(x, y);

            Point left = (x != rowBound.min)? new(x - 1, y) : new(rowBound.max, y);
            Point right = (x != rowBound.max)? new(x + 1, y) : new(rowBound.min, y);
            Point up = (y != colBound.min)? new(x, y-1) : new(x, colBound.max);
            Point down = (y != colBound.max)? new(x, y+1) : new(x, colBound.min);

            if (map[left.y][left.x] == '.') { cur.left = new(Tile.Get(left), 0); }
            if (map[right.y][right.x] == '.') { cur.right = new(Tile.Get(right), 0); }
            if (map[up.y][up.x] == '.') { cur.up = new(Tile.Get(up), 0); }
            if (map[down.y][down.x] == '.') { cur.down = new(Tile.Get(down), 0); }
        }
    }
}
Walk();

//2
Tile.Clear();
for (int y = 0; y < map.Length; y++)
{
    string row = map[y];
    MinMax rowBound = rowBounds[y];
    for (int x = rowBound.min; x <= rowBound.max; x++)
    {
        if (row[x] == '.')
        {
            MinMax colBound = colBounds[x];
            Tile cur = Tile.Get(x, y);

            Point left;
            int left_flip = 0;
            if (x == rowBound.min)
            {
                switch (y)
                {
                     //A to D, face right
                    case (>= 0 and <= 49):
                        left = new Point(0, 149 - y);
                        left_flip = 2;
                        break;
                    //B to D, face down
                    case (>= 50 and <= 99):
                        left = new Point(y - 50, 100);
                        left_flip = 3;
                        break;
                    //D to A, face right
                    case (>= 100 and <= 149):
                        left = new Point(50, 149 - y);
                        left_flip = 2;
                        break;
                    //E to A, face down
                    case (>= 150 and <= 199):
                        left = new Point(y-100, 0);
                        left_flip = 3;
                        break;
                    default:
                        throw new Exception();
                }
            }
            else
            {
                left = new(x-1, y);
            }

            Point right;
            int right_flip = 0;
            if (x == rowBound.max)
            {
                switch (y)
                {
                    //F to C, face left
                    case (>= 0 and <= 49):
                        right = new Point(99, 149 - y);
                        right_flip = 2;
                        break;
                    //B to F, face up
                    case (>= 50 and <= 99):
                        right = new Point(y + 50, 49);
                        right_flip = 3;
                        break;
                    //C to F, face left
                    case (>= 100 and <= 149):
                        right = new Point(149, 149 - y);
                        right_flip = 2;
                        break;
                    //E to C, face up
                    case (>= 150 and <= 199):
                        right = new Point(y - 100, 149);
                        right_flip = 3;
                        break;
                    default:
                        throw new Exception();
                }
            }
            else
            {
                right = new Point(x+1, y);
            }

            Point up;
            int up_flip = 0;
            if (y == colBound.min)
            {
                switch (x)
                {
                    //D to B, face right
                    case (>= 0 and <= 49):
                        up = new Point(50, x + 50);
                        up_flip = 1;
                        break;
                    //A to E, face right
                    case (>= 50 and <= 99):
                        up = new Point(0, x + 100);
                        up_flip = 1;
                        break;
                    //F to E, face up
                    case (>= 100 and <= 149):
                        up = new Point(x - 100, 199);
                        up_flip = 0;
                        break;
                    default:
                        throw new Exception();
                };
            }
            else
            {
                up = new Point(x, y-1);
            }

            Point down;
            int down_flip = 0;
            if (y == colBound.max)
            {
                switch (x)
                {
                    //E to F, face down
                    case (>= 0 and <= 49):
                        down = new Point(x + 100, 0);
                        down_flip = 0;
                        break;
                    //C to E, face left
                    case (>= 50 and <= 99):
                        down = new Point(49, x + 100);
                        down_flip = 1;
                        break;
                    //F to B, face left
                    case (>= 100 and <= 149):
                        down = new Point(99, x - 50);
                        down_flip = 1;
                        break;
                    default:
                        throw new Exception();
                }
            }
            else
            {
                down = new Point(x, y+1);
            }

            if (map[left.y][left.x] == '.') { cur.left = new(Tile.Get(left), left_flip); }
            if (map[right.y][right.x] == '.') { cur.right = new(Tile.Get(right), right_flip); }
            if (map[up.y][up.x] == '.') { cur.up = new(Tile.Get(up), up_flip); }
            if (map[down.y][down.x] == '.') { cur.down = new(Tile.Get(down), down_flip); }
        }
    }
}
Walk();

void Walk()
{
    Player me = new(rowBounds[0].min, 0);
    string num = "";

    foreach (char car in path)
    {
        if (car == 'L' || car == 'R')
        {
            if (int.TryParse(num, out int steps))
            {
                me.Move(steps);
                num = "";
            }

            if (car == 'L')
            {
                me.TurnLeft();
            }
            else
            {
                me.TurnRight();
            }
        }
        else
        {
            num += car;
        }
    }
    me.Move(int.Parse(num));
    Console.WriteLine(me.GetPassword());
}

class Player
{
    Tile cur;
    int facing = 0;

    public Player(int x, int y)
    {
        cur = Tile.Get(x, y);
    }

    public int GetPassword() => 1000 * (cur.y + 1) + 4 * (cur.x + 1) + facing;

    public void TurnLeft()
    {
        facing = (facing + 3) % 4;
    }
    public void TurnRight()
    {
        facing = (facing + 1) % 4;
    }

    public void Move(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Step? next = facing switch
            {
                0 => cur.right,
                1 => cur.down,
                2 => cur.left,
                3 => cur.up,
                _ => throw new Exception()
            };
            if (next is not null)
            {
                cur = next.tile;
                facing = (facing + next.flip) % 4;
            }
            else
            {
                break;
            }
        }
    }
}

readonly record struct MinMax(int min, int max);
readonly record struct Point(int x, int y);
record Step(Tile tile, int flip);

class Tile
{
    public Step? right;
    public Step? left;
    public Step? up;
    public Step? down;

    readonly Point loc;
    public int x => loc.x;
    public int y => loc.y;

    private Tile(Point point)
    {
        loc = point;
    }

    static Dictionary<Point, Tile> lookup = new();
    public static Tile Get(Point point)
    {
        if (!lookup.ContainsKey(point))
        {
            lookup[point] = new Tile(point);
        }
        return lookup[point];
    }
    public static Tile Get(int x, int y) => Get(new(x, y));
    public static void Clear()
    {
        lookup = new();
    }
}