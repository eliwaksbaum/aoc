var input = File.ReadAllLines("input.txt");

//1
int sum = input
    .Chunk(3)
    .Select(chunk => new Tuple<Packet, Packet> (Extract(chunk[0], 0, out int _), Extract(chunk[1], 0, out int _)))
    .Select(pair => new PacketVisitor().Compare(pair.Item1, pair.Item2))
    .Select((result, i) => (result < 0)? i + 1 : 0)
    .Sum();

Console.WriteLine(sum);

//2
List<Tuple<Packet, string>> ordered = input
    .Where(s => !String.IsNullOrWhiteSpace(s))
    .Prepend("[[2]]")
    .Prepend("[[6]]")
    .Select(line => new Tuple<Packet, string> (Extract(line, 0, out int _), line))
    .OrderBy(x => x.Item1, new PacketVisitor())
    .ToList();

int two = ordered.FindIndex(x => x.Item2 == "[[2]]") + 1;
int six = ordered.FindIndex(x => x.Item2 == "[[6]]") + 1;
Console.WriteLine(two * six);


Packet Extract(string s, int start, out int end)
{
    CompositePacket top = new();
    for (int i = start + 1; i < s.Length; i++)
    {
        switch (s[i])
        {
            case '[':
                int skip;
                top.Add(Extract(s, i, out skip));
                i = skip;
                break;
            case '1':
                if (s[i+1] == '0')   //yeah, yeah. sue me
                {
                    top.Add(new SinglePacket(10));
                    i++;
                }
                else
                {
                    top.Add(new SinglePacket(1));
                }
                break;
            case var c when char.IsAsciiDigit(c):
                top.Add(new SinglePacket(int.Parse(s[i..(i+1)])));
                break;
            case ',':
                break;
            case ']':
                end = i;
                return top;
        }
    }
    throw new Exception();
}

class PacketVisitor : IComparer<Packet>
{
    SinglePacket? s_left;
    CompositePacket? c_left;
    int inorder;

    public int Compare(Packet? left, Packet? right)
    {
        if (left is null) { return -1; }
        else if (right is null) { return 1; }
        else
        {
            left.AcceptAsLeft(this);
            right.AcceptAsRight(this);
            return inorder;
        }
    }

    public void VisitLeft(SinglePacket left)
    {
        s_left = left;
    }
    public void VisitLeft(CompositePacket left)
    {
        c_left = left;
    }
    public void VisitRight(SinglePacket right)
    {
        if (s_left is not null)
        {
            inorder = Compare(s_left, right);
        }
        if (c_left is not null)
        {
            inorder = Compare(c_left, new(right));
        }
    }
    public void VisitRight(CompositePacket right)
    {
        if (c_left is not null)
        {
            inorder = Compare(c_left, right);
        }
        if (s_left is not null)
        {
            inorder = Compare(new(s_left), right);
        }
    }
    
    static int Compare(SinglePacket left, SinglePacket right) => left.Num - right.Num;

    static int Compare(CompositePacket left, CompositePacket right)
    {
        for (int i = 0; i < left.Count; i++)
        {
            if (i >= right.Count)
            {
                return 1;
            }

            PacketVisitor v = new();
            int ri = v.Compare(left[i], right[i]);
            
            if (ri == 0)
            {
                continue;
            }
            else
            {
                return ri;
            }
        }
        return (left.Count == right.Count)? 0 : -1;
    }
}

interface Packet
{
    public void AcceptAsLeft(PacketVisitor visitor);
    public void AcceptAsRight(PacketVisitor visitor);
}

class SinglePacket : Packet
{
    public readonly int Num;

    public SinglePacket(int i)
    {
        Num = i;
    }

    public void AcceptAsLeft(PacketVisitor visitor)
    {
        visitor.VisitLeft(this);
    }
    public void AcceptAsRight(PacketVisitor visitor)
    {
        visitor.VisitRight(this);
    }
}

class CompositePacket : Packet
{
    readonly List<Packet> elements = new();
    public int Count => elements.Count;
    public Packet this[int i] => elements[i];

    public CompositePacket() {}
    public CompositePacket(SinglePacket s)
    {
        Add(s);
    }

    public void Add(Packet p)
    {
        elements.Add(p);
    }

    public void AcceptAsLeft(PacketVisitor visitor)
    {
        visitor.VisitLeft(this);
    }
    public void AcceptAsRight(PacketVisitor visitor)
    {
        visitor.VisitRight(this);
    }
}