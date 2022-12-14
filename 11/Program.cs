string[] input = File.ReadAllLines("input.txt");

int[] domains = input.Chunk(7).Select(chunk => int.Parse(chunk[3][^2..^0])).ToArray();

Monkey[] monkeys1 = input.Chunk(7).Select(chunk => GetMonkey(chunk, domains)).ToArray();
Monkey[] monkeys2 = input.Chunk(7).Select(chunk => GetMonkey(chunk, domains)).ToArray();

//1
for(int i = 0; i < 20; i++)
{
    Round(monkeys1, true);
}
Console.WriteLine(MonkeyBusiness(monkeys1));

//2
for(int i = 0; i < 10000; i++)
{
    Round(monkeys2, false);
}
Console.WriteLine(MonkeyBusiness(monkeys2));

double MonkeyBusiness(Monkey[] monkeys) => monkeys
    .Select(m => m.Count)
    .OrderDescending()
    .Take(2)
    .Aggregate(1d, (a,b) => a * b);

void Round(Monkey[] monkeys, bool manage)
{
    foreach(Monkey m in monkeys)
    {
        List<Throw> throws = m.Turn(manage);
        foreach (Throw t in throws)
        {
            monkeys[t.monkey].Catch(t.item);
        }
    }
}

Monkey GetMonkey(string[] lines, int[] domains)
{
    List<Item> si = GetStartingItems(lines[1], domains);
    Monkey.Operation o = GetOperation(lines[2]);
    Monkey.Test t = GetTest(lines[3..6]);
    return new Monkey(si, o, t);
}

List<Item> GetStartingItems(string line, int[] domains)
{
    return line[17..^0]
        .Split(',')
        .Select(num => int.Parse(num))
        .Select(worry => new Item(worry, domains))
        .ToList();
}

Monkey.Operation GetOperation(string line)
{
    if (line[^3..^0] == "old")
    {
        return (x) => x.Square();
    }
    else
    {
        char op = line[23];
        int term = int.Parse(line.Split(op)[1]);
        switch (op)
        {
            case '+':
                return (x) => x.Add(term);
            case '*':
                return (x) => x.Multiply(term);
            default:
                throw new Exception();
        }
    }
}

Monkey.Test GetTest(string[] lines)
{
    int mod = int.Parse(lines[0][^2..^0]);
    int trueTarget = int.Parse(lines[1][^1..^0]);
    int falseTarget = int.Parse(lines[2][^1..^0]);
    return (x) => x.Passes(mod)? trueTarget : falseTarget;
}

class Item
{
    private int raw;
    private Dictionary<int, int> worries = new();

    public Item(int rawItem, int[] domains)
    {
        raw = rawItem;
        foreach (int mod in domains)
        {
            worries.Add(mod, rawItem % mod);
        }
    }

    public bool Passes(int mod)
    {
        return worries[mod] == 0;
    }

    public void Square()
    {
        raw = raw * raw;
        foreach (int mod in worries.Keys)
        {
            worries[mod] = (worries[mod] * worries[mod]) % mod;
        }
    }
    public void Multiply(int term)
    {
        raw *= term;
        foreach (int mod in worries.Keys)
        {
            worries[mod] = (worries[mod] * term) % mod;
        }
    }
    public void Add(int term)
    {
        raw += term;
        foreach (int mod in worries.Keys)
        {
            worries[mod] = (worries[mod] + term) % mod;
        }
    }
    public void Manage()
    {
        raw /= 3;
        foreach (int mod in worries.Keys)
        {
            worries[mod] = (raw % mod);
        }
    }
}

readonly record struct Throw(Item item, int monkey);

class Monkey
{
    public delegate void Operation(Item item);
    public delegate int Test(Item item);

    private readonly List<Item> items;
    private readonly Operation operation;
    private readonly Test test;

    private int count;
    public int Count => count;

    public Monkey(List<Item> pItems, Operation pOperation, Test pTest)
    {
        items = pItems;
        operation = pOperation;
        test = pTest;
    }

    public List<Throw> Turn(bool manage)
    {
        List<Throw> throws = new();
        foreach (Item item in items)
        {
            count += 1;
            operation(item);
            if (manage)
            {
                item.Manage();
            }
            int monkey = test(item);
            throws.Add(new Throw(item, monkey));
        }
        items.Clear();
        return throws;
    }

    public void Catch(Item item)
    {
        items.Add(item);
    }
}