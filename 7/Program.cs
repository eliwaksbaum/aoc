string[] log = File.ReadAllLines("input.txt");

Directory root = new Directory("/");

Directory cur = root;
foreach (string line in log)
{
    string[] words = line.Split(' ');
    switch (line[0])
    {
        case '$':
            if (words[1] == "cd")
            {
                switch (words[2])
                {
                    case "..":
                        cur = cur.parent;
                        break;
                    case "/":
                        cur = root;
                        break;
                    default:
                        cur = cur.children[words[2]];
                        break;
                }
            }
            break;
        case 'd':
            Directory child = new Directory(cur, words[1]);
            cur.children.Add(words[1], child);
            break;
        default:
            cur.files.Add(words[1], Int32.Parse(words[0]));
            break;
    }
}

List<int> allSizes = new();
int GetSize(Directory node)
{
    int cur_size = node.fileSize;
    foreach (Directory child in node.children.Values)
    {
        cur_size += GetSize(child);
    }
    allSizes.Add(cur_size);
    return cur_size;
}
GetSize(root);

//1
int smallSum = allSizes.Where((s) => s <= 100000).Sum();
Console.WriteLine(smallSum);

//2
int free= 70000000 - allSizes.Last();
int required = 30000000 - free;
int deleteSize = allSizes.Where((s) => s >= required).Min();
Console.WriteLine(deleteSize);

class Directory
{
    public readonly string name;
    public readonly Directory? parent;
    public readonly Dictionary<string, Directory> children = new();
    public readonly Dictionary<string, int> files = new();

    public int fileSize => files.Values.Sum();

    public Directory(Directory parent, string name)
    {
        this.parent = parent;
        this.name = $"{parent.name}/{name}";
    }
    public Directory(string name)
    {
        this.name = name;
    }
}