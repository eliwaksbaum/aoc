string[] rows = File.ReadAllLines("input.txt");

Tree[][] fromLeft = rows
    .Select((row, y) => row.
        Select(
            (car, x) => new Tree ((int) Char.GetNumericValue(car), x, y)
        ).ToArray()
    ).ToArray();

Tree[][] fromRight = fromLeft
    .Select(
        (row) => (row.Reverse().ToArray())
    ).ToArray();

Tree[][] fromTop = new Tree[rows[0].Length][];
for (int i = 0; i < fromLeft[0].Length; i++)
{
    List<Tree> list = new();
    for (int j = 0; j < fromLeft.Length; j++)
    {
        list.Add(fromLeft[j][i]);
    }
    fromTop[i] = list.ToArray();
}

Tree[][] fromBottom = fromTop
    .Select(
        (row) => (row.Reverse().ToArray())
    ).ToArray();

Tree[][][] dirs = new Tree[4][][]{fromRight, fromLeft, fromTop, fromBottom};

//1
HashSet<Tree> CountsFromSightLine(Tree[][] lines)
{
    HashSet<Tree> visibles = new();
    foreach (Tree[] line in lines)
    {
        int tallest = -1;
        foreach (Tree tree in line)
        {
            if (tree.height > tallest)
            {
                visibles.Add(tree);
                tallest = tree.height;
            }
        }
    }
    return visibles;
}

int num_visible = dirs
    .Select(d => CountsFromSightLine(d))
    .Aggregate((a, b) => a.Union(b).ToHashSet())
    .Count();
Console.WriteLine(num_visible);

//2
Dictionary<Tree, int> ScoresFromSightLine(Tree[][] lines)
{
    Dictionary<Tree, int> scores = new();
    foreach (Tree[] line in lines)
    {
        Dictionary<int, int> lastOfHeightN = Enumerable.Range(0,10).ToDictionary(x => x, x => 0);
        int cur_pos = 0;
        foreach (Tree tree in line)
        {
            int th = tree.height;
            int blocker = Enumerable.Range(th, 10-th).Select(h => lastOfHeightN[h]).Max();
            int score = cur_pos - blocker;

            scores[tree] = score;
            lastOfHeightN[th] = cur_pos;
            cur_pos++;
        }
    }
    return scores;
}

Dictionary<Tree, int> Multiply(Dictionary<Tree, int> a, Dictionary<Tree, int> b)
{
    Dictionary<Tree, int> product = new();
    foreach (Tree tree in a.Keys)
    {
        product.Add(tree, a[tree] * b[tree]);
    }
    return product;
}

int max_score = dirs
    .Select(d => ScoresFromSightLine(d))
    .Aggregate(Multiply)
    .Select(entry => entry.Value)
    .Max();
Console.WriteLine(max_score);


readonly record struct Tree(int height, int x, int y);