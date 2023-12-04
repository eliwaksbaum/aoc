using System.Text.RegularExpressions;

using Pos = (int r, int c);

namespace AoC
{
    public partial class Day3
    {
        [GeneratedRegex(@"[^\.0-9]")]
        private static partial Regex Symbols();
        [GeneratedRegex(@"\d+")]
        private static partial Regex Numbers();

        public static void Go()
        {
            string[] input = File.ReadAllLines("3.txt");

            HashSet<Pos> symbol_pos = [];
            HashSet<(int num, List<Pos> ps)> number_pos = [];
            Dictionary<Pos, List<int>> gear_map = [];

            
            foreach ((string line, int i) in input.Select((l, i) => (l, i)))
            {
                foreach(Match m in Symbols().Matches(line))
                {
                    symbol_pos.Add((i, m.Index));
                    if (m.Value == "*")
                    {
                        gear_map.Add((i, m.Index), []);
                    }
                }

                foreach (Match m in Numbers().Matches(line))
                {
                    List<Pos> ps = [];
                    for (int j = 0; j < m.Length; j++)
                    {
                        ps.Add((i, m.Index + j));
                    }
                    number_pos.Add((int.Parse(m.Value), ps));
                }
            }

            int part_sum =  number_pos.Where(x => GetNeighbors(x.ps).Any(
                                p => symbol_pos.Contains(p)
                            ))
                            .Select(x => x.num).Sum();
            Console.WriteLine(part_sum);

            foreach (var (num, ps) in number_pos)
            {
                foreach (Pos n in GetNeighbors(ps))
                {
                    if (gear_map.ContainsKey(n))
                    {
                        gear_map[n].Add(num);
                    }
                }
            }
            int gear_sum = gear_map.Where(e => e.Value.Count == 2)
                .Select(e => e.Value.Aggregate(1, (acc, b) => acc * b))
                .Sum();
            Console.WriteLine(gear_sum);
        }

        static HashSet<Pos> GetNeighbors(List<Pos> ps)
        {
            HashSet<Pos> ns = [];
            for (int i = 0; i < ps.Count; i++)
            {
                var (r, c) = ps[i];
                if (i == 0)
                {
                    ns.UnionWith([(r - 1, c - 1), (r, c - 1), (r + 1, c - 1)]);
                }
                ns.UnionWith([(r - 1, c), (r + 1, c)]);
                if (i == ps.Count - 1)
                {
                    ns.UnionWith([(r - 1, c + 1), (r, c + 1), (r + 1, c + 1)]);
                }
            }
            return ns;
        }
    }
}