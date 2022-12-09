string[] lines = File.ReadAllLines("input.txt");

IEnumerable<int[]> assignment_pairs = lines.Select(
        (line, i) => line.Split(',')
        .SelectMany((assignment) => assignment.Split('-'))
        .Select((num) => Int32.Parse(num))
        .ToArray()
    );

//1
bool OneContainsOther(int[] pair)
{
    return
    (pair[0] <= pair[2] && pair[1] >= pair[3])
    ||
    (pair[0] >= pair[2] && pair[1] <= pair[3]);
}

int num_contains = assignment_pairs
    .Where((a_pair) => OneContainsOther(a_pair))
    .Count();
Console.WriteLine(num_contains);

//2
bool Overlaps(int[] pair)
{
    return !(pair[1] < pair[2] || pair[0] > pair[3]);
}

int num_overlaps = assignment_pairs
    .Where((a_pair) => Overlaps(a_pair))
    .Count();
Console.WriteLine(num_overlaps);