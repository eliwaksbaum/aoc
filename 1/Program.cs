string input = File.ReadAllText("input.txt");
string[] elves = input.Split("\n\n");

List<int> calories = elves.Select((elf, i) => elf.Split("\n"))
    .Select((snacks, i) => 
        snacks.Select((snack, j) => Int32.Parse(snack))
            .Sum()
    ).ToList();
    
//1
Console.WriteLine(calories.Max());

//2
int top3 = calories.Order().Reverse().Take(3).Sum();
Console.WriteLine(top3);