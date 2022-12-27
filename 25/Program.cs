double fuel_sum = File.ReadAllLines("input.txt").Select(x => SNAFUtoDecimal(x)).Sum();

Console.WriteLine(DecimaltoSNAFU(fuel_sum));

double SNAFUtoDecimal(string snafu)
{
    double dec = 0;
    for (int i = 0; i < snafu.Length; i++)
    {
        double place = Math.Pow(5, i);
        double value = snafu[snafu.Length - 1 - i] switch {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '-' => -1,
            '=' => -2,
            _ => throw new Exception()
        };
        dec += value * place;
    }
    return dec;
}

string DecimaltoSNAFU(double dec)
{
    double cur = dec;
    string snafu = "";
    while (cur != 0)
    {
        double dividend = Math.Floor(cur / 5);
        double remainder = cur - (dividend * 5);
        string digit = remainder.ToString();

        if (remainder == 3)
        {
            digit = "=";
            dividend += 1;
        }
        else if (remainder == 4)
        {
            digit = "-";
            dividend += 1;
        }

        snafu = digit + snafu;
        cur = dividend;
    }
    return snafu;
}
