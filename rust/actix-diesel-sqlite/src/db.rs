pub mod models;
pub mod schema;

use diesel::prelude::*;
use dotenv::dotenv;
use std::env;
use models::*;
use schema::posts::dsl::*;


fn establish_connection() -> SqliteConnection {
    dotenv().ok();

    let database_url = env::var("DATABASE_URL").unwrap_or_else(|_| String::from("test.db"));
    SqliteConnection::establish(&database_url)
        .unwrap_or_else(|_| panic!("Error connecting to {}", 
                                   database_url))
}

pub fn get_posts() -> Vec<Post> {
    let posts_result = posts
        .filter(published.eq(true))
        .limit(5)
        .load::<Post>(&establish_connection());

    match posts_result{
        Ok(ps) => ps,
        Err(e) => {
            eprintln!("Error loading posts: {}", e);
            Vec::new()
        }
    }
}

