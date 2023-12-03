use std::fs;

mod day_1;

fn main()
{
    day_1::go(fs::read_to_string("1.txt").unwrap())
}