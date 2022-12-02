string[] lines = File.ReadAllLines("input.txt");

Dictionary<char, int> them_to_num = new() {{'A', 0}, {'B', 1}, {'C', 2}};
Dictionary<int, int> result_to_points = new() {{0, 3}, {1, 0}, {2, 6}};

//1
Dictionary<char, int> me_to_num = new() {{'X', 0}, {'Y', 1}, {'Z', 2}};

int PointsFromRound1(char them, char me)
{
    int them_num = them_to_num[them];
    int me_num = me_to_num[me];
    int result_num = (them_num - me_num + 3) % 3;
    int result_points = result_to_points[result_num];
    return result_points + me_num + 1;
}

int score1 = lines.Select((l, i) => PointsFromRound1(l[0], l[2])).Sum();
Console.WriteLine(score1);

//2
Dictionary<char, int> result_to_num = new() {{'X', 1}, {'Y', 0}, {'Z', 2}};

int PointsFromRound2(char them, char result)
{
    int them_num = them_to_num[them];
    int result_num = result_to_num[result];
    int me_num = (them_num - result_num + 3) % 3;
    int result_points = result_to_points[result_num];
    return result_points + me_num + 1;
}

int score2 = lines.Select((l, i) => PointsFromRound2(l[0], l[2])).Sum();
Console.WriteLine(score2);