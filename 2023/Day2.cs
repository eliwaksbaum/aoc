using System.Text.RegularExpressions;

namespace AoC
{
    public partial class Day2
    {
        public static void Go()
        {
            string[] input = File.ReadAllLines("2.txt");

            int id_sum = input.Select((l, i) => (g: GetMaxes(l), i: i+1))
                .Where(x => IsPossible(x.g, new(12, 13, 14)))
                .Select(x => x.i)
                .Sum();
            Console.WriteLine(id_sum);

            int power_sum = input.Select(l => Power(GetMaxes(l))).Sum();
            Console.WriteLine(power_sum);
        }

        static bool IsPossible (Triple maxes, Triple onlies)
        {
            return maxes.Red <= onlies.Red && maxes.Green <= onlies.Green && maxes.Blue <= onlies.Blue;
        }

        static int Power(Triple maxes)
        {
            return maxes.Red * maxes.Green * maxes.Blue;
        }

        [GeneratedRegex(@"(\d+)\sred")]
        private static partial Regex Redex();
        [GeneratedRegex(@"(\d+)\sgreen")]
        private static partial Regex GreenEx();
        [GeneratedRegex(@"(\d+)\sblue")]
        private static partial Regex Blueex();
        static Triple GetMaxes(string game)
        {
            int r = Redex().Matches(game).Select(m => int.Parse(m.Groups[1].Value)).Max();

            int g = GreenEx().Matches(game).Select(m => int.Parse(m.Groups[1].Value)).Max();

            int b = Blueex().Matches(game).Select(m => int.Parse(m.Groups[1].Value)).Max();

            return new(r, g, b);
        }
    }

    record struct Triple(int Red, int Green, int Blue);
}