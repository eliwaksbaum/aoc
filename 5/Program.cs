using System.Text.RegularExpressions;

Stack<char>[] stacks = File.ReadAllLines("stacks.txt")
    .Select((line, i) =>
        line.Reverse().Aggregate(new Stack<char>(), (seed, next) => {
            seed.Push(next);
            return seed;
        })
    )
    .ToArray();

Stack<char>[] StacksCopy()
{
    var copy = new Stack<char>[stacks.Length];
    for (int i = 0; i < stacks.Length; i++)
    {
        copy[i] = new Stack<char>(stacks[i].Reverse());
    }
    return copy;
}

Regex num_reg = new Regex(@"\d{1,2}");
Move MoveFromLine(string line)
{
    int[] matches = num_reg.Matches(line)
        .Select((m, i) => Int32.Parse(m.Value))
        .ToArray();
    return new Move(){amount = matches[0], source = matches[1] - 1, target = matches[2] - 1};
}

Move[] moves = File.ReadAllLines("input.txt")
    .Skip(10)
    .Select((l, i) => MoveFromLine(l))
    .ToArray();

//1
Stack<char>[] work_stacks = StacksCopy();
foreach (Move m in moves)
{
    for (int i = 0; i < m.amount; i++)
    {
        char crate = work_stacks[m.source].Pop();
        work_stacks[m.target].Push(crate);
    }
}

string message = "";
foreach (Stack<char> stack in work_stacks)
{
    message += stack.Peek();
}
Console.WriteLine(message);

//2
work_stacks = StacksCopy();
foreach (Move m in moves)
{
    Stack<char> intermediate = new();
    for (int i = 0; i < m.amount; i++)
    {
        char crate = work_stacks[m.source].Pop();
        intermediate.Push(crate);
    }
    while (intermediate.Count > 0)
    {
       work_stacks[m.target].Push(intermediate.Pop());
    }
}

message = "";
foreach (Stack<char> stack in work_stacks)
{
    message += stack.Peek();
}
Console.WriteLine(message);


readonly struct Move
{
    public readonly int amount {get; init;}
    public readonly int source {get; init;}
    public readonly int target {get; init;}

    public override string ToString() => $"{amount} from {source} to {target}";
}