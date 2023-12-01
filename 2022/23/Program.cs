string[] input = File.ReadAllLines("input.txt");

List<Elf> elves = new();
HashSet<Point> elfTracker = new();

for (int y = 0; y < input.Length; y++)
{
    string row = input[y];
    for (int x = 0; x < row.Length; x++)
    {
        if (row[x] == '#')
        {
            elves.Add(new Elf(x, y, elfTracker));
        }
    }
}

//1
for (int i = 0; i < 10; i++)
{
    DoRound(i);
}

int empty = 0;
var Xs = elfTracker.Select(p => p.x);
var Ys = elfTracker.Select(p => p.y);
for (int y = Ys.Min(); y <= Ys.Max(); y++)
{
    for (int x = Xs.Min(); x <= Xs.Max(); x++)
    {
        if (!elfTracker.Contains(new(x,y)))
        {
            empty++;
        }
    }
}
Console.WriteLine(empty);

//2
int round = 10;
while (true)
{
    Elf.NewRound();
    DoRound(round);

    if(Elf.NumMoved == 0)
    {
        break;
    }
    round++;
}
Console.WriteLine(round + 1);

void DoRound(int round)
{
    Dictionary<Proposal, int> proposals = new();

    foreach (Elf elf in elves)
    {
        Proposal? p = elf.GetProposal(round);
        if (p is not null)
        {
            if (proposals.TryGetValue(p, out int count))
            {
                proposals[p] = count + 1;
            }
            else
            {
                proposals[p] = 1;
            }
        }
    }

    foreach (var entry in proposals.Where(pair => pair.Value == 1))
    {
        entry.Key.Accept();
    }
}

class Proposal
{
    readonly Elf elf;
    readonly Point destination;

    public Proposal(Elf elf, Point destination)
    {
        this.elf = elf;
        this.destination = destination;
    }

    public void Accept()
    {
        elf.Move(destination);
    }

    public override bool Equals(object? obj)
    {
        if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
        {
            return false;
        } 
        Proposal p = (Proposal) obj;
        return destination.Equals(p.destination);
    }

    public override int GetHashCode()
    {
        return destination.GetHashCode();
    }
}

class Elf
{
    Point location;
    readonly HashSet<Point> elfTracker;

    public Elf(int x, int y, HashSet<Point> elfTracker)
    {
        location = new(x, y);
        this.elfTracker = elfTracker;
        elfTracker.Add(location);
    }

    public void Move(Point next)
    {
        elfTracker.Remove(location);
        location = next;
        elfTracker.Add(location);
        numMoved++;
    }

    public Proposal? GetProposal(int round)
    {
        if (Get8Neighbors(location).All(n => !elfTracker.Contains(n)))
        {
            return null;
        }
        for (int i = 0; i < 4; i++)
        {
            int list_position = ((round % 4) + i) % 4;
            if (Get3Neighbors(list_position, location).All(n => !elfTracker.Contains(n)))
            {
                return list_position switch {
                    0 => new Proposal(this, location with {y = location.y - 1}),
                    1 => new Proposal(this, location with {y = location.y + 1}),
                    2 => new Proposal(this, location with {x = location.x - 1}),
                    3 => new Proposal(this, location with {x = location.x + 1}),
                    _ => throw new Exception()
                };
            }
        }
        return null;
    }

    static int numMoved = 0;
    public static int NumMoved => numMoved;
    public static void NewRound()
    {
        numMoved = 0;
    }

    static Point[] Get3Neighbors(int i, Point location)
    {
        int x = location.x;
        int y = location.y;
        return i switch {
            //N, NE, NW
            0 => new Point[] {new(x, y - 1), new(x + 1, y - 1), new(x - 1, y - 1)},
            //S, SE, SW
            1 => new Point[] {new(x, y + 1), new(x + 1, y + 1), new(x - 1, y + 1)},
            //W, NW, SW
            2 => new Point[] {new(x - 1, y), new(x - 1, y - 1), new(x - 1, y + 1)},
            //E, NE, SE
            3 => new Point[] {new(x + 1, y), new(x + 1, y - 1), new(x + 1, y + 1)},
            _ => throw new Exception()
        };
    }

    static Point[] Get8Neighbors(Point location)
    {
        int x = location.x;
        int y = location.y;
        return new Point[] {new(x, y - 1), new(x + 1, y - 1), new(x - 1, y - 1), new(x, y + 1), new(x + 1, y + 1), new(x - 1, y + 1), new(x - 1, y), new(x + 1, y)};
    }
}

readonly record struct Point (int x, int y);