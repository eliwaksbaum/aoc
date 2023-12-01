
List<Valve> valves = File.ReadAllLines("test.txt").Select(x => GetValve(x)).ToList();

CommutativeDict distances = new();
for (int i = 0; i < valves.Count; i++)
{
    BFS(valves[i], distances);
}
Console.WriteLine(distances);

Console.WriteLine(MaxPressure(30, 0, Valve.Get("AA"), new(), 0, valves.ToDictionary(v => v.name, v => v.name == "AA"? 0 : -1)));


Valve GetValve(string line)
{
    string[] words = line.Split(' ');
    string name = words[1];
    int ppm = int.Parse(words[4][5..^1]);
    List<string> neighbors = words[9..^0]
        .Select(item => item[^1] == ','? item[0..^1] : item)
        .ToList();

    return new Valve(name, ppm, neighbors);
}

void BFS(Valve start, CommutativeDict distances)
{
    HashSet<Valve> found = new();
    found.Add(start);

    Queue<Edge> q = new();
    q.Enqueue(new(start, 0));

    while(q.Count != 0)
    {
        Edge cur = q.Dequeue();
        foreach (String n_name in cur.v)
        {
            Valve n = Valve.Get(n_name);
            if (!found.Contains(n))
            {
                distances.Add(start.name, n_name, cur.d + 1);
                q.Enqueue(new(n, cur.d + 1));
                found.Add(n);
            }
        }
    }
}

int MaxPressure(int time_left, int pressure_released, Valve cur, HashSet<Valve> on, int ppm, Dictionary<String, int> prev_visits)
{
    if (time_left == 0)
    {
        return pressure_released;
    }
    else
    {
        List<int> paths = new();
        foreach (String n_name in cur)
        {
            if (prev_visits[n_name] < pressure_released + ppm)
            {
                Dictionary<String, int> update = new(prev_visits)
                {
                    [n_name] = pressure_released + ppm
                };
                paths.Add(MaxPressure(time_left - 1, pressure_released + ppm, Valve.Get(n_name), on, ppm, update));
            }
        }
        if (!on.Contains(cur))
        {
            HashSet<Valve> next_on = new(on) {cur};
            paths.Add(MaxPressure(time_left - 1, pressure_released + ppm, cur, next_on, ppm + cur.ppm, prev_visits));
        }
        return paths.Max();
    }
}

class Ranker : IComparer<Valve>
{
    readonly Valve cur;
    readonly CommutativeDict distances;
    readonly HashSet<Valve> on;

    public Ranker(Valve cur, CommutativeDict distances, HashSet<Valve> on)
    {
        this.cur = cur;
        this.distances = distances;
        this.on = on;
    }

    public int Compare(Valve? a, Valve? b)
    {
        int cur_to_a = distances.Get(cur.name, a.name);
        int cur_to_b = distances.Get(cur.name, b.name);
        int a_to_b = distances.Get(a.name, b.name);

        int appm = on.Contains(a)? 0 : a.ppm;
        int bppm = on.Contains(b)? 0 : b.ppm;
        return bppm * (cur_to_a + a_to_b - cur_to_b + 1) - appm * (cur_to_b + a_to_b - cur_to_a + 1);
    }
}

readonly record struct Status(int time_left, int pressure_released);

readonly record struct Edge(Valve v, int d);

class Valve : IEnumerable<string>
{
    public readonly string name;
    public readonly int ppm;
    readonly List<string> neighbors;

    static Dictionary<string, Valve> lookup = new();

    public Valve(string name, int ppm, List<string> neighbors)
    {
        this.name = name;
        this.ppm = ppm;
        this.neighbors = neighbors;
        lookup.Add(name, this);
    }

    public static Valve Get(string name)
    {
        return lookup[name];
    }

    public IEnumerator<string> GetEnumerator() => neighbors.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}

class CommutativeDict
{
    readonly record struct Key(string a, string b);
    readonly Dictionary<Key, int> dict = new();

    public void Add(string a, string b, int v)
    {
        dict.Add(new(a, b), v);
    }

    public int Get(string a, string b)
    {
        int v;
        if (!dict.TryGetValue(new(a, b), out v))
        {
            dict.TryGetValue(new(b, a), out v);
        }
        return v;
    }

    public override string ToString()
    {
        string m = "";
        foreach (var entry in dict)
        {
            m += $"{entry.Key.a} -> {entry.Key.b} : {entry.Value}\n";
        }
        return m;
    }
}