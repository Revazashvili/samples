#[macro_use]
extern crate diesel;
//extern crate serde_derive;
extern crate dotenv;

mod db;
use db::*;
use actix_web::{web, App, HttpResponse, HttpServer, Responder};

async fn index() -> impl Responder {
    let posts = get_posts();
    HttpResponse::Ok().json(posts)
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    HttpServer::new(|| {
        App::new()
            .route("/", web::get().to(index))
    })
        .bind("127.0.0.1:7878")?
        .run()
        .await
}
