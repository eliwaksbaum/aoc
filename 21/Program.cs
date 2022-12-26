string[] input1 = File.ReadAllLines("input.txt");

List<NumberMonkey> starters = new();
List<ComputerMonkey> waiters = new();
Parse(input1, starters, waiters);

//1
foreach(ComputerMonkey m in waiters)
{
    m.Subscribe();
}

foreach(NumberMonkey m in starters)
{
    m.Go();
}
Console.WriteLine(Monkey.Get("root").GetValue());

//2
double goal = Monkey.Get("pfjc").GetValue();

int first_power = 0;
for (int i = 0; i < 30; i += 1)
{
    double result = Run("lttc", Math.Pow(10, i));
    if (result < 0)
    {
        first_power = i-1;
        break;
    }
}

int power_space = first_power + 1;
List<int> digits = new();
while (power_space > 2)
{
    double[] results = new double[10];
    for (int i = 0; i < 10; i++)
    {
        double test = Narrow(first_power, digits) + Math.Pow(10, power_space - 1) * i;        
        double result = Run("lttc", test);
        results[i] = result; 
    }

    if (results.All(x => x < goal))
    {
        digits.Add(0);
    }
    if (results.All(x => x > goal))
    {
        digits.Add(9);
    }
    else
    {
        for (int j = 0; j < 10; j++)
        {
            if (results[j] < goal)
            {
                digits.Add(j-1);
                break;
            }
        }
    }
    power_space--;
}

for (double i = 0; i < 100; i++)
{
    double humn = Narrow(first_power, digits) + i;
    double result = Run("lttc", humn);
    if (goal - result == 0)
    {
        Console.WriteLine(humn);
        break;
    }
}



double Run(string interest, double humn)
{
    Monkey.ResetAll();
    starters.Remove((NumberMonkey) Monkey.Get("humn"));
    starters.Add(new("humn", humn));
    ((ComputerMonkey) Monkey.Get("sjth")).Subscribe();
    
    foreach(NumberMonkey m in starters)
    {
        m.Go();
    }

    return Monkey.Get(interest).GetValue();
}

double Narrow(int start, List<int> digits)
{
    double n = Math.Pow(10, start);
    for (int i = 0; i < digits.Count; i++)
    {
        n += digits[i] * Math.Pow(10, start - i);
    }
    return n;
}

void Parse(string[] input, List<NumberMonkey> starters, List<ComputerMonkey> waiters)
{
    foreach (string line in input)
    {
        string[] words = line.Split(' ');
        string name = words[0][0..^1];
        if (int.TryParse(words[1], out int num))
        {
            starters.Add(new(name, num));
        }
        else
        {
            Func<double, double, double> computer = words[2] switch {
                "+" => (a, b) => a + b,
                "-" => (a, b) => a - b,
                "*" => (a, b) => a * b,
                "/" => (a, b) => a / b,
                _ => throw new Exception()
            };
            waiters.Add(new ComputerMonkey(name, words[1], words[3], computer));
        }
    }
}

record Result(double value);

abstract class Monkey
{
    public readonly string name;
    public event Action<double>? Shout;
    double value;

    public Monkey(string name)
    {
        this.name = name;
        lookup[name] = this;
    }

    public double GetValue() => value;

    protected void Ready(double value)
    {
        this.value = value;
        if (Shout is not null)
        {
            Shout(value);
        }
    }

    protected abstract void Reset();

    static Dictionary<string, Monkey> lookup = new();
    public static Monkey Get(string name) => lookup[name];
    public static void ResetAll()
    {
        foreach (Monkey m in lookup.Values)
        {
            m.Reset();
        }
    }
}

class ComputerMonkey : Monkey
{
    readonly string m1;
    readonly string m2;
    Result? a;
    Result? b;
    Func<double, double, double> Compute;

    public ComputerMonkey(string name, string m1, string m2, Func<double, double, double> Computer) : base(name)
    {
        this.m1 = m1;
        this.m2 = m2;
        Compute = Computer;
    }

    protected override void Reset()
    {
        a = null;
        b = null;
    }

    public void Subscribe()
    {
        Monkey.Get(m1).Shout += M1;
        Monkey.Get(m2).Shout += M2;
    }

    void M1(double n)
    {
        a = new(n);
        if (b is not null)
        {
            Ready(Compute(a.value, b.value));
        }
    }

    void M2(double n)
    {
        b = new(n);
        if (a is not null)
        {
            Ready(Compute(a.value, b.value));
        }
    }
}

class NumberMonkey : Monkey
{
    readonly double num;

    public NumberMonkey(string name, double num) : base(name)
    {
        this.num = num;
    }

    public void Go()
    {
        Ready(num);
    }

    protected override void Reset() {}
}