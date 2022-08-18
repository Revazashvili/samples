use std::fs;
use std::error::Error;

pub fn search(config: Config) -> Result<(),Box<dyn Error>>{
    let contents = fs::read_to_string(config.filename)?;
    println!("With text: \n {}",contents);
    println!("Contains: {}",contents.contains(&config.query));
    Ok(())
}

pub struct Config {
    pub query: String,
    pub filename: String
}

impl Config {
    pub fn new(args: Vec<String>) -> Result<Config,&'static str> {
        if args.len() < 3 {
            return Err("not enough arguments");
        }
        Ok(Config { query:args[1].clone(), filename:args[2].clone() })
    }
}