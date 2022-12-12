string[] input = File.ReadAllLines("input.txt");

int[] payloads = input.SelectMany(l => ToAddsPerCycle(l)).ToArray();

List<int> cyclesL = new(){};
int cur = 1;
for (int i = 0; i < payloads.Length; i++)
{
    cyclesL.Add(cur);
    cur += payloads[i];
}
int[] cycles = cyclesL.ToArray();

//1
int total = 0;
for (int i = 19; i <= 219; i += 40)
{
    total += cycles[i] * (i+1);
}
Console.WriteLine(total);

//2
char[][] screen = new char[6][];
for (int i = 0; i < 6; i ++)
{
    screen[i] = new char[40];
}

for (int i = 0; i < 240; i++)
{
    int sprite_pos = cycles[i];
    int column = i % 40;
    screen[i/40][column] = (Math.Abs(column - sprite_pos) <= 1)? '#' : ' ';
}

foreach(char[] row in screen)
{
    Console.WriteLine(row);
}


List<int> ToAddsPerCycle(string line)
{
    List<int> adds = new();
    string[] splits = line.Split(' ');

    adds.Add(0);
    if (splits[0] == "addx")
    {
        int load = int.Parse(splits[1][0..^0]);
        adds.Add(load);
    }
    return adds;
}