namespace AoC
{
    public class Day4
    {
        public static void Go()
        {
            string[] input = File.ReadAllLines("4.txt");
            int[] matches = input.Select(Matches).ToArray();

            double points = matches.Select(m => m >= 1 ? Math.Pow(2, m-1) : 0).Sum();
            Console.WriteLine(points);

            Console.WriteLine(Cards(matches));
        }


        static int Matches(string card)
        {
            var halfs = card.Split('|');
            HashSet<int> winners = halfs[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(2).Select(int.Parse).ToHashSet();
            HashSet<int> haves = halfs[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();

            winners.IntersectWith(haves);
            return winners.Count;
        }

        static int Cards(int[] matches)
        {
            int[] counts = new int[matches.Length];

            for (int i = 0; i < matches.Length; i++)
            {
                counts[i]++;
                int m = matches[i];

                for (int j = 1; j <= m; j++)
                {
                    counts[i+j] += counts[i];
                }
            }
            return counts.Sum();
        }
    }
}