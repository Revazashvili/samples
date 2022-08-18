use std::env;
use std::fs;
use std::process;

fn main() {
    let config = Config::new(env::args().collect())
        .unwrap_or_else(|err| {
            println!("Problem parsing arguments: {}",err);
            process::exit(1)
        });

    println!("Searching for {}",config.query);
    println!("In file {}",config.filename);

    let contents = fs::read_to_string(config.filename)
        .expect("something went wrong while reading file");

    println!("With text: \n {}",contents);

    let contains = contents.contains(&config.query);
    println!("Contains: {}",contains);

}

struct Config {
    query: String,
    filename: String
}

impl Config {
    fn new(args: Vec<String>) -> Result<Config,&'static str> {
        if args.len() < 3 {
            return Err("not enough arguments");
        }
        Ok(Config { query:args[1].clone(), filename:args[2].clone() })
    }
}