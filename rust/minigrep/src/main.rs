mod lib;

use std::env;
use std::process;

use lib::{search,Config};

fn main() {
    let config = Config::new(env::args().collect())
        .unwrap_or_else(|err| {
            println!("Problem parsing arguments: {}",err);
            process::exit(1)
        });

    println!("Searching for {}",config.query);
    println!("In file {}",config.filename);

    if let Err(e) = search(config){
        println!("Application error: {}",e);
        process::exit(1);
    }
}
