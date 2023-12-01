List<int> foo = File.ReadAllLines("input.txt").Select(x => int.Parse(x)).ToList();

Console.WriteLine(foo.Count * foo.Count);

int L = foo.Count;
int asks = foo.Select(x => ((Math.Abs(x)/L + 1) * L) + x)
    .Select(x => x%L)
    .Sum();

Console.WriteLine(asks);