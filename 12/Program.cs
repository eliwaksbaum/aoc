string[] input = File.ReadAllLines("input.txt");

int[][] map = input
    .Select( line =>
        line.Select( car =>
            {
                int height = (int) car;
                if (car == 'S') {height = (int) 'a';}
                if (car == 'E') {height = (int) 'z';}
                return height;
            }
        )
        .Prepend(-1)
        .Append(-1)
        .ToArray()
    )
    .Prepend(Enumerable.Repeat(-1, input[0].Length + 2).ToArray())
    .Append(Enumerable.Repeat(-1, input[0].Length + 2).ToArray())
    .ToArray();

//Manual!!
Node.SetDest(92, 21);
//Manual!!

for (var y = 1; y < map.Length - 1; y++)
{
    for (var x = 1; x < map[0].Length - 1; x++)
    {
        Node cur = Node.Get(x, y);
        int cur_height = map[y][x];

        Point[] potential_neighbors = new Point[] {
            new Point(x-1, y), 
            new Point(x+1, y),
            new Point(x, y-1),
            new Point(x, y+1)
        };

        foreach (Point p in potential_neighbors)
        {
            if (map[p.y][p.x] - cur_height <= 1)
            {
                cur.AddNeighbor(Node.Get(p));
            }
        }
    }
}

//1
Console.WriteLine(AStar(Node.Get(1,21), Node.Get(92, 21)));

//2
PriorityQueue<Node, int> starts = new();

for (var y = 1; y < map.Length - 1; y++)
{
    for (var x = 1; x < map[0].Length - 1; x++)
    {
        if (map[y][x] == 97)
        {
            Node.Reset();
            Node cur = Node.Get(x, y);
            starts.Enqueue(cur, AStar(cur, Node.Get(92, 21)));
        }
    }
}
int shortest_path;
starts.TryPeek(out Node? e, out shortest_path);
Console.WriteLine(shortest_path);


int AStar(Node start, Node end)
{
    HashSet<Node> visited = new();
    PriorityQueue<Node, int> frontier = new();
    
    start.steps_to = 0;
    Node cur = start;
    while (cur != end)
    {
        foreach (Node neighbor in cur)
        {
            if (!visited.Contains(neighbor))
            {
                int tentative_steps = neighbor.steps_to;
                int current_steps = cur.steps_to + 1;
                if (current_steps < tentative_steps)
                {
                    neighbor.steps_to = current_steps;
                    frontier.Enqueue(neighbor, current_steps + neighbor.manhattan);
                }
            }
        }
        visited.Add(cur);
        if (!frontier.TryDequeue(out cur, out int p))
        {
            //No path from start to end
            return int.MaxValue;
        }
    }
    return cur.steps_to;
}

class Node : System.Collections.IEnumerable
{
    readonly Point pos;
    readonly List<Node> neighbors = new();
    public readonly int manhattan;
    public int steps_to = 1000000000;

    private Node(Point p)
    {
        pos = p;
        manhattan = Math.Abs(dest.x - p.x) + Math.Abs(dest.y - p.y);
    }

    public System.Collections.IEnumerator GetEnumerator() => neighbors.GetEnumerator();

    public void AddNeighbor(Node neighbor)
    {
        neighbors.Add(neighbor);
    }

    static Dictionary<Point, Node> flyStore = new();
    static Point dest;
    
    public static Node Get(Point pos)
    {
        Node? n;
        if (!flyStore.TryGetValue(pos, out n))
        {
            n = new Node(pos);
            flyStore.Add(pos, n);
        }
        return n;

    }
    public static Node Get(int x, int y)
    {
        return Get(new Point(x, y));
    }

    public static void SetDest(int x, int y)
    {
        dest = new Point(x, y);
    }

    public static void Reset()
    {
        foreach (Node n in flyStore.Values)
        {
            n.steps_to = 1000000000;
        }
    }
}

readonly record struct Point(int x, int y);