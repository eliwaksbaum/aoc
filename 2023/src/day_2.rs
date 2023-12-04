pub fn go(input: String)
{
    let games: Vec<Game> = input.lines()
        .enumerate()
        .map(|l| line_to_game(l))
        .collect();
                   
    let sum1: usize = games.iter()
        .filter(|g| is_possible(g, (12, 13, 14)))
        .map(|g| g.id)
        .sum();
    println!("{sum1}");

    let sum2: u32 = games.iter()
        .map(|g| power(g))
        .sum();
    println!("{sum2}");
}

struct Round
{
    red: Option<u32>,
    green: Option<u32>,
    blue: Option<u32>
}
struct Game
{
    id: usize,
    rounds: Vec<Round>
}

fn power(game: &Game) -> u32
{
    game.rounds.iter().filter_map(|rd| rd.red).max().unwrap() *
    game.rounds.iter().filter_map(|rd| rd.green).max().unwrap() *
    game.rounds.iter().filter_map(|rd| rd.blue).max().unwrap()
}

fn is_possible(game: &Game, (r, g, b): (u32, u32, u32)) -> bool
{
    game.rounds.iter().filter_map(|rd| rd.red).max().unwrap() <= r &&
    game.rounds.iter().filter_map(|rd| rd.green).max().unwrap() <= g &&
    game.rounds.iter().filter_map(|rd| rd.blue).max().unwrap() <= b
}

fn line_to_game((i, line): (usize, &str)) -> Game
{
    let content = line.split(":").last().unwrap();
    Game {id: i + 1, rounds: content.split(";").map(|s| text_to_round(s)).collect()}
}

fn text_to_round(txt: &str) -> Round
{
    let subsets = txt.split(",").map(|s| s.split_whitespace().collect::<Vec<&str>>());

    let (mut r, mut g, mut b) = ("", "", "");

    for x in subsets
    {
        match x[1]
        {
            "red" => {r = x[0]},
            "green" => {g = x[0]},
            "blue" => {b = x[0]},
            _ => panic!()
        }
    }

    Round {red: r.parse().ok(), green: g.parse().ok(), blue: b.parse().ok()}
}