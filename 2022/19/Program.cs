Console.WriteLine(Harvest(4, 2, 3, 14, 2, 7));

int Harvest(int ore_cost, int clay_cost, int obsidian_ore_cost, int obsidian_clay_cost, int geode_ore_cost, int geode_obs_cost)
{
    int ore_bots = 1;
    int clay_bots = 0;
    int obsidian_bots = 0;
    int geode_bots = 0;

    int ore = 0;
    int clay = 0;
    int obsidian = 0;
    int geodes = 0;

    for (int i = 0; i < 24; i++)
    {
        bool ore_flag = false;
        bool clay_flag = false;
        bool obsidian_flag = false;
        bool geode_flag = false;

        if (obsidian >= geode_obs_cost && ore >= geode_ore_cost)
        {
            geode_flag = true;
            obsidian -= geode_obs_cost;
            ore -= geode_ore_cost;
        }
        else if (clay >= obsidian_clay_cost && ore >= obsidian_ore_cost)
        {
            obsidian_flag = true;
            clay -= obsidian_clay_cost;
            ore -= obsidian_ore_cost;
        }
        else if (ore >= clay_cost)
        {
            clay_flag = true;
            ore -= clay_cost;
        }

        ore += ore_bots;
        clay += clay_bots;
        obsidian += obsidian_bots;
        geodes += geode_bots;

        if (clay_flag)
        {
            clay_bots++;
        }
        if (obsidian_flag)
        {
            obsidian_bots++;
        }
        if (geode_flag)
        {
            geode_bots++;
        }
    }
    return geodes;
}