namespace AoC
{
    public class Day1
    {
        public static void Go()
        {
            string[] input = File.ReadAllLines("1.txt");

            string[] digits = ["1", "2", "3", "4", "5", "6", "7", "8", "9"];
            int sum1 = input.Select(l => GetCalVal(l, digits)).Sum();
            Console.WriteLine(sum1);

            string[] words = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
            int sum2 = input.Select(l => GetCalVal(l, digits.Union(words))).Sum();
            Console.WriteLine(sum2);
        }

        static int GetCalVal(string line, IEnumerable<string> patterns)
        {
            string first = patterns.Select(p => (p, i: line.IndexOf(p))).Where(x => x.i != -1).MinBy(x => x.i).p;
            string last = patterns.Select(p => (p, i: line.LastIndexOf(p))).Where(x => x.i != -1).MaxBy(x => x.i).p;
            
            return NumFromString(first) * 10 + NumFromString(last);
        }

        static int NumFromString(string s)
        {
            if (int.TryParse(s, out int n))
            {
                return n;
            }
            else return s switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                _ => throw new FormatException()
            };
        }
    }
}