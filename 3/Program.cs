string[] rucksacks = File.ReadAllLines("input.txt");

//1
int item_sum = rucksacks.Select((r, i) => r.Chunk(r.Length/2).ToList())
    .Select((compartments, i) => compartments[0].Intersect(compartments[1]))
    .Select((shared, i) => (int) shared.First())
    .Select((ascii, i) => ascii > 96 ? ascii - 96 : ascii - 38)
    .Sum();

Console.WriteLine(item_sum);

//2
int badge_sum = rucksacks.Chunk(3)
    .Select((group, i) =>
        group.Aggregate((a, b) => new string(a.Intersect(b).ToArray()))
    )
    .Select((shared, i) => (int) shared.First())
    .Select((ascii, i) => ascii > 96 ? ascii - 96 : ascii - 38)
    .Sum();

Console.WriteLine(badge_sum);