pub fn go(input: String)
{
    let digits = ["1", "2", "3", "4", "5", "6", "7", "8", "9"];
    let sum1 : u32 = input.lines().map(|l| get_cal_val(l, digits.iter())).sum();
    println!("{sum1}");

    let words = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
    let sum2 : u32 = input.lines().map(|l| get_cal_val(l, digits.iter().chain(words.iter()))).sum();
    println!("{sum2}");

}

fn get_cal_val<'a>(line: &str, patterns: impl Iterator<Item = &'a &'static str>) -> u32
{
    let mut matches: Vec<(usize, &str)> = patterns
        .map(|x| line.match_indices(x))
        .flatten()
        .collect();
    matches.sort_by_key(|(i, _)| *i);

    digit_from_string(matches[0].1) * 10 + digit_from_string(matches[matches.len() - 1].1)
}

fn digit_from_string(txt: &str) -> u32
{
    match (txt, txt.parse())
    {
        (_, Ok(n)) => n,
        ("one", _) => 1,
        ("two", _) => 2,
        ("three", _) => 3,
        ("four", _) => 4,
        ("five", _) => 5,
        ("six", _) => 6,
        ("seven", _) => 7,
        ("eight", _) => 8,
        ("nine", _) => 9,
        _ => panic!()
    }
}
